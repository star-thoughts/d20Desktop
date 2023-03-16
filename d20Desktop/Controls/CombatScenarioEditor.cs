using Fiction.GameScreen.Combat;
using Fiction.GameScreen.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Removeor for a combat scenario
    /// </summary>
    public sealed class CombatScenarioEditor : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="CombatScenarioEditor"/> class
        /// </summary>
        static CombatScenarioEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CombatScenarioEditor), new FrameworkPropertyMetadata(typeof(CombatScenarioEditor)));
        }
        #endregion
        #region Member Variables
        private TextBox _countTextBox;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the scenario being Removeed
        /// </summary>
        public CombatScenarioEditViewModel Scenario
        {
            get { return (CombatScenarioEditViewModel)GetValue(ScenarioProperty); }
            set { SetValue(ScenarioProperty, value); }
        }
        /// <summary>
        /// Gets or sets the selected combatant
        /// </summary>
        public CombatantTemplateEditViewModel SelectedCombatant
        {
            get { return (CombatantTemplateEditViewModel)GetValue(SelectedCombatantProperty); }
            set { SetValue(SelectedCombatantProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Scenario"/>
        /// </summary>
        public static readonly DependencyProperty ScenarioProperty = DependencyProperty.Register(nameof(Scenario), typeof(CombatScenarioEditViewModel), typeof(CombatScenarioEditor));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedCombatant"/>
        /// </summary>
        public static readonly DependencyProperty SelectedCombatantProperty = DependencyProperty.Register(nameof(SelectedCombatant), typeof(CombatantTemplateEditViewModel), typeof(CombatScenarioEditor));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            CommandBindings.Add(new CommandBinding(Commands.Add, AddCommand_Executed, AddCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Remove, RemoveCommand_Executed, RemoveCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.ChooseSource, ChooseSource_Executed, ChooseSource_CanExecute));

            _countTextBox = Template.FindName("PART_CountTextBox", this) as TextBox;
        }

        private void ChooseSource_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is CombatantTemplateEditViewModel combatant)
                {
                    ChooseCombatantTemplateSourceViewModel vm = new ChooseCombatantTemplateSourceViewModel(Scenario.Campaign);
                    EditWindow window = new EditWindow();
                    window.Owner = Window.GetWindow(this);
                    window.DataContext = vm;

                    if (window.ShowDialog() == true)
                    {
                        combatant.Source = vm.SelectedSource;
                        if (_countTextBox != null)
                        {
                            _countTextBox.Focus();
                            _countTextBox.SelectAll();
                        }
                    }
                }
            });
        }

        private void ChooseSource_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.Parameter is CombatantTemplateEditViewModel;
        }

        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;

                CombatantTemplateEditViewModel combatant = new CombatantTemplateEditViewModel(new CombatantTemplate(Scenario.Campaign));
                Scenario.Combatants.Add(combatant);
                SelectedCombatant = combatant;
            });
        }

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = true;
        }

        private void RemoveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (SelectedCombatant != null)
                {
                    if (Scenario.Campaign.Combat.CanDeleteCombatantTemplate(SelectedCombatant.Combatant))
                    {
                        CombatantTemplateEditViewModel combatant = Scenario.Combatants.AfterOrFirstOrDefault(SelectedCombatant);
                        Scenario.Combatants.Remove(SelectedCombatant);
                        SelectedCombatant = combatant;
                    }
                    else
                    {
                        string message = GameScreen.Resources.Resources.CannotDeleteCombatantMessage;
                        Window window = Window.GetWindow(this);
                        MessageBox.Show(window, message, window.Title, MessageBoxButton.OK);
                    }
                }
            });
        }

        private void RemoveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = SelectedCombatant != null;
        }
        #endregion
    }
}
