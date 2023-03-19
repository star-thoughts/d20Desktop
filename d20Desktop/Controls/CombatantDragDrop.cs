using Fiction.GameScreen.Combat;
using Fiction.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Handles drag and drop operations on combatant lists
    /// </summary>
    public static class CombatantDragDrop
    {
        private static Point? _startPoint;
        private static ItemsControlDropAdorner? _adorner;

        public static readonly DependencyProperty CombatantsProperty = DependencyProperty.RegisterAttached("Combatants", typeof(IInitiativeCollection), typeof(CombatantDragDrop),
            new FrameworkPropertyMetadata(null, CombatantsChanged));

        /// <summary>
        /// Sets the associated combatants collection to the given ItemsControl
        /// </summary>
        /// <param name="control">Control to set to</param>
        /// <param name="collection">Value to set</param>
        public static void SetCombatants(ItemsControl control, IInitiativeCollection collection)
        {
            Exceptions.ThrowIfArgumentNull(control, nameof(control));

            control.SetValue(CombatantsProperty, collection);
        }

        /// <summary>
        /// Gets the combatants collection associated with the given control
        /// </summary>
        /// <param name="control">Control to get combatants collection for</param>
        /// <returns>Collection of combatants</returns>
        public static IInitiativeCollection GetCombatants(ItemsControl control)
        {
            Exceptions.ThrowIfArgumentNull(control, nameof(control));

            return (IInitiativeCollection)control.GetValue(CombatantsProperty);
        }

        private static void CombatantsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (d is ItemsControl list)
                {
                    if (e.NewValue != null)
                    {
                        list.PreviewMouseLeftButtonDown += List_PreviewMouseDown;
                    }
                    else
                    {
                        list.PreviewMouseLeftButtonDown -= List_PreviewMouseDown;
                    }
                }
            });
        }

        private static void List_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (sender is ItemsControl itemControl && _startPoint.HasValue)
                {
                    Point currentPoint = e.GetPosition(itemControl);
                    if ((currentPoint - _startPoint.Value).Length > 5)
                    {
                        FrameworkElement? container = VisualTreeHelperEx.GetItemsControlContainerAtPoint(itemControl, _startPoint.Value);
                        IActiveCombatant? combatant = container?.DataContext as IActiveCombatant;

                        itemControl.AllowDrop = true;
                        itemControl.Drop += ItemControl_Drop;
                        itemControl.DragEnter += ItemControl_DragEnter;
                        itemControl.DragLeave += ItemControl_DragLeave;
                        try
                        {
                            DragDrop.DoDragDrop(itemControl, new DataObject(typeof(IActiveCombatant), combatant), DragDropEffects.Move);
                        }
                        finally
                        {
                            itemControl.AllowDrop = false;
                            itemControl.Drop -= ItemControl_Drop;
                            itemControl.DragEnter -= ItemControl_DragEnter;
                            itemControl.DragLeave -= ItemControl_DragLeave;
                            _startPoint = null;
                            _adorner?.Dispose();
                        }
                    }
                }
            });
        }

        private static void ItemControl_DragLeave(object sender, DragEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                _adorner?.Dispose();
            });
        }

        private static void ItemControl_DragEnter(object sender, DragEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (sender is ItemsControl itemControl)
                {
                    _adorner?.Dispose();
                    _adorner = new ItemsControlDropAdorner(itemControl, itemControl.GetOrientation());
                    _adorner.LineBrush = Brushes.White;
                }
            });
        }

        private static void ItemControl_Drop(object sender, DragEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (sender is ItemsControl itemControl && _adorner != null && _adorner.IsDropValid)
                {
                    IActiveCombatant? target = _adorner.DropTarget as IActiveCombatant;
                    IActiveCombatant? combatant = e.Data.GetData(typeof(IActiveCombatant)) as IActiveCombatant;

                    if (combatant != null)
                    {
                        IInitiativeCollection collection = GetCombatants(itemControl);
                        if (target != null)
                            collection.MoveBefore(combatant, target);
                        else
                            collection.MoveToEnd(combatant);
                    }
                }
            });
        }

        private static void List_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (sender is IInputElement element)
                {
                    _startPoint = e.GetPosition(element);
                    element.PreviewMouseMove += List_PreviewMouseMove;
                    element.PreviewMouseLeftButtonUp += Element_PreviewMouseLeftButtonUp;
                }
            });
        }

        private static void Element_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (sender is IInputElement element)
                {
                    _startPoint = null;
                    element.PreviewMouseMove -= List_PreviewMouseMove;
                    element.PreviewMouseLeftButtonUp -= Element_PreviewMouseLeftButtonUp;
                }
            });
        }
    }
}
