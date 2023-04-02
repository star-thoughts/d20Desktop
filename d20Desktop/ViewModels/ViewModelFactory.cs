using Fiction.GameScreen.Combat;
using Fiction.GameScreen.Serialization;
using Fiction.GameScreen.Server;
using System;
using System.ComponentModel;
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
        #region Member Variables
        private PrepareCombatViewModel? _prepareCombat;
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
        /// <summary>
        /// Gets the view model used for preparing for combat
        /// </summary>
        public PrepareCombatViewModel BeginCombat
        {
            get
            {
                if (_prepareCombat == null)
                {
                    if (ActiveCombat != null)
                        _prepareCombat = new PrepareCombatViewModel(this, ActiveCombat);
                    else
                        _prepareCombat = new PrepareCombatViewModel(this);
                }
                return _prepareCombat;
            }
        }
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
        public CampaignManagement Server { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Sets the server to use for campaign and combat management
        /// </summary>
        /// <param name="campaignServer"></param>
        public void SetServer(CampaignManagement campaignServer)
        {
            if (campaignServer == null)
                throw new ArgumentNullException(nameof(campaignServer));

            Server = campaignServer;
        }
        /// <summary>
        /// Creates a new combat
        /// </summary>
        /// <param name="preparer">Preparer containing information about the combat</param>
        /// <returns>View model for the combat created</returns>
        /// <exception cref="InvalidOperationException">The preparer wasn't valid, or there is already a combat running.</exception>
        public ActiveCombatViewModel CreateOrUpdateCombat(PrepareCombatViewModel preparer)
        {
            Exceptions.ThrowIfArgumentNull(preparer, nameof(preparer));
            if (!preparer.IsValid)
                throw new InvalidOperationException("Combat wasn't ready.");

            if (ActiveCombat == null)
            {
                ActiveCombat combat = new ActiveCombat("Active Combat", preparer.Preparer, new XmlActiveCombatSerializer(Campaign));
                ActiveCombat = new ActiveCombatViewModel(this, combat);
            }
            else
            {
                ActiveCombat.Combat.AddCombatants(preparer.Preparer);
            }
            _prepareCombat = null;

            this.RaisePropertyChanged(nameof(ActiveCombat));
            return ActiveCombat;
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

            await Task.Yield();
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
