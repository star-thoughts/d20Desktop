using Fiction.GameScreen.Combat;
using Fiction.GameScreen.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for resolving initiatives before adding to combat
    /// </summary>
    public sealed class ResolveInitiatives : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="ResolveInitiatives"/>
        /// </summary>
        static ResolveInitiatives()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResolveInitiatives), new FrameworkPropertyMetadata(typeof(ResolveInitiatives)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the initiative resolver view model
        /// </summary>
        public ResolveInitiativesViewModel Resolver
        {
            get { return (ResolveInitiativesViewModel)GetValue(ResolverProperty); }
            set { SetValue(ResolverProperty, value); }
        }
        /// <summary>
        /// Gets or sets the currently selected combatant
        /// </summary>
        public CombatantPreparer SelectedCombatant
        {
            get { return (CombatantPreparer)GetValue(SelectedCombatantProperty); }
            set { SetValue(SelectedCombatantProperty, value); }
        }
        /// <summary>
        /// Gets a dictionary containing brushes by initiative roll for coloring groups of initiatives
        /// </summary>
        public Dictionary<int, Brush> InitiativeBrushes
        {
            get { return (Dictionary<int, Brush>)GetValue(InitiativeBrushesProperty); }
            set { SetValue(InitiativeBrushesProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Resolver"/>
        /// </summary>
        public static readonly DependencyProperty ResolverProperty = DependencyProperty.Register(nameof(Resolver), typeof(ResolveInitiativesViewModel), typeof(ResolveInitiatives),
            new FrameworkPropertyMetadata(null, ResolverChanged));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedCombatant"/>
        /// </summary>
        public static readonly DependencyProperty SelectedCombatantProperty = DependencyProperty.Register(nameof(SelectedCombatant), typeof(CombatantPreparer), typeof(ResolveInitiatives));
        /// <summary>
        /// DependencyProperty for <see cref="InitiativeBrushes"/>
        /// </summary>
        public static readonly DependencyProperty InitiativeBrushesProperty = DependencyProperty.Register(nameof(InitiativeBrushes), typeof(Dictionary<int, Brush>), typeof(ResolveInitiatives));

        private static void ResolverChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (d is ResolveInitiatives resolve && e.NewValue is ResolveInitiativesViewModel resolver)
                    resolve.UpdateResolver(resolver);
            });
        }
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            CommandBindings.Add(new CommandBinding(Commands.Up, UpCommand_Executed, UpCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Down, DownCommand_Executed, DownCommand_CanExecute));

            InitiativeBrushConverter? converter = Template.Resources["InitiativeBrush"] as InitiativeBrushConverter;
            if (converter != null)
            {
                Binding binding = new Binding(nameof(InitiativeBrushes));
                binding.Source = this;
                BindingOperations.SetBinding(converter, InitiativeBrushConverter.BrushesProperty, binding);
            }
        }

        private void UpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            if (e.Parameter is CombatantPreparer combatant)
            {
                e.CanExecute = combatant.InitiativeOrder != 1;
            }
        }

        private void UpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is CombatantPreparer combatant)
                {
                    int order = combatant.InitiativeOrder;
                    if (order > 0)
                    {
                        Resolver.Preparer.Preparer.MoveUp(combatant);
                    }
                }
            });
        }

        private void DownCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            if (e.Parameter is CombatantPreparer combatant)
            {
                e.CanExecute = combatant.InitiativeOrder != Resolver.Combatants.Max(p => p.InitiativeOrder);
            }
        }

        private void DownCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is CombatantPreparer combatant)
                {
                    int order = combatant.InitiativeOrder;
                    if (order < Resolver.Combatants.Count)
                    {
                        Resolver.Preparer.Preparer.MoveDown(combatant);
                    }
                }
            });
        }

        private void UpdateResolver(ResolveInitiativesViewModel resolver)
        {
            //  Get one of each initiative roll
            int[] totals = resolver.Preparer.Combatants.Select(p => p.InitiativeTotal)
                .GroupBy(p => p)
                .Where(p => p.Count() > 1)
                .Select(p => p.Key)
                .ToArray();

            Brush[] brushes = new Brush[]
            {
                Brushes.AliceBlue,
                Brushes.Aquamarine,
                Brushes.Blue,
                Brushes.BlueViolet,
                Brushes.Brown,
                Brushes.CadetBlue,
                Brushes.Chocolate,
                Brushes.Cornsilk,
                Brushes.Cyan,
                Brushes.DeepPink,
                Brushes.Firebrick,
                Brushes.ForestGreen,
                Brushes.Fuchsia,
                Brushes.Gold,
                Brushes.Green,
                Brushes.GreenYellow,
                Brushes.Indigo,
                Brushes.Khaki,
                Brushes.LawnGreen,
                Brushes.LimeGreen,
                Brushes.Magenta,
                Brushes.Moccasin,
                Brushes.OliveDrab,
                Brushes.Orange,
                Brushes.OrangeRed,
                Brushes.Purple,
                Brushes.Red,
                Brushes.RosyBrown,
                Brushes.Tan,
                Brushes.Teal,
                Brushes.Violet,
            };

            //  Create colors for them
            Dictionary<int, Brush> totalBrushes = new Dictionary<int, Brush>();
            int index = 0;
            for (int i = 0; i < totals.Length; i++)
            {
                totalBrushes[totals[i]] = brushes[index];

                //  Move to the next color
                index++;
                //  If we run out of colors, start over
                if (index >= brushes.Length)
                    index = 0;
            }

            InitiativeBrushes = totalBrushes;
        }
        #endregion
    }
    #region BrushConverter
    /// <summary>
    /// Converter that gets the appropriate brush for the given converter
    /// </summary>
    public class InitiativeBrushConverter : DependencyObject, IValueConverter
    {
        public Dictionary<int, Brush> Brushes
        {
            get { return (Dictionary<int, Brush>)GetValue(BrushesProperty); }
            set { SetValue(BrushesProperty, value); }
        }
        public static readonly DependencyProperty BrushesProperty = DependencyProperty.Register(nameof(Brushes), typeof(Dictionary<int, Brush>), typeof(InitiativeBrushConverter));

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CombatantPreparer combatant && Brushes != null)
            {
                if (Brushes.TryGetValue(combatant.InitiativeTotal, out Brush? brush))
                    return brush;
                //  If no brush, make it transparent
                return System.Windows.Media.Brushes.Transparent;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
