using Fiction.GameScreen.Combat;
using Fiction.GameScreen.Serialization;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
            if (campaign.Combat.Active != null)
                ActiveCombat = new ActiveCombatViewModel(this, campaign.Combat.Active);
        }
        #endregion
        #region Member Variables
        private CombatScenariosViewModel _scenarios;
        private PrepareCombatViewModel _prepareCombat;
        #endregion
        #region Properties
        /// <summary>
        /// Gets the campaign this view model factory is for
        /// </summary>
        public CampaignSettings Campaign { get; private set; }
        /// <summary>
        /// Gets the view model for combat scenarios
        /// </summary>
        public CombatScenariosViewModel CombatScenarios
        {
            get
            {
                if (_scenarios == null)
                    _scenarios = new CombatScenariosViewModel(this, Campaign);
                return _scenarios;
            }
        }
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
        private ActiveCombatViewModel _activeCombat;
        /// <summary>
        /// Gets the active combat
        /// </summary>
        /// <remarks>
        /// To set this, call CreateCombat
        /// </remarks>
        public ActiveCombatViewModel ActiveCombat
        {
            get { return _activeCombat; }
            set
            {
                if (!ReferenceEquals(_activeCombat, value))
                {
                    _activeCombat = value;
                    Campaign.Combat.Active = _activeCombat?.Combat;
                    this.RaisePropertyChanged();
                }
            }
        }
        private ManageMonstersViewModel _monsterManager;
        /// <summary>
        /// Gets the manager for monsters in the campaign
        /// </summary>
        public ManageMonstersViewModel MonsterManager
        {
            get
            {
                if (_monsterManager == null)
                    _monsterManager = new ManageMonstersViewModel(this);
                return _monsterManager;
            }
        }
        private ManagePlayersViewModel _players;
        /// <summary>
        /// Gets the view model for managing players
        /// </summary>
        public ManagePlayersViewModel Players
        {
            get
            {
                if (_players == null)
                    _players = new ManagePlayersViewModel(this);
                return _players;
            }
        }
        private ManageMonsterTypesViewModel _manageTypes;
        /// <summary>
        /// Gets the view model for managing monster types and subtypes
        /// </summary>
        public ManageMonsterTypesViewModel ManageTypes
        {
            get
            {
                if (_manageTypes == null)
                    _manageTypes = new ManageMonsterTypesViewModel(this);
                return _manageTypes;
            }
        }
        private ManageSpellsViewModel _manageSpells;
        /// <summary>
        /// Gets the view model for managing spells in a campaign
        /// </summary>
        public ManageSpellsViewModel Spells
        {
            get
            {
                if (_manageSpells == null)
                    _manageSpells = new ManageSpellsViewModel(this);
                return _manageSpells;
            }
        }
        private ManageSpellSchoolsViewModel _manageSchools;
        /// <summary>
        /// Gets the view model for managing spell schools
        /// </summary>
        public ManageSpellSchoolsViewModel ManageSpellSchools
        {
            get
            {
                if (_manageSchools == null)
                    _manageSchools = new ManageSpellSchoolsViewModel(this);
                return _manageSchools;
            }
        }
        private ManageSourcesViewModel _manageSources;
        /// <summary>
        /// Gets the view model for managing sources in the campaign
        /// </summary>
        public ManageSourcesViewModel ManageSources
        {
            get
            {
                if (_manageSources == null)
                    _manageSources = new ManageSourcesViewModel(this);
                return _manageSources;
            }
        }
        private MagicItemsViewModel _magicItems;
        /// <summary>
        /// Gets the view model for managing magic items
        /// </summary>
        public MagicItemsViewModel MagicItems
        {
            get
            {
                if (_magicItems == null)
                    _magicItems = new MagicItemsViewModel(this);
                return _magicItems;
            }
        }
        private ConditionsViewModel _conditions;
        /// <summary>
        /// Gets the view model for managing conditions
        /// </summary>
        public ConditionsViewModel Conditions
        {
            get
            {
                if (_conditions == null)
                    _conditions = new ConditionsViewModel(this);
                return _conditions;
            }
        }
        #endregion
        #region Methods
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
            ActiveCombatViewModel active = ActiveCombat;
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
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67
        #endregion
    }
}
