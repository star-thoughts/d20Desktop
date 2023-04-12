using Fiction.GameScreen.Serialization;
using Fiction.GameScreen.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml;

namespace Fiction.GameScreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="MainWindow"/>
        /// </summary>
        public MainWindow()
        {
            ViewModels = new ObservableCollection<CampaignViewModelCore>();

            IsIdle = true;

            InitializeComponent();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the current campaign's view model factory
        /// </summary>
        public ViewModelFactory? Campaign
        {
            get { return (ViewModelFactory)GetValue(CampaignProperty); }
            private set { SetValue(CampaignProperty, value); }
        }
        /// <summary>
        /// Gets or sets the view models currently being displayed
        /// </summary>
        public ObservableCollection<CampaignViewModelCore> ViewModels
        {
            get { return (ObservableCollection<CampaignViewModelCore>)GetValue(ViewModelsProperty); }
            private set { SetValue(ViewModelsProperty, value); }
        }
        /// <summary>
        /// Gets or sets the currently selected page
        /// </summary>
        public CampaignViewModelCore? SelectedPage
        {
            get { return (CampaignViewModelCore)GetValue(SelectedPageProperty); }
            set { SetValue(SelectedPageProperty, value); }
        }
        /// <summary>
        /// Gets or sets whether or not the application is idle
        /// </summary>
        public bool IsIdle
        {
            get { return (bool)GetValue(IsIdleProperty); }
            set { SetValue(IsIdleProperty, value); }
        }
        /// <summary>
        /// Gets or sets the last file name used for the campaign
        /// </summary>
        public string? FileName { get; private set; }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for <see cref="Campaign"/>
        /// </summary>
        public static readonly DependencyProperty CampaignProperty = DependencyProperty.RegisterAttached(nameof(Campaign), typeof(ViewModelFactory), typeof(MainWindow));
        /// <summary>
        /// DependencyProperty for <see cref="ViewModels"/>
        /// </summary>
        public static readonly DependencyProperty ViewModelsProperty = DependencyProperty.Register(nameof(ViewModels), typeof(ObservableCollection<CampaignViewModelCore>), typeof(MainWindow));
        /// <summary>
        /// DependencyProperty for <see cref="SelectedPage"/>
        /// </summary>
        public static readonly DependencyProperty SelectedPageProperty = DependencyProperty.Register(nameof(SelectedPage), typeof(CampaignViewModelCore), typeof(MainWindow));
        /// <summary>
        /// DependencyProperty for <see cref="IsIdle"/>
        /// </summary>
        public static readonly DependencyProperty IsIdleProperty = DependencyProperty.Register(nameof(IsIdle), typeof(bool), typeof(MainWindow));

        private static void CampaignChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MainWindow)?.CampaignChanged((IViewModelFactory)e.NewValue, (IViewModelFactory)e.OldValue);
        }
        #endregion
        #region Methods
        private void AddPageIfNotOpen(CampaignViewModelCore page)
        {
            if (!ViewModels.Contains(page))
                ViewModels.Add(page);

            SelectedPage = page;
        }

        private void CampaignChanged(IViewModelFactory newValue, IViewModelFactory oldValue)
        {
            ViewModels.Clear();
            if (newValue != null)
            {
                AddPageIfNotOpen(newValue.Players);

                CampaignSettings campaign = newValue.Campaign;
                BindingOperations.EnableCollectionSynchronization(campaign.Combat.Scenarios, new object());
                BindingOperations.EnableCollectionSynchronization(campaign.EquipmentManager.MagicItems, new object());
                BindingOperations.EnableCollectionSynchronization(campaign.MonsterManager.Monsters, new object());
                BindingOperations.EnableCollectionSynchronization(campaign.MonsterManager.Types, new object());
                BindingOperations.EnableCollectionSynchronization(campaign.MonsterManager.Groups, new object());
                BindingOperations.EnableCollectionSynchronization(campaign.MonsterManager.SubTypes, new object());
                BindingOperations.EnableCollectionSynchronization(campaign.Players.PlayerCharacters, new object());
                BindingOperations.EnableCollectionSynchronization(campaign.Sources, new object());
                BindingOperations.EnableCollectionSynchronization(campaign.Spells.Spells, new object());
                BindingOperations.EnableCollectionSynchronization(campaign.Spells.Schools, new object());
                BindingOperations.EnableCollectionSynchronization(campaign.Spells.SpellEffectTypes, new object());
                BindingOperations.EnableCollectionSynchronization(campaign.Spells.SubSchools, new object());
            }
            else
            {
                CampaignSettings campaign = oldValue.Campaign;
                BindingOperations.DisableCollectionSynchronization(campaign.Combat.Scenarios);
                BindingOperations.DisableCollectionSynchronization(campaign.EquipmentManager.MagicItems);
                BindingOperations.DisableCollectionSynchronization(campaign.MonsterManager.Monsters);
                BindingOperations.DisableCollectionSynchronization(campaign.MonsterManager.Types);
                BindingOperations.DisableCollectionSynchronization(campaign.MonsterManager.Groups);
                BindingOperations.DisableCollectionSynchronization(campaign.MonsterManager.SubTypes);
                BindingOperations.DisableCollectionSynchronization(campaign.Players.PlayerCharacters);
                BindingOperations.DisableCollectionSynchronization(campaign.Sources);
                BindingOperations.DisableCollectionSynchronization(campaign.Spells.Spells);
                BindingOperations.DisableCollectionSynchronization(campaign.Spells.Schools);
                BindingOperations.DisableCollectionSynchronization(campaign.Spells.SpellEffectTypes);
                BindingOperations.DisableCollectionSynchronization(campaign.Spells.SubSchools);
            }
        }

        private void ManageCombatScenariosCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Campaign != null;
        }

        private void ManageCombatScenariosCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (Campaign != null)
                    AddPageIfNotOpen(Campaign.CombatScenarios);
            });
        }

        private void BeginCombatCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Campaign != null
                && (e.Parameter == null || (e.Parameter is PrepareCombatViewModel preparer && preparer.IsValid));
        }

        private async void BeginCombatCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await Exceptions.FailSafeMethodCall(async () =>
            {
                if (Campaign != null)
                {
                    //  null parameter means we want to view the combat preparer
                    if (e.Parameter == null)
                    {
                        e.Handled = true;
                        AddPageIfNotOpen(Campaign.BeginCombat);
                    }
                    //  Otherwise a valid preparer means start combat, or add it to combat
                    else if (e.Parameter is PrepareCombatViewModel preparer && preparer.IsValid)
                    {
                        e.Handled = true;
                        if (ResolveInitiatives(preparer))
                        {
                            ActiveCombatViewModel combat = await Campaign.CreateOrUpdateCombat(preparer);

                            AddPageIfNotOpen(combat);
                            ViewModels.Remove(preparer);
                        }
                    }
                }
            });
        }

        private bool ResolveInitiatives(PrepareCombatViewModel preparer)
        {
            EditWindow window = new EditWindow();
            window.Owner = this;
            window.DataContext = new ResolveInitiativesViewModel(preparer);

            return window.ShowDialog() == true;
        }

        private void CloseTabCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.Parameter is CampaignViewModelCore;
        }

        private void CloseTabCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (e.Parameter is CampaignViewModelCore vm)
                {
                    CloseTab(vm);
                }
            });
        }

        private void CloseTab(CampaignViewModelCore vm)
        {
            CampaignViewModelCore? next = ViewModels.AfterOrFirstOrDefault(vm);
            ViewModels.Remove(vm);
            if (!ReferenceEquals(vm, next))
                SelectedPage = next;
        }

        private void CloseTabs<T>() where T : CampaignViewModelCore
        {
            T[] tabs = ViewModels.OfType<T>().ToArray();
            foreach (T tab in tabs)
                CloseTab(tab);
        }

        private void EndCombat_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = e.Parameter is ActiveCombatViewModel;
        }

        private async void EndCombat_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await Exceptions.FailSafeMethodCall(async () =>
               {
                   e.Handled = true;
                   if (e.Parameter is ActiveCombatViewModel combat && Campaign != null)
                   {
                       if (MessageBox.Show(this, GameScreen.Resources.Resources.EndCombatWarningMessage, Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                       {
                           CloseTab(combat);
                           await Campaign.EndCombat();
                       }
                   }
               });
        }

        private void ActiveCombat_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Campaign?.ActiveCombat != null;
        }

        private void ActiveCombat_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (Campaign?.ActiveCombat != null)
                    AddPageIfNotOpen(Campaign.ActiveCombat);
            });
        }

        private void ManageMonstersCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Campaign?.MonsterManager != null;
        }

        private void ManageMonstersCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (Campaign?.MonsterManager != null)
                    AddPageIfNotOpen(Campaign.MonsterManager);
            });
        }

        private void ManagePlayersCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Campaign != null;
        }

        private void ManagePlayersCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (Campaign != null)
                    AddPageIfNotOpen(Campaign.Players);
            });
        }

        private void NewCampaign_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = true;
        }

        private async void NewCampaign_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await Exceptions.FailSafeMethodCall(async () =>
             {
                 e.Handled = true;
                 try
                 {
                     if (await EnsureCampaignIsSaved())
                     {
                         Campaign = new ViewModelFactory(new CampaignSettings());
                         FileName = string.Empty;
                     }
                 }
                 catch (Exception exc)
                 {
                     MessageBox.Show(exc.Message);
                 }
             });
        }

        private async Task<bool> EnsureCampaignIsSaved()
        {
            if (Campaign != null)
            {
                if (MessageBox.Show(this, GameScreen.Resources.Resources.SaveCampaignFirstMessage, Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    return await SaveCampaign();
                }
            }
            return true;
        }

        private async Task<bool> SaveCampaign()
        {
            IsIdle = false;
            try
            {
                if (string.IsNullOrEmpty(FileName))
                    FileName = AskUserForCampaignSaveFile();
                if (!string.IsNullOrWhiteSpace(FileName) && Campaign != null)
                {
                    XmlCampaignSerializer serializer = new XmlCampaignSerializer(ReaderWriterGenerator.DefaultAsync);
                    using (Stream stream = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, true))
                        await serializer.WriteCampaign(stream, Campaign.Campaign);
                }
            }
            finally
            {
                IsIdle = true;
            }
            return !string.IsNullOrEmpty(FileName);
        }

        private string AskUserForCampaignSaveFile()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = ".d20";
            dialog.Filter = "Campaign (*.d20)|*.d20|All Files (*.*)|*.*";
            dialog.OverwritePrompt = true;

            if (dialog.ShowDialog() == true)
                return dialog.FileName;

            return string.Empty;
        }

        private void OpenCampaign_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = true;
        }

        private async void OpenCampaign_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await Exceptions.FailSafeMethodCall(async () =>
             {
                 e.Handled = true;
                 try
                 {
                     IsEnabled = false;
                     if (await EnsureCampaignIsSaved())
                     {

                         OpenFileDialog dialog = new OpenFileDialog();
                         dialog.Title = GameScreen.Resources.Resources.OpenCampaignDialogTitle;
                         dialog.AddExtension = true;
                         dialog.CheckFileExists = true;
                         dialog.CheckPathExists = true;
                         dialog.DefaultExt = ".d20";
                         dialog.Filter = "Campaign (*.d20)|*.d20|All Files (*.*)|*.*";
                         dialog.Multiselect = false;

                         if (dialog.ShowDialog() == true)
                         {
                             using (Stream stream = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.None))
                                 Campaign = new ViewModelFactory(await new XmlCampaignSerializer(ReaderWriterGenerator.Default).ReadCampaignSettings(stream));
                             FileName = dialog.FileName;
                         }
                     }
                 }
                 catch (Exception exc)
                 {
                     MessageBox.Show(exc.Message);
                 }
                 finally
                 {
                     IsEnabled = true;
                 }
             });
        }

        private void SaveCampaign_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Campaign != null;
        }

        private async void SaveCampaign_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await Exceptions.FailSafeMethodCall(async () =>
            {
                e.Handled = true;
                IsIdle = false;
                try
                {
                    await SaveCampaign();
                }
                finally
                {
                    IsIdle = true;
                }
            });
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Campaign != null)
            {
                MessageBoxResult result = MessageBox.Show(this, GameScreen.Resources.Resources.SaveCampaignFirstMessage, Title, MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    IsIdle = false;
                    e.Cancel = true;
                    try
                    {
                        if (await SaveCampaign())
                        {
                            FileName = string.Empty;
                            Campaign = null;
                            await Dispatcher.InvokeAsync(() => Close(), System.Windows.Threading.DispatcherPriority.ContextIdle);
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                    finally
                    {
                        IsIdle = true;
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void ManageTypes_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Campaign != null;
        }

        private void ManageTypes_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (Campaign != null)
                    AddPageIfNotOpen(Campaign.ManageTypes);
            });
        }

        private void ManageSpells_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Campaign != null;
        }

        private void ManageSpells_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;
                if (Campaign != null)
                    AddPageIfNotOpen(Campaign.Spells);
            });
        }


        private void ManageSpellSchools_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Campaign != null && IsIdle;
        }

        private void ManageSpellSchools_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;

                if (Campaign != null)
                    AddPageIfNotOpen(Campaign.ManageSpellSchools);
            });
        }

        private void ManageSources_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Campaign != null && IsIdle;
        }

        private void ManageSources_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;

                if (Campaign?.ManageSources != null)
                    AddPageIfNotOpen(Campaign.ManageSources);
            });
        }

        private void MagicItems_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Campaign != null && IsIdle;
        }

        private void MagicItems_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;

                if (Campaign != null)
                    AddPageIfNotOpen(Campaign.MagicItems);
            });
        }

        private void ManageConditionsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Campaign != null && IsIdle;
        }

        private void ManageConditionsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exceptions.FailSafeMethodCall(() =>
            {
                e.Handled = true;

                if (Campaign != null)
                    AddPageIfNotOpen(Campaign.Conditions);
            });
        }

        private void ConnectToServerCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = Campaign != null;
        }

        private async void ConnectToServerCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (Campaign != null)
            {
                ServerConfigViewModel vm = new ServerConfigViewModel(Campaign, Path.GetFileNameWithoutExtension(FileName) ?? "Campaign");

                EditWindow window = new EditWindow();
                window.Owner = this;
                window.DataContext = vm;

                if (window.ShowDialog() == true)
                    await vm.Save();
            }
        }
        #endregion
    }
}
