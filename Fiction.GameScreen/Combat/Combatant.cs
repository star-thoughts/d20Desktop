using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Standard combatant for an active combat
    /// </summary>
    public class Combatant : ICombatant, INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="Combatant"/>
        /// </summary>
        /// <param name="campaign">Campaign this combatant is in</param>
        /// <param name="preparedInfo">Preparation info for the combat</param>
        public Combatant(CampaignSettings campaign, CombatantPreparer preparedInfo)
        {
            Exceptions.ThrowIfArgumentNull(campaign, nameof(campaign));
            Exceptions.ThrowIfArgumentNull(preparedInfo, nameof(preparedInfo));

            Initialize(campaign, preparedInfo);
        }

        [MemberNotNull(nameof(Campaign)), MemberNotNull(nameof(_name)), MemberNotNull(nameof(PreparedInfo)),
            MemberNotNull(nameof(Health)), MemberNotNull(nameof(Conditions)), MemberNotNull(nameof(DamageReduction))]
        private void Initialize(CampaignSettings campaign, CombatantPreparer preparedInfo)
        {
            Campaign = campaign;
            PreparedInfo = preparedInfo;
            _name = preparedInfo.Name;
            Ordinal = preparedInfo.Ordinal;
            InitiativeOrder = preparedInfo.InitiativeOrder;
            DisplayToPlayers = preparedInfo.DisplayToPlayers;
            DisplayName = preparedInfo.DisplayName;
            IncludeInCombat = true;
            IsPlayer = preparedInfo.IsPlayer;

            Health = new CombatantHealth();
            Health.MaxHealth = preparedInfo.HitPoints;
            Health.FastHealing = preparedInfo.FastHealing;
            Health.DeadAt = preparedInfo.DeadAt;
            Health.UnconsciousAt = preparedInfo.UnconsciousAt;

            Id = campaign.GetNextId();

            Conditions = new ObservableCollection<AppliedCondition>();
            DamageReduction = preparedInfo.DamageReduction?.Copy()
                ?? new ObservableCollection<DamageReduction>();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the ID used by this combatant on a combat server
        /// </summary>
        public string? ServerID { get; set; }
        /// <summary>
        /// Gets the campaign this combatant is part of
        /// </summary>
        public CampaignSettings Campaign { get; internal set; }
        /// <summary>
        /// Gets the combat this combatant is part of
        /// </summary>
        public ActiveCombat? Combat { get { return Campaign.Combat.Active; } }
        /// <summary>
        /// Gets the ID of the combatant
        /// </summary>
        public int Id { get; internal set; }
        private string _name;
        /// <summary>
        /// Gets or sets the name of this combatant
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
        /// <summary>
        /// Gets the ordinal of this combatant
        /// </summary>
        public int Ordinal { get; internal set; }
        /// <summary>
        /// Gets the source of this combatant
        /// </summary>
        public ICombatantTemplate? Source { get { return PreparedInfo.Source; } }
        /// <summary>
        /// Gets the info used to prepare this combatant for combat
        /// </summary>
        public CombatantPreparer PreparedInfo { get; private set; }
        /// <summary>
        /// Gets health information for the combatant
        /// </summary>
        public CombatantHealth Health { get; private set; }
        private int _initiativeOrder;
        /// <summary>
        /// Gets or sets the order in initiative for this combatant
        /// </summary>
        public int InitiativeOrder
        {
            get { return _initiativeOrder; }
            set
            {
                if (_initiativeOrder != value)
                {
                    _initiativeOrder = value;
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
                    this.RaisePropertiesChanged(nameof(DisplayToPlayers), nameof(CanPlayersSee));

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
        private bool _isCurrent;
        /// <summary>
        /// Gets whether or not this is the current combatant
        /// </summary>
        public bool IsCurrent
        {
            get { return _isCurrent; }
            set
            {
                if (_isCurrent != value)
                {
                    _isCurrent = value;
                    HasGoneOnce = true;
                    this.RaisePropertyChanged();
                }
            }
        }
        private bool _hasGoneOnce;
        /// <summary>
        /// Gets or sets whether this combatant has gone at least once in combat
        /// </summary>
        public bool HasGoneOnce
        {
            get { return _hasGoneOnce; }
            set
            {
                if (_hasGoneOnce != value)
                {
                    _hasGoneOnce = value;
                    this.RaisePropertiesChanged(nameof(HasGoneOnce), nameof(CanPlayersSee));
                }
            }
        }
        /// <summary>
        /// Gets whether or not this combatant is a player
        /// </summary>
        public bool IsPlayer { get; private set; }
        private bool _includeInCombat;
        /// <summary>
        /// Gets or sets wehther this combatant should be included in combat
        /// </summary>
        public bool IncludeInCombat
        {
            get { return _includeInCombat; }
            set
            {
                if (_includeInCombat != value)
                {
                    _includeInCombat = value;
                    this.RaisePropertiesChanged(nameof(IncludeInCombat), nameof(CanPlayersSee));
                }
            }
        }
        /// <summary>
        /// Gets whether or not the players can see this combatant
        /// </summary>
        /// <remarks>
        /// The players should be able to see the character if:
        /// <list type="bullet">
        /// <item>It is a player and <see cref="IncludeInCombat"/> is true, or</item>
        /// <item><see cref="IncludeInCombat"/> is true, they are to be displayed to players and they've gone at least once</item>
        /// </list>
        /// </remarks>
        public bool CanPlayersSee { get { return (IsPlayer && IncludeInCombat) || (IncludeInCombat && DisplayToPlayers && HasGoneOnce); } }
        /// <summary>
        /// Gets a collection of conditions applied to this combatant
        /// </summary>
        public ObservableCollection<AppliedCondition> Conditions { get; private set; }
        /// <summary>
        /// Gets a collection of items describing damage reduction
        /// </summary>
        public ObservableCollection<DamageReduction> DamageReduction { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Handles all beginning of turn events and determines whether the combatant can take a turn
        /// </summary>
        /// <param name="settings">Settings for the combat</param>
        /// <returns>Whether or not the combatant can take a turn</returns>
        public bool TryBeginTurn(CombatSettings settings)
        {
            if (Health.FastHealing != 0 && !Health.IsDead)
                Health.ApplyHealing(Health.FastHealing, false);

            return _includeInCombat && (!settings.SkipDownedCombatants || !Health.IsDown);
        }
        #endregion
#pragma warning disable 67
        /// <summary>
        /// Event that is triggered when a property changes
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 67
    }
}
