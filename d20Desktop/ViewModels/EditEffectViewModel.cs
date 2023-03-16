using Fiction.GameScreen.Combat;
using System.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for editing effects
    /// </summary>
    public sealed class EditEffectViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="EditEffectViewModel"/>
        /// </summary>
        /// <param name="campaign">Campaign the effect is in</param>
        /// <param name="effect">Effect to edit</param>
        public EditEffectViewModel(CampaignSettings campaign, Effect effect)
        {
            _effect = effect;
            Campaign = campaign;

            Name = _effect.Name;
            InitiativeSource = _effect.InitiativeSource;
            Source = _effect.Source;
            Targets = _effect.Targets.ToObservableCollection();
            RelatedItem = _effect.RelatedItem;
            Duration = _effect.DurationRounds;
            RemainingRounds = _effect.RemainingRounds;
        }
        /// <summary>
        /// Constructs a new <see cref="EditEffectViewModel"/> for a new effect
        /// </summary>
        /// <param name="campaign">Campaign to add the effect to</param>
        public EditEffectViewModel(CampaignSettings campaign)
        {
            Campaign = campaign;
            Targets = new ObservableCollection<ICombatant>();
        }
        #endregion
        #region Member Variables
        private Effect _effect;
        #endregion
        #region Properties
        /// <summary>
        /// Gets the currently active combat
        /// </summary>
        public ActiveCombat Combat { get { return Campaign.Combat.Active; } }
        /// <summary>
        /// Gets the campaign
        /// </summary>
        public CampaignSettings Campaign { get; private set; }
        private string _name;
        /// <summary>
        /// Gets or sets the name of the effect
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
        private ICombatant _initiativeSource;
        /// <summary>
        /// Gets or sets the combatant that controls the duration
        /// </summary>
        public ICombatant InitiativeSource
        {
            get { return _initiativeSource; }
            set
            {
                if (!ReferenceEquals(_initiativeSource, value))
                {
                    _initiativeSource = value;
                    this.RaisePropertiesChanged(nameof(InitiativeSource), nameof(IsValid));
                }
            }
        }
        private ICombatant _source;
        /// <summary>
        /// Gets or sets the source of this effect
        /// </summary>
        public ICombatant Source
        {
            get { return _source; }
            set
            {
                if (!ReferenceEquals(_source, value))
                {
                    _source = value;
                    this.RaisePropertiesChanged(nameof(InitiativeSource), nameof(IsValid));
                }
            }
        }
        private ObservableCollection<ICombatant> _targets;
        /// <summary>
        /// Gets a collection of targets of the effect
        /// </summary>
        public ObservableCollection<ICombatant> Targets
        {
            get { return _targets; }
            private set
            {
                if (!ReferenceEquals(_targets, value))
                {
                    _targets = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private ICampaignObject _relatedItem;
        /// <summary>
        /// Gets or sets an item related to this one
        /// </summary>
        public ICampaignObject RelatedItem
        {
            get { return _relatedItem; }
            set
            {
                bool wasSameName = _relatedItem != null && string.Equals(_relatedItem.Name, Name, StringComparison.CurrentCultureIgnoreCase);

                _relatedItem = value;

                if (_relatedItem != null)
                {
                    if (string.IsNullOrEmpty(Name) || wasSameName)
                        Name = _relatedItem.Name;
                }

                this.RaisePropertyChanged();
            }
        }
        private int _duration;
        /// <summary>
        /// Gets or sets the duration of the effect
        /// </summary>
        public int Duration
        {
            get { return _duration; }
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    RemainingRounds = value;
                    this.RaisePropertiesChanged(nameof(Duration), nameof(IsValid));
                }
            }
        }
        private int _remainingRounds;
        /// <summary>
        /// Gets or sets the remaining duration in rounds
        /// </summary>
        public int RemainingRounds
        {
            get { return _remainingRounds; }
            set
            {
                if (_remainingRounds != value)
                {
                    _remainingRounds = value;
                    this.RaisePropertiesChanged(nameof(RemainingRounds), nameof(IsValid));
                }
            }
        }
        /// <summary>
        /// Gets whether or not this is a valid view model
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return InitiativeSource != null
                    && Source != null
                    && Duration > 0
                    && !string.IsNullOrWhiteSpace(Name);
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Saves this effect
        /// </summary>
        public Effect Save()
        {
            if (_effect == null)
                _effect = new Effect(Source, Duration, Targets.ToArray());

            _effect.Name = Name;
            _effect.InitiativeSource = InitiativeSource;
            _effect.Source = Source;
            _effect.RelatedItem = RelatedItem;
            _effect.DurationRounds = Duration;
            _effect.RemainingRounds = RemainingRounds;

            _effect.Targets.Clear();
            foreach (ICombatant target in Targets)
                _effect.Targets.Add(target);

            return _effect;
        }
        #endregion
    }
}
