using Fiction.GameScreen.Spells;
using Fiction.GameScreen.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    public sealed class ManageSpells : Control
    {
        #region Constructors
        static ManageSpells()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ManageSpells), new FrameworkPropertyMetadata(typeof(ManageSpells)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model
        /// </summary>
        public ManageSpellsViewModel ViewModel
        {
            get { return (ManageSpellsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value);}
        }
        /// <summary>
        /// Gets or sets the currently selected spell
        /// </summary>
        public Spell SelectedSpell
        {
            get { return (Spell)GetValue(SelectedSpellProperty); }
            set { SetValue(SelectedSpellProperty, value);}
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ManageSourcesViewModel), typeof(ManageSpells));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedSpell"/>
        /// </summary>
        public static readonly DependencyProperty SelectedSpellProperty = DependencyProperty.Register(nameof(SelectedSpell), typeof(Spell), typeof(ManageSpells));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            CommandBindings.Add(new CommandBinding(Commands.Add, Add_Executed, Add_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Remove, Remove_Executed, Remove_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Edit, Edit_Executed, Edit_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.CreateCopy, Copy_Executed, Copy_CanExecute));
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (ViewModel?.Spells != null && SelectedSpell != null)
                    EditSpell(SelectedSpell, createCopy: true);
            });
        }

        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = ViewModel?.Spells != null && SelectedSpell != null;
        }

        private void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (ViewModel?.Spells != null && SelectedSpell != null)
                {
                    Spell spell = SelectedSpell;
                    EditSpell(spell, createCopy: false);
                }
            });
        }

        private void EditSpell(Spell spell, bool createCopy)
        {
            EditSpellViewModel edit = new EditSpellViewModel(spell, createCopy);
            EditWindow window = new EditWindow();
            window.Owner = Window.GetWindow(this);
            window.DataContext = edit;

            if (window.ShowDialog() == true)
            {
                edit.Save();
                ViewModel.Factory.Campaign.ReconcileSources();
            }
        }

        private void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = ViewModel?.Spells != null && SelectedSpell != null;
        }

        private void Remove_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (ViewModel?.Spells != null && SelectedSpell != null)
                    ViewModel.Spells.Remove(SelectedSpell);
            });
        }

        private void Remove_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = SelectedSpell != null && ViewModel?.Spells != null;
        }

        private void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (ViewModel?.Spells != null && SelectedSpell != null)
                {
                    EditSpellViewModel edit = new EditSpellViewModel(ViewModel.Factory.Campaign);
                    EditWindow window = new EditWindow();
                    window.Owner = Window.GetWindow(this);
                    window.DataContext = edit;

                    if (window.ShowDialog() == true)
                    {
                        edit.Save();
                        ViewModel.Factory.Campaign.ReconcileSources();
                    }
                }
            });
        }

        private void Add_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = ViewModel?.Spells != null;
        }
        #endregion
    }
}
