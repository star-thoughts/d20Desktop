using Fiction.GameScreen.Combat;
using Fiction.GameScreen.Serialization;
using Fiction.GameScreen.Server;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// Generates and tracks view models
    /// </summary>
    public class ViewModelFactory : IViewModelFactory, INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ViewModelFactory"/>
        /// </summary>
        /// <param name="campaign">Campaign information</param>
        public ViewModelFactory(CampaignSettings campaign)
        {
            Campaign = campaign;
            CombatScenarios = new CombatScenariosViewModel(this, Campaign);
            MonsterManager = new ManageMonstersViewModel(this);
            ManageTypes = new ManageMonsterTypesViewModel(this);
            Spells = new ManageSpellsViewModel(this);
            ManageSpellSchools = new ManageSpellSchoolsViewModel(this);
            ManageSources = new ManageSourcesViewModel(this);
            MagicItems = new MagicItemsViewModel(this);
            Conditions = new ConditionsViewModel(this);

            Players = new ManagePlayersViewModel(this);
            if (campaign.Combat.Active != null)
                ActiveCombat = new ActiveCombatViewModel(this, campaign.Combat.Active);
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the campaign this view model factory is for
        /// </summary>
        public CampaignSettings Campaign { get; private set; }
        /// <summary>
        /// Gets the view model for combat scenarios
        /// </summary>
        public CombatScenariosViewModel CombatScenarios { get; }
        private CombatPrepWatcher? _prepareCombatWatcher;
        private PrepareCombatViewModel? _prepareCombat;
        /// <summary>
        /// Gets the view model used for preparing for combat
        /// </summary>
        public PrepareCombatViewModel BeginCombat
        {
            get
            {
                if (_prepareCombat == null)
                {
                    CreateCombatPrep();
                }
                return _prepareCombat;
            }
        }

        private CombatWatcher? _combatWatcher;
        private ActiveCombatViewModel? _activeCombat;
        /// <summary>
        /// Gets the active combat
        /// </summary>
        /// <remarks>
        /// To set this, call CreateCombat
        /// </remarks>
        public ActiveCombatViewModel? ActiveCombat
        {
            get { return _activeCombat; }
            private set
            {
                if (!ReferenceEquals(_activeCombat, value))
                {
                    _activeCombat = value;
                    Campaign.Combat.Active = _activeCombat?.Combat;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets the manager for monsters in the campaign
        /// </summary>
        public ManageMonstersViewModel MonsterManager { get; }
        /// <summary>
        /// Gets the view model for managing players
        /// </summary>
        public ManagePlayersViewModel Players { get; }
        /// <summary>
        /// Gets the view model for managing monster types and subtypes
        /// </summary>
        public ManageMonsterTypesViewModel ManageTypes { get; }
        /// <summary>
        /// Gets the view model for managing spells in a campaign
        /// </summary>
        public ManageSpellsViewModel Spells { get; }
        /// <summary>
        /// Gets the view model for managing spell schools
        /// </summary>
        public ManageSpellSchoolsViewModel ManageSpellSchools { get; }
        /// <summary>
        /// Gets the view model for managing sources in the campaign
        /// </summary>
        public ManageSourcesViewModel ManageSources { get; }
        /// <summary>
        /// Gets the view model for managing magic items
        /// </summary>
        public MagicItemsViewModel MagicItems { get; }
        /// <summary>
        /// Gets the view model for managing conditions
        /// </summary>
        public ConditionsViewModel Conditions { get; }
        /// <summary>
        /// Gets the server connection for campaign and combat management
        /// </summary>
        public ICampaignManagement? Server { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Sets the server to use for campaign and combat management
        /// </summary>
        /// <param name="campaignServer"></param>
        public void SetServer(ICampaignManagement campaignServer)
        {
            if (campaignServer == null)
                throw new ArgumentNullException(nameof(campaignServer));

            Server = campaignServer;
        }

        [MemberNotNull(nameof(_prepareCombat))]
        private async void CreateCombatPrep()
        {
            if (ActiveCombat == null)
            {
                _prepareCombat = new PrepareCombatViewModel(this);
                if (!string.IsNullOrWhiteSpace(Campaign.CampaignID))
                {
                    ICombatManagement? combatManagement = null;
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(Campaign.ServerUri))
                        {
                            HttpClient client = new HttpClient();
                            client.BaseAddress = new Uri(Campaign.ServerUri);

                            combatManagement = new CombatManagement(client);
                            CombatPrepWatcher watcher = new CombatPrepWatcher(Campaign.CampaignID, _prepareCombat.Preparer, combatManagement);

                            if (await watcher.InitializeAsync())
                                _prepareCombatWatcher = watcher;
                        }
                    }
                    catch
                    {
                        combatManagement?.Dispose();
                    }
                }
            }
            else
                _prepareCombat = new PrepareCombatViewModel(this, ActiveCombat);
        }

        /// <summary>
        /// Creates a new combat
        /// </summary>
        /// <param name="preparer">Preparer containing information about the combat</param>
        /// <returns>View model for the combat created</returns>
        /// <exception cref="InvalidOperationException">The preparer wasn't valid, or there is already a combat running.</exception>
        public async Task<ActiveCombatViewModel> CreateOrUpdateCombat(PrepareCombatViewModel preparer)
        {
            Exceptions.ThrowIfArgumentNull(preparer, nameof(preparer));
            if (!preparer.IsValid)
                throw new InvalidOperationException("Combat wasn't ready.");

            await EndCombatPrep();

            if (ActiveCombat == null)
            {
                await CreateCombat(preparer);
            }
            else
            {
                ActiveCombat.Combat.AddCombatants(preparer.Preparer);
            }
            _prepareCombat = null;

            this.RaisePropertyChanged(nameof(ActiveCombat));
            return ActiveCombat;
        }

        private async Task EndCombatPrep()
        {
            if (_prepareCombatWatcher != null)
                await _prepareCombatWatcher.DisposeAsync();
        }

        [MemberNotNull(nameof(ActiveCombat))]
        private async Task CreateCombat(PrepareCombatViewModel preparer)
        {
            ActiveCombat combat = new ActiveCombat("Active Combat", preparer.Preparer, new XmlActiveCombatSerializer(Campaign));
            ActiveCombat = new ActiveCombatViewModel(this, combat);

            if (!string.IsNullOrWhiteSpace(Campaign.CampaignID))
            {
                ICombatManagement? combatManagement = null;
                try
                {
                    HttpClient? client = GetServerClient();
                    if (client != null)
                    {
                        combatManagement = new CombatManagement(client);
                        CombatWatcher watcher = new CombatWatcher(Campaign.CampaignID, combat, combatManagement);

                        if (await watcher.InitializeAsync())
                            _combatWatcher = watcher;
                    }
                }
                catch
                {
                    combatManagement?.Dispose();
                }
            }
        }

        /// <summary>
        /// Terminates combat
        /// </summary>
        /// <returns>Task for asynchronous completion</returns>
        public async Task EndCombat()
        {
            ActiveCombatViewModel? active = ActiveCombat;
            ActiveCombat = null;
            _prepareCombat = null;

            if (_combatWatcher != null)
            {
                await _combatWatcher.EndCombat();
                await _combatWatcher.DisposeAsync();
            }

            await Task.Yield();
        }

        /// <summary>
        /// Gets the campaign manager for the configured server
        /// </summary>
        /// <returns>Interface for managing campaigns on the server</returns>
        public ICampaignManagement? GetCampaignManager()
        {
            if (Server == null)
            {
                HttpClient? client = GetServerClient();
                if (client != null)
                    Server = new CampaignManagement(client, Campaign.CampaignID);
            }
            return Server;
        }

        /// <summary>
        /// Creates an HTTP Client for communicating with the server
        /// </summary>
        /// <returns>Http client for communicating with the server</returns>
        private HttpClient? GetServerClient()
        {
            if (!string.IsNullOrWhiteSpace(Campaign.ServerUri))
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Campaign.ServerUri);

                return client;
            }
            return null;
        }
        #endregion
        #region Events
#pragma warning disable 67
        /// <summary>
        /// Event that is triggered when a property changes
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 67
        #endregion
    }
}
