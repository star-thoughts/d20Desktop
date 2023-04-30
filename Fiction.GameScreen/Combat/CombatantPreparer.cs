using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Contains information for preparing a combatant for combat
    /// </summary>
    public class CombatantPreparer : IActiveCombatant, INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CombatantPreparer"/>
        /// </summary>
        /// <param name="source">Source of the combatant</param>
        public CombatantPreparer(ICombatantTemplate? source)
        {
            Source = source;

            if (source != null)
            {
                _name = source.Name;
                InitiativeModifier = source.InitiativeModifier;
                DisplayToPlayers = source.DisplayToPlayers;
                DisplayName = source.DisplayName;
                HitDiceStrategy = source.HitDieRollingStrategy;
                _hitDieString = source.HitDieString;
                FastHealing = source.FastHealing;
                IsPlayer = source.Source?.IsPlayer ?? false;
                DeadAt = source.DeadAt;
                UnconsciousAt = source.UnconsciousAt;

                DamageReduction = source.DamageReduction?.Copy()
                    ?? new ObservableCollection<DamageReduction>();
            }
            else
            {
                _name = string.Empty;
                _hitDieString = string.Empty;
                DamageReduction = new ObservableCollection<DamageReduction>();
            }
        }
        #endregion
        #region Member Variables
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the ID of the combatant on the combat server
        /// </summary>
        public string? ServerID { get; set; }
        /// <summary>
        /// Gets the campaign this combatant is part of
        /// </summary>
        CampaignSettings? ICampaignObject.Campaign { get; }
        /// <summary>
        /// Gets the id of this combatant
        /// </summary>
        public int Id { get { return Source?.Id ?? -1; } }
        /// <summary>
        /// Gets the template associated with this combatant
        /// </summary>
        public ICombatantTemplate? Source { get; private set; }
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
        private int _ordinal;
        /// <summary>
        /// Gets the ordinal for this combatant
        /// </summary>
        public int Ordinal
        {
            get { return _ordinal; }
            set
            {
                if (_ordinal != value)
                {
                    _ordinal = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private int _initiativeRoll;
        /// <summary>
        /// Gets or sets the actual initiative roll
        /// </summary>
        public int InitiativeRoll
        {
            get { return _initiativeRoll; }
            set
            {
                if (_initiativeRoll != value)
                {
                    _initiativeRoll = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(InitiativeTotal));
                }
            }
        }
        private int _initiativeModifier;
        /// <summary>
        /// Gets or sets the initiative modifier
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
                    this.RaisePropertyChanged(nameof(InitiativeTotal));
                }
            }
        }
        /// <summary>
        /// Gets or sets the total initiative roll after modifier
        /// </summary>
        public int InitiativeTotal
        {
            get { return InitiativeRoll + InitiativeModifier; }
            set
            {
                InitiativeRoll = value - InitiativeModifier;
            }
        }
        private int _hitPoints;
        /// <summary>
        /// Gets or sets the hit points on this combatant
        /// </summary>
        public int HitPoints
        {
            get { return _hitPoints; }
            set
            {
                if (_hitPoints != value)
                {
                    _hitPoints = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _hitDieString;
        /// <summary>
        /// Gets or sets the hit die for this combatant
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
        private RollingStrategy _hitDiceStrategy;
        /// <summary>
        /// Gets or sets the rolling strategy to use
        /// </summary>
        public RollingStrategy HitDiceStrategy
        {
            get { return _hitDiceStrategy; }
            set
            {
                if (_hitDiceStrategy != value)
                {
                    _hitDiceStrategy = value;
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
        private int _initiativeOrder;
        /// <summary>
        /// Gets or sets the order of initiative this combatant falls in
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
        private int _initGroup;
        /// <summary>
        /// Gets or sets the initiative group for this combatant
        /// </summary>
        /// <remarks>
        /// Any combatants in the same group are considered to have tied for initiative
        /// </remarks>
        public int InitiativeGroup
        {
            get { return _initGroup; }
            set
            {
                if (_initGroup != value)
                {
                    _initGroup = value;
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
        /// Gets or sets the amount of fast healing for this combatant
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
        /// <summary>
        /// Gets whether or not this is a player character
        /// </summary>
        public bool IsPlayer { get; private set; }
        /// <summary>
        /// Gets a collection of items describing damage reduction
        /// </summary>
        public ObservableCollection<DamageReduction> DamageReduction { get; }
        #endregion
        #region Methods
        /// <summary>
        /// Rolls initiative
        /// </summary>
        public void RollInitiative()
        {
            InitiativeRoll = Dice.Roll(1, 20);
        }
        /// <summary>
        /// Rolls hit points
        /// </summary>
        public void RollHitPoints()
        {
            if (string.IsNullOrEmpty(HitDieString))
                throw new InvalidOperationException("Cannot roll on this combatant because no hit dice was set.");

            HitPoints = Dice.Roll(HitDieString, HitDiceStrategy);
        }
        /// <summary>
        /// Creates a combatant for an active combat
        /// </summary>
        /// <returns>Combatant created</returns>
        public virtual ICombatant? CreateCombatant()
        {
            return Source?.CreateCombatant(this);
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
