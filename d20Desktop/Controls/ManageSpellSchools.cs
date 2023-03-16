using Fiction.GameScreen.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for managing schools and sub-schools
    /// </summary>
    public sealed class ManageSpellSchools : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="ManageSpellSchools"/> class
        /// </summary>
        static ManageSpellSchools()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ManageSpellSchools), new FrameworkPropertyMetadata(typeof(ManageSpellSchools)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model for managing schools
        /// </summary>
        public ManageSpellSchoolsViewModel ViewModel
        {
            get { return (ManageSpellSchoolsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModelProperty"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ManageSpellSchoolsViewModel), typeof(ManageSpellSchools));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            CommandBindings.Add(new CommandBinding(Commands.Add, Add_Executed, Add_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Remove, Remove_Executed, Remove_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Edit, Edit_Executed, Edit_CanExecute));
        }

        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if ((e.OriginalSource is Button button) && button.DataContext is string type)
                {
                    EnterValueViewModel vm = new EnterValueViewModel();
                    EditWindow window = new EditWindow();
                    window.Owner = Window.GetWindow(this);
                    window.SizeToContent = SizeToContent.Height;
                    window.DataContext = vm;

                    if (window.ShowDialog() == true)
                    {
                        switch (type)
                        {
                            case "Schools":
                                ViewModel.SpellManager.AddSchool(vm.Value);
                                break;
                            case "SubSchools":
                                ViewModel.SpellManager.AddSubSchool(vm.Value);
                                break;
                            case "EffectTypes":
                                ViewModel.SpellManager.AddEffectType(vm.Value);
                                break;
                        }
                    }
                }
            });
        }

        private void Add_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = ViewModel != null;
        }

        private void Remove_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if ((e.OriginalSource is Button button) && button.DataContext is string type && e.Parameter is string value)
                {
                    switch (type)
                    {
                        case "Schools":
                            ViewModel.SpellManager.RemoveSchool(value);
                            break;
                        case "SubSchools":
                            ViewModel.SpellManager.RemoveSubSchool(value);
                            break;
                        case "EffectTypes":
                            ViewModel.SpellManager.RemoveEffectType(value);
                            break;
                    }
                }
            });
        }

        private void Remove_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.Parameter is string;
        }

        private void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                if (e.OriginalSource is Button button && button.DataContext is string type && e.Parameter is string value)
                {
                    EnterValueViewModel vm = new EnterValueViewModel(value);
                    EditWindow window = new EditWindow();
                    window.Owner = Window.GetWindow(this);
                    window.SizeToContent = SizeToContent.Height;
                    window.DataContext = vm;

                    if (window.ShowDialog() == true)
                    {
                        switch (type)
                        {
                            case "Schools":
                                ViewModel.SpellManager.ReplaceSchool(value, vm.Value);
                                break;
                            case "SubSchools":
                                ViewModel.SpellManager.ReplaceSubSchool(value, vm.Value);
                                break;
                            case "EffectTypes":
                                ViewModel.SpellManager.ReplaceEffectType(value, vm.Value);
                                break;
                        }
                    }
                }
            });
        }

        private void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.Parameter is string;
        }
        #endregion
    }
}
