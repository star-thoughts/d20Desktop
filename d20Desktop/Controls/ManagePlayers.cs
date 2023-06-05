using Fiction.GameScreen.Players;
using Fiction.GameScreen.ViewModels;
using Fiction.Windows;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Control for managing players in a campaign
    /// </summary>
    public sealed class ManagePlayers : Control
    {
        #region Constructors
        /// <summary>
        /// Initializes the <see cref="ManagePlayers"/> class
        /// </summary>
        static ManagePlayers()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ManagePlayers), new FrameworkPropertyMetadata(typeof(ManagePlayers)));
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the view model for this view
        /// </summary>
        public ManagePlayersViewModel ViewModel
        {
            get { return (ManagePlayersViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="ViewModel"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ManagePlayersViewModel), typeof(ManagePlayers));
        #endregion
        #region Methods
        public override void OnApplyTemplate()
        {
            CommandBindings.Add(new CommandBinding(Commands.Add, Add_Executed, Add_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Remove, Remove_Executed, Remove_CanExecute));
            CommandBindings.Add(new CommandBinding(Commands.Edit, Edit_Executed, Edit_CanExecute));

            ListBox? listBox = Template.FindName("PART_PlayerCharacterList", this) as ListBox;
            if (listBox != null)
            {
                Style style = listBox.GetOrCreateItemContainerStyle<ListBoxItem>();
                style.Setters.Add(new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler(CharacterList_DoubleClick)));
            }
        }

        private async void CharacterList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            await Exceptions.FailSafeMethodCall(async () =>
            {
                if (sender is ListBoxItem item && item.DataContext is PlayerCharacter pc)
                    await EditCharacter(new EditPlayerCharacterViewModel(ViewModel.Campaign, pc, ViewModel.Factory.Server));
            });
        }

        private void Edit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.Parameter is PlayerCharacter;
        }

        private async void Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await Exceptions.FailSafeMethodCall(async () =>
            {
                e.Handled = true;
                if (e.Parameter is PlayerCharacter pc)
                    await EditCharacter(new EditPlayerCharacterViewModel(ViewModel.Campaign, pc, ViewModel.Factory.Server));
            });
        }

        private void Remove_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.Parameter is PlayerCharacter;
        }

        private void Remove_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is PlayerCharacter pc)
                    ViewModel.Characters.Remove(pc);
            });
        }

        private void Add_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = ViewModel?.Campaign != null;
        }

        private async void Add_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await Exceptions.FailSafeMethodCall(async () =>
            {
                e.Handled = true;
                if (ViewModel?.Campaign != null)
                {
                    EditPlayerCharacterViewModel vm = new EditPlayerCharacterViewModel(ViewModel.Campaign, ViewModel.Factory.Server);
                    await EditCharacter(vm);
                }
            });
        }

        private async Task EditCharacter(EditPlayerCharacterViewModel vm)
        {
            EditWindow window = new EditWindow();
            window.Owner = Window.GetWindow(this);
            window.DataContext = vm;

            if (window.ShowDialog() == true)
                await vm.Save();
        }
        #endregion
    }
}
