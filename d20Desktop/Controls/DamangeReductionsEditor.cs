using Fiction.GameScreen.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for editing a list of damage reductions
    /// </summary>
    public sealed class DamageReductionsEditor : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="DamageReductionsEditor"/> class
        /// </summary>
        static DamageReductionsEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DamageReductionsEditor), new FrameworkPropertyMetadata(typeof(DamageReductionsEditor)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the damage reduction information to edit
        /// </summary>
        public EditDamageReductionsViewModel DamageReductions
        {
            get { return (EditDamageReductionsViewModel)GetValue(DamageReductionsProperty); }
            set { SetValue(DamageReductionsProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="DamageReductions"/>
        /// </summary>
        public static readonly DependencyProperty DamageReductionsProperty = DependencyProperty.Register(nameof(DamageReductions), typeof(EditDamageReductionsViewModel), typeof(DamageReductionsEditor));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CommandBindings.Add(new CommandBinding(Commands.Add, AddDamageReduction_Executed, AddDamageReduction_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Remove, RemoveDamageReduction_Executed, RemoveDamageReduction_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Edit, EditDamageReduction_Executed, EditDamageReduction_CanExecute));
        }

        private bool EditDamageReduction(EditDamageReductionViewModel vm)
        {
            EditWindow window = new EditWindow();
            window.Owner = Window.GetWindow(this);
            window.DataContext = vm;

            if (window.ShowDialog() == true)
            {
                vm.Save();
                return true;
            }
            return false;
        }

        private void AddDamageReduction_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (DamageReductions != null)
                {
                    EditDamageReductionViewModel vm = new EditDamageReductionViewModel();
                    if (EditDamageReduction(vm))
                        DamageReductions.DamageReductions.Add(vm);
                }
            });
        }

        private void AddDamageReduction_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = DamageReductions != null;
        }

        private void RemoveDamageReduction_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (DamageReductions != null && e.Parameter is EditDamageReductionViewModel vm)
                    DamageReductions.DamageReductions.Remove(vm);
            });
        }

        private void RemoveDamageReduction_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = DamageReductions != null && e.Parameter is EditDamageReductionViewModel;
        }

        private void EditDamageReduction_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (DamageReductions != null && e.Parameter is EditDamageReductionViewModel vm)
                    EditDamageReduction(vm);
            });
        }

        private void EditDamageReduction_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = DamageReductions != null && e.Parameter is EditDamageReductionViewModel;
        }
        #endregion
    }
}
