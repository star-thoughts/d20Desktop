using Fiction.GameScreen.Combat;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for editing a combatant in a combat scenario
    /// </summary>
    public sealed class CombatantTemplateEditViewModel : INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CombatantTemplateEditViewModel"/>
        /// </summary>
        /// <param name="combatant">Combatant to edit</param>
        public CombatantTemplateEditViewModel(ICombatantTemplate combatant)
        {
            _source = combatant.Source;
            Combatant = combatant;
            _name = combatant.Name;
            _hitDieString = combatant.HitDieString;
            _hitDieRollingStrategy = combatant.HitDieRollingStrategy;
            InitiativeModifier = combatant.InitiativeModifier;
            _count = combatant.Count;
            DisplayToPlayers = combatant.DisplayToPlayers;
            _displayName = combatant.DisplayName;
            FastHealing = combatant.FastHealing;
            DeadAt = combatant.DeadAt;
            UnconsciousAt = combatant.UnconsciousAt;
            _damageReduction = combatant.DamageReduction.Copy();

            DamageReduction.CollectionChanged += (s, e) => this.RaisePropertyChanged(nameof(DamageReduction));
        }
        #endregion
        #region Member Variables
        #endregion
        #region Properties
        /// <summary>
        /// Gets the combatant being edited
        /// </summary>
        public ICombatantTemplate Combatant { get; private set; }
        private string _name;
        /// <summary>
        /// Gets or sets the name of this combatant
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.Equals(_name, value))
                {
                    _name = value;
                    this.RaisePropertiesChanged(nameof(Name), nameof(IsValid));
                }
            }
        }
        private string _hitDieString;
        /// <summary>
        /// Gets or sets the hit dice for this combatant
        /// </summary>
        public string HitDieString
        {
            get { return _hitDieString; }
            set
            {
                if (!string.Equals(_hitDieString, value))
                {
                    _hitDieString = value;
                    this.RaisePropertiesChanged(nameof(HitDieString), nameof(IsValid));
                }
            }
        }
        private RollingStrategy _hitDieRollingStrategy;
        /// <summary>
        /// Gets or sets the rolling strategy to use for hit points
        /// </summary>
        public RollingStrategy HitDieRollingStrategy
        {
            get { return _hitDieRollingStrategy; }
            set
            {
                if (_hitDieRollingStrategy != value)
                {
                    _hitDieRollingStrategy = value;
                    this.RaisePropertiesChanged(nameof(HitDieRollingStrategy), nameof(IsValid));
                }
            }
        }
        private int _initiativeModifier;
        /// <summary>
        /// Gets or sets the modifier for the combatant's initiative
        /// </summary>
        public int InitiativeModifier
        {
            get { return _initiativeModifier; }
            set
            {
                if (_initiativeModifier != value)
                {
                    _initiativeModifier = value;
                    this.RaisePropertiesChanged(nameof(InitiativeModifier), nameof(IsValid));
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
                    this.RaisePropertiesChanged(nameof(DeadAt), nameof(IsValid));
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
                    this.RaisePropertiesChanged(nameof(UnconsciousAt), nameof(IsValid));
                }
            }
        }

        private ObservableCollection<DamageReduction> _damageReduction;
        /// <summary>
        /// Gets the view model used for editing damage reduction
        /// </summary>
        public ObservableCollection<DamageReduction> DamageReduction
        {
            get { return _damageReduction; }
            private set
            {
                if (!ReferenceEquals(_damageReduction, value))
                {
                    _damageReduction = value;
                    this.RaisePropertiesChanged(nameof(DamageReduction), nameof(IsValid));
                }
            }
        }
        private string _count;
        /// <summary>
        /// Gets or sets the count of combatants to create when the combat starts
        /// </summary>
        public string Count
        {
            get { return _count; }
            set
            {
                if (!string.Equals(_count, value))
                {
                    _count = value;
                    this.RaisePropertiesChanged(nameof(Count), nameof(IsValid));
                }
            }
        }
        private ICombatantTemplateSource? _source;
        /// <summary>
        /// Gets or sets the source for this combatant
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
                if (_source != null)
                {
                    Name = _source.Name ?? string.Empty;
                    HitDieString = _source.HitDieString;
                    InitiativeModifier = _source.InitiativeModifier;
                    FastHealing = _source.FastHealing;
                    DeadAt = _source.DeadAt;
                    UnconsciousAt = _source.UnconsciousAt;
                    DamageReduction = Combat.DamageReduction.Parse(_source, "damageReduction").ToObservableCollection();
                }
            }
        }
        /// <summary>
        /// Gets whether or not this view model's data is valid
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Name)
                  && Dice.IsValidString(HitDieString)
                  && Dice.IsValidString(Count);
            }
        }
        private bool _displayToPlayers;
        /// <summary>
        /// Gets or sets whether or not the player's can see this combatant
        /// </summary>
        public bool DisplayToPlayers
        {
            get { return _displayToPlayers; }
            set
            {
                if (_displayToPlayers != value)
                {
                    _displayToPlayers = value;
                    this.RaisePropertiesChanged(nameof(DisplayToPlayers), nameof(IsValid));
                }
            }
        }
        private string? _displayName;
        /// <summary>
        /// Gets or sets the name to display to players when combat starts
        /// </summary>
        public string? DisplayName
        {
            get { return _displayName; }
            set
            {
                if (!string.Equals(_displayName, value))
                {
                    _displayName = value;
                    this.RaisePropertiesChanged(nameof(DisplayName), nameof(IsValid));
                }
            }
        }
        private int _fastHealing;
        /// <summary>
        /// Gets or sets the amount of fast healing this combatant has
        /// </summary>
        public int FastHealing
        {
            get { return _fastHealing; }
            set
            {
                if (_fastHealing != value)
                {
                    _fastHealing = value;
                    this.RaisePropertiesChanged(nameof(FastHealing), nameof(IsValid));
                }
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Updates the data with the information from this view model
        /// </summary>
        /// <returns>Combatant template saved</returns>
        public ICombatantTemplate Save()
        {
            if (!IsValid)
                throw new InvalidOperationException("This view model is not in a valid state to save.");

            Combatant.Count = Count;
            Combatant.DisplayName = DisplayName;
            Combatant.DisplayToPlayers = DisplayToPlayers;
            Combatant.FastHealing = FastHealing;
            Combatant.HitDieRollingStrategy = HitDieRollingStrategy;
            Combatant.HitDieString = HitDieString;
            Combatant.InitiativeModifier = InitiativeModifier;
            Combatant.Name = Name;
            Combatant.Source = Source;
            Combatant.DeadAt = DeadAt;
            Combatant.UnconsciousAt = UnconsciousAt;
            Combatant.DamageReduction.CopyFrom(DamageReduction);

            return Combatant;
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