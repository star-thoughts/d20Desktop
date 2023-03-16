using Fiction.GameScreen.Combat;
using Fiction.GameScreen.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for displaying damage reduction information on a single line, and allows editing of it
    /// </summary>
    public sealed class DamageReductionLine : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="DamageReductionLine"/> class
        /// </summary>
        static DamageReductionLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DamageReductionLine), new FrameworkPropertyMetadata(typeof(DamageReductionLine)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the damage reduction to display and edit
        /// </summary>
        public ObservableCollection<DamageReduction> DamageReduction
        {
            get { return (ObservableCollection<DamageReduction>)GetValue(DamageReductionProperty); }
            set { SetValue(DamageReductionProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="DamageReduction"/>
        /// </summary>
        public static readonly DependencyProperty DamageReductionProperty = DependencyProperty.Register(nameof(DamageReduction), typeof(ObservableCollection<DamageReduction>), typeof(DamageReductionLine));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CommandBindings.Add(new CommandBinding(Commands.Edit, EditDamageReduction_Executed, EditDamageReduction_CanExecute));
        }

        private void EditDamageReduction_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (DamageReduction != null)
            {
                EditDamageReductionsViewModel vm = new EditDamageReductionsViewModel(DamageReduction);

                EditWindow window = new EditWindow();
                window.Owner = Window.GetWindow(this);
                window.DataContext = vm;

                if (window.ShowDialog() == true)
                    vm.Save();
            }
        }

        private void EditDamageReduction_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = DamageReduction != null;
        }
        #endregion
    }
}
