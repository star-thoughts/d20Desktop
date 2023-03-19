using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Fiction.Windows
{
    /// <summary>
    /// Adorner for drawing a line during a drag/drop operation on an ItemsControl
    /// </summary>
    public sealed class ItemsControlDropAdorner : Adorner, IDisposable
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ItemsControlDropAdorner"/>
        /// </summary>
        /// <param name="adornedElement">ItemsControl to adorn</param>
        /// <param name="orientation">The orientation to use to draw the adorner</param>
        public ItemsControlDropAdorner(ItemsControl adornedElement, Orientation orientation)
            : base(adornedElement)
        {
            AdornerLayer.GetAdornerLayer(AdornedElement)?.Add(this);

            adornedElement.DragOver += AdornedElement_DragOver;

            _pen = new Pen(Brushes.Black, 2);
            Orientation = orientation;
            IsHitTestVisible = false;
        }
        #endregion
        #region Member Variables
        private Pen _pen;
        private bool _before;
        #endregion
        #region Properties
        private FrameworkElement? _target;
        private FrameworkElement? Target
        {
            get { return _target; }
            set
            {
                if (!ReferenceEquals(_target, value))
                {
                    _target = value;
                    InvalidateVisual();
                }
            }
        }
        /// <summary>
        /// Gets the ItemsControl associated with this
        /// </summary>
        public ItemsControl ItemsControl { get { return (ItemsControl)AdornedElement; } }
        /// <summary>
        /// Gets the orientation used to draw the adorner
        /// </summary>
        public Orientation Orientation { get; private set; }
        /// <summary>
        /// Gets or sets the brush used to create the pen
        /// </summary>
        public Brush LineBrush
        {
            get { return _pen.Brush; }
            set
            {
                if (!ReferenceEquals(_pen.Brush, value))
                {
                    _pen = new Pen(value, 2);
                }
            }
        }
        /// <summary>
        /// Gets where items should be inserted after a drop, or null to append
        /// </summary>
        public object? DropTarget { get; private set; }
        /// <summary>
        /// Gets whether or not the current value in <see cref="DropTarget"/> is valid
        /// </summary>
        /// <remarks>
        /// When the value in <see cref="DropTarget"/> is not valid, that means the user isn't hovering over a combatant and no drop line is drawn.  A drop shouldn't
        /// occur in this case.
        /// </remarks>
        public bool IsDropValid { get; private set; }
        #endregion
        #region Methods
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (Target != null)
            {
                Point pt1 = new Point();
                Point pt2 = new Point();

                switch (ItemsControl.GetOrientation())
                {
                    case Orientation.Horizontal:
                        pt1 = new Point(_before ? 0 : Target.RenderSize.Width, 0);
                        pt2 = new Point(_before ? 0 : Target.RenderSize.Width, Target.RenderSize.Height);
                        break;
                    case Orientation.Vertical:
                        pt1 = new Point(0, _before ? 0 : Target.RenderSize.Height);
                        pt2 = new Point(Target.RenderSize.Width, _before ? 0 : Target.RenderSize.Height);
                        break;
                }

                GeneralTransform transform = Target.TransformToAncestor(ItemsControl);
                pt1 = transform.Transform(pt1);
                pt2 = transform.Transform(pt2);

                drawingContext.DrawLine(_pen, pt1, pt2);
            }
        }
        private void Detach()
        {
            AdornedElement.DragOver -= AdornedElement_DragOver;
        }
        private void AdornedElement_DragOver(object sender, DragEventArgs e)
        {
            Point pt = e.GetPosition(ItemsControl);
            Target = VisualTreeHelperEx.GetItemsControlContainerAtPoint(ItemsControl, pt);
            if (Target != null)
            {
                _before = GetIsBefore(Target, pt);
                DropTarget = GetDropTarget(Target, _before);
                IsDropValid = true;
            }
            else
            {
                DropTarget = null;
                IsDropValid = false;
            }
        }

        private object? GetDropTarget(FrameworkElement target, bool before)
        {
            object? result = target.DataContext;
            if (!before)
            {
                int index = ItemsControl.ItemContainerGenerator.IndexFromContainer(target);
                result = ItemsControl.Items.OfType<object>()
                    .AfterOrDefault(result);
            }
            return result;
        }

        private bool GetIsBefore(FrameworkElement target, Point pt)
        {
            pt = ItemsControl.TransformToDescendant(target).Transform(pt);
            bool before = false;
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    before = pt.X < target.RenderSize.Width / 2;
                    break;
                case Orientation.Vertical:
                    before = pt.Y < target.RenderSize.Height / 2;
                    break;
            }
            return before;
        }

        public void Dispose()
        {
            Detach();
            AdornerLayer.GetAdornerLayer(ItemsControl)?.Remove(this);
        }
        #endregion
    }
}
