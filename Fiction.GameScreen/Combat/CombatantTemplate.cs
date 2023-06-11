using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Contains information for creating combatants
    /// </summary>
    public class CombatantTemplate : ICombatantTemplate, INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CombatantTemplate"/>
        /// </summary>
        public CombatantTemplate(CampaignSettings campaign)
        {
            Exceptions.ThrowIfArgumentNull(campaign, nameof(campaign));

            _name = string.Empty;
            _hitDieString = string.Empty;
            _count = string.Empty;

            Campaign = campaign;
            Id = campaign.GetNextId();
            Initialize();
        }


        [MemberNotNull(nameof(_hitDieRollingStrategy)), MemberNotNull(nameof(DamageReduction))]
        private void Initialize()
        {
            _hitDieRollingStrategy = RollingStrategy.Standard;
            _displayToPlayers = true;
            DamageReduction = new ObservableCollection<DamageReduction>();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the campaign associated with this combatant
        /// </summary>
        public CampaignSettings Campaign { get; internal set; }
        /// <summary>
        /// Gets the ID of this combatant
        /// </summary>
        public int Id { get; internal set; }
        /// <summary>
        /// Gets or sets the ID of this combatant template on the server
        /// </summary>
        public string? ServerID { get; set; }
        private string _name;
        /// <summary>
        /// Gets or sets the name of the combatant
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.Equals(_name, value, StringComparison.CurrentCulture))
                {
                    _name = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _hitDieString;
        /// <summary>
        /// Gets or sets the string used for Hit Dice
        /// </summary>
        public string HitDieString
        {
            get { return _hitDieString; }
            set
            {
                if (!string.Equals(_hitDieString, value, StringComparison.CurrentCulture))
                {
                    _hitDieString = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private RollingStrategy _hitDieRollingStrategy;
        /// <summary>
        /// Gets or sets the rolling strategy to use
        /// </summary>
        public RollingStrategy HitDieRollingStrategy
        {
            get { return _hitDieRollingStrategy; }
            set
            {
                if (_hitDieRollingStrategy != value)
                {
                    _hitDieRollingStrategy = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private int _initiativeModifier;
        /// <summary>
        /// Gets or sets the default initiative modifier for this combatant
        /// </summary>
        public int InitiativeModifier
        {
            get { return _initiativeModifier; }
            set
            {
                if (_initiativeModifier != value)
                {
                    _initiativeModifier = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _count;
        /// <summary>
        /// Gets or sets the number of combatants to add
        /// </summary>
        public string Count
        {
            get { return _count; }
            set
            {
                if (!string.Equals(_count, value, StringComparison.CurrentCulture))
                {
                    _count = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private bool _displayToPlayers;
        /// <summary>
        /// Gets or sets whether or not to display this combatant to the players
        /// </summary>
        public bool DisplayToPlayers
        {
            get { return _displayToPlayers; }
            set
            {
                if (_displayToPlayers != value)
                {
                    _displayToPlayers = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _displayName;
        /// <summary>
        /// Gets or sets the name to display to the players during combat
        /// </summary>
        public string? DisplayName
        {
            get { return _displayName; }
            set
            {
                if (!string.Equals(_displayName, value, StringComparison.CurrentCulture))
                {
                    _displayName = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private int _fastHealing;
        /// <summary>
        /// Gets or sets the amount of fast healing the combatant will have
        /// </summary>
        public int FastHealing
        {
            get { return _fastHealing; }
            set
            {
                if (_fastHealing != value)
                {
                    _fastHealing = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private int _deadAt;
        /// <summary>
        /// Gets or sets the amount of hit points the combatant is dead at
        /// </summary>
        public int DeadAt
        {
            get { return _deadAt; }
            set
            {
                if (_deadAt != value)
                {
                    _deadAt = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private int _unconsciousAt;
        /// <summary>
        /// Gets or sets the amount of hit points the combatant is unconscious at
        /// </summary>
        public int UnconsciousAt
        {
            get { return _unconsciousAt; }
            set
            {
                if (_unconsciousAt != value)
                {
                    _unconsciousAt = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private ICombatantTemplateSource? _source;
        /// <summary>
        /// Gets or sets the source for this combatant template, or null if no source.
        /// </summary>
        public ICombatantTemplateSource? Source
        {
            get { return _source; }
            set
            {
                if (!ReferenceEquals(_source, value))
                {
                    _source = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets whether or not this information is valid
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Name)
                  && !string.IsNullOrWhiteSpace(HitDieString)
                  && !string.IsNullOrWhiteSpace(Count);
            }
        }
        /// <summary>
        /// Gets a collection of damage reductions
        /// </summary>
        public ObservableCollection<DamageReduction> DamageReduction { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Creates a combatant using preparation information
        /// </summary>
        /// <param name="preparer">Preparation information for the combatant</param>
        /// <returns>Combatant for an active combat</returns>
        public ICombatant CreateCombatant(CombatantPreparer preparer)
        {
            Exceptions.ThrowIfArgumentNull(preparer, nameof(preparer));

            return new Combatant(Campaign, preparer);
        }
        /// <summary>
        /// Gets combatant preparers for this combatant
        /// </summary>
        /// <param name="preparer">Preparation information for the combat</param>
        /// <returns>Preparation information for this combatant</returns>
        public CombatantPreparer[] Prepare(CombatPreparer preparer)
        {
            Exceptions.ThrowIfArgumentNull(preparer, nameof(preparer));
            if (!IsValid)
                throw new InvalidOperationException("Cannot prepare a combatant with this template.");

            int count = Dice.Roll(Count);
            return Enumerable.Range(0, count)
                .Select(p => new CombatantPreparer(this))
                .ToArray();
        }

        public d20Web.Models.Combat.CombatantTemplate ToServerTemplate()
        {
            return new d20Web.Models.Combat.CombatantTemplate()
            {
                Count = Count,
                DamageReduction = DamageReduction.ToServerDamageReduction().ToArray(),
                DeadAt = DeadAt,
                DisplayName = DisplayName,
                DisplayToPlayers = DisplayToPlayers,
                HitDieRollingStrategy = (d20Web.Models.RollingStrategy)HitDieRollingStrategy,
                HitDieString = HitDieString,
                ID = ServerID,
                SourceID = Source?.ServerID,
                FastHealing = FastHealing,
                InitiativeModifier = InitiativeModifier,
                Name = Name,
                UnconsciousAt = UnconsciousAt,
            };
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
