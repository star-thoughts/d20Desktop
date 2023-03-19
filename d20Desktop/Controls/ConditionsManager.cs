using Fiction.GameScreen.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    public sealed class ConditionsManager : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="ConditionsManager"/> class
        /// </summary>
        static ConditionsManager()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConditionsManager), new FrameworkPropertyMetadata(typeof(ConditionsManager)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the conditions to manage
        /// </summary>
        public ConditionsViewModel Conditions
        {
            get { return (ConditionsViewModel)GetValue(ConditionsProperty); }
            set { SetValue(ConditionsProperty, value); }
        }
        /// <summary>
        /// Gets or sets the currently selected condition
        /// </summary>
        public Combat.Condition? SelectedCondition
        {
            get { return (Combat.Condition?)GetValue(SelectedConditionProperty); }
            set { SetValue(SelectedConditionProperty, value);}
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Conditions"/>
        /// </summary>
        public static readonly DependencyProperty ConditionsProperty = DependencyProperty.Register(nameof(Conditions), typeof(ConditionsViewModel), typeof(ConditionsManager));
        /// <summary>
        /// Gets or sets the currently selected condition
        /// </summary>
        public static readonly DependencyProperty SelectedConditionProperty  = DependencyProperty.Register(nameof(SelectedCondition), typeof(Combat.Condition), typeof(ConditionsManager));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CommandBindings.Add(new CommandBinding(Commands.Add, AddCommand_Executed, AddCommand_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Remove, RemoveCommand_Executed, RemoveCommand_CanExecute));
        }

        private void RemoveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (Conditions?.Conditions != null
                && SelectedCondition != null)
            {
                Combat.Condition? next = Conditions.Conditions?.AfterOrFirstOrDefault(SelectedCondition);
                Conditions.Conditions?.Remove(SelectedCondition);
                SelectedCondition = next;
            }
        }

        private void RemoveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = SelectedCondition != null && Conditions?.Conditions != null;
        }

        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (Conditions?.Conditions != null)
            {
                SelectedCondition = new Combat.Condition(string.Empty, string.Empty);
                Conditions.Conditions.Add(SelectedCondition);
            }
        }

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Conditions?.Conditions != null;
        }
        #endregion
    }
}
