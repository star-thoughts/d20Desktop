using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Contains information on an effect during combat
    /// </summary>
    public sealed class Effect : INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="Effect"/>
        /// </summary>
        /// <param name="source">Source of the effect</param>
        /// <param name="targets">Targets for the effect</param>
        public Effect(ICombatant source, int duration, params ICombatant[] targets)
        {
            _initiativeSource = source;
            Source = source;
            DurationRounds = duration;
            RemainingRounds = duration;
            Targets = targets.ToObservableCollection();
        }
        #endregion
        #region Properties
        private string? _name;
        /// <summary>
        /// Gets or sets the name of the effect
        /// </summary>
        public string? Name
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
        private ICombatant? _initiativeSource;
        /// <summary>
        /// Gets or sets the initiative source
        /// </summary>
        /// <remarks>
        /// The initiative source is used to decrement remaining rounds and expiration.  It is normally
        /// the <see cref="Source"/>, but can move to another if the <see cref="Source"/> moves to another spot
        /// in initiative.
        /// </remarks>
        public ICombatant? InitiativeSource
        {
            get { return _initiativeSource; }
            set
            {
                if (!ReferenceEquals(_initiativeSource, value))
                {
                    _initiativeSource = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private ICampaignObject? _relatedItem;
        /// <summary>
        /// Gets an item related to this effect
        /// </summary>
        public ICampaignObject? RelatedItem
        {
            get { return _relatedItem; }
            set
            {
                if (!ReferenceEquals(_relatedItem, value))
                {
                    _relatedItem = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private ICombatant? _source;
        /// <summary>
        /// Gets or sets the source of the effect, if any
        /// </summary>
        public ICombatant? Source
        {
            get { return _source; }
            set
            {
                if (!ReferenceEquals(_source, value))
                {
                    //  If the source and initiative source were the same, then move it as well
                    if (ReferenceEquals(_source, _initiativeSource))
                        InitiativeSource = value;

                    _source = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets a collection of targets of the effect
        /// </summary>
        public ObservableCollection<ICombatant> Targets { get; private set; }
        private int _duration;
        /// <summary>
        /// Gets the duration in rounds of this effect
        /// </summary>
        public int DurationRounds
        {
            get { return _duration; }
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private int _remainingRounds;
        /// <summary>
        /// Gets or sets the remaining number of rounds
        /// </summary>
        public int RemainingRounds
        {
            get { return _remainingRounds; }
            set
            {
                if (_remainingRounds != value)
                {
                    _remainingRounds = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion
        #region Methods
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
