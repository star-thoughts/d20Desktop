using Fiction.GameScreen.Combat;
using Fiction.GameScreen.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control to manage combat and combatants
    /// </summary>
    /// <remarks>
    /// Standard combat and combatant controls appear in the <see cref="ActiveCombat"/> view, but this allows for less
    /// frequent controls and options to be used.  It also keeps the main screen from getting too cluttered with options
    /// not frequently used.
    /// </remarks>
    public sealed class ManageCombatants : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes teh <see cref="ManageCombatants"/> class
        /// </summary>
        static ManageCombatants()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ManageCombatants), new FrameworkPropertyMetadata(typeof(ManageCombatants)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model
        /// </summary>
        public ManageCombatantsViewModel ViewModel
        {
            get { return (ManageCombatantsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        /// <summary>
        /// Gets or sets the selected combatant
        /// </summary>
        public ICombatant SelectedCombatant
        {
            get { return (ICombatant)GetValue(SelectedCombatantProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ManageCombatantsViewModel), typeof(ManageCombatants));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedCombatant"/>
        /// </summary>
        public static readonly DependencyProperty SelectedCombatantProperty = DependencyProperty.Register(nameof(SelectedCombatant), typeof(ICombatant), typeof(ManageCombatants));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            CommandBindings.Add(new CommandBinding(Commands.Up, Up_Executed, Up_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Down, Down_Executed, Down_CanExecute));
        }

        private void Up_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is IActiveCombatant combatant && ViewModel.Combat.Combat.CanMoveUp(combatant))
                    ViewModel.Combat.Combat.MoveUp(combatant);
            });
        }

        private void Up_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.Parameter is IActiveCombatant combatant && ViewModel.Combat.Combat.CanMoveUp(combatant);
        }

        private void Down_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is IActiveCombatant combatant && ViewModel.Combat.Combat.CanMoveDown(combatant))
                    ViewModel.Combat.Combat.MoveDown(combatant);
            });
        }

        private void Down_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.Parameter is IActiveCombatant combatant && ViewModel.Combat.Combat.CanMoveDown(combatant);
        }
        #endregion
    }
}
