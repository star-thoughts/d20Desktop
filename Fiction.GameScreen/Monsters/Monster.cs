using Fiction.GameScreen.Combat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Fiction.GameScreen.Monsters
{
    /// <summary>
    /// Contains information about a single monster
    /// </summary>
    public sealed class Monster : ICampaignObject, INotifyPropertyChanged, ICombatantTemplateSource, ISourcedItem, IFilterable
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="Monster"/>
        /// </summary>
        /// <param name="campaign">Campaign the monster is in</param>
        /// <param name="name">Name of the monster</param>
        /// <param name="stats">Monster's stats</param>
        public Monster(CampaignSettings campaign, string name, IMonsterStats stats)
        {
            Exceptions.ThrowIfArgumentNull(campaign, nameof(campaign));

            Campaign = campaign;
            _name = name;
            Stats = stats;
            Id = Campaign.GetNextId();
        }
        /// <summary>
        /// Constructs a new <see cref="Monster"/>
        /// </summary>
        /// <param name="campaign">Campaign the monster is in</param>
        /// <param name="id">Monster's ID in the campaign</param>
        /// <param name="name">Name of the monster</param>
        /// <param name="stats">Monster's stats</param>
        public Monster(CampaignSettings campaign, int id, string name, IMonsterStats stats)
        {
            Campaign = campaign;
            _name = name;
            Stats = stats;
            Id = id;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the campaign this monster is in
        /// </summary>
        public CampaignSettings Campaign { get; private set; }
        /// <summary>
        /// Gets the ID of the monster in the campaign
        /// </summary>
        public int Id { get; private set; }
        private string _name;
        /// <summary>
        /// Gets or sets the name of this monster
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!string.Equals(_name, value, StringComparison.CurrentCulture))
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }
        private IMonsterStats _stats;
        /// <summary>
        /// Gets or sets the stats for this monser
        /// </summary>
        public IMonsterStats Stats
        {
            get { return _stats; }
            set
            {
                if (!ReferenceEquals(_stats, value))
                {
                    _stats = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _hitDieString;
        /// <summary>
        /// Gets or sets the hit die for this monster
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
                }
            }
        }
        /// <summary>
        /// Gets whether or not this monster's stats are valid
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Name)
                    && !string.IsNullOrWhiteSpace(HitDieString);
            }
        }
        private int _fastHealing;
        /// <summary>
        /// Gets or sets the amount of fast healing
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
        /// Gets or sets the hit points the combatant dies at
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
        /// Gets or sets the hit points the combatant goes unconscious at
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
        /// <summary>
        /// Gets whether or not this is a player character
        /// </summary>
        public bool IsPlayer { get { return false; } }
        private string _source;
        /// <summary>
        /// Gets or sets the source of this monster
        /// </summary>
        public string Source
        {
            get { return _source; }
            set
            {
                if (!string.Equals(_source, value, StringComparison.CurrentCulture))
                {
                    _source = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets the source type for display purposes
        /// </summary>
        public string SourceType
        {
            get { return Resources.Resources.MonsterSourceType; }
        }

        /// <summary>
        /// Gets whether or not this can be displayed in a filtered list
        /// </summary>
        /// <param name="filterText">Text filtered by</param>
        /// <returns>Whether or not to display this monster</returns>
        public bool CanDisplay(string filterText)
        {
            return this.MatchesFilter(filterText, Name)
                || string.Equals(filterText, Stats["group"]?.Value as string, StringComparison.CurrentCultureIgnoreCase);
        }
        #endregion
        #region Methods
        #endregion
        #region Events
        /// <summary>
        /// Event that is triggered when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
