using Fiction.GameScreen.Monsters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Fiction.GameScreen.Equipment
{
    public sealed class MagicItem : INotifyPropertyChanged, ICampaignObject, ISourcedItem, IFilterable
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="MagicItem"/> for a new item
        /// </summary>
        /// <param name="campaign">Campaign the item is being added to</param>
        public MagicItem(CampaignSettings campaign)
        {
            _name = string.Empty;
            Campaign = campaign;

            Id = campaign.GetNextId();
            Auras = new ObservableCollection<string>();
        }
        /// <summary>
        /// Constructs a new <see cref="MagicItem"/> for an existing item
        /// </summary>
        /// <param name="name">Name of the item</param>
        /// <param name="group">Group the item is in</param>
        /// <param name="id">ID of the item in the campaign</param>
        public MagicItem(CampaignSettings campaign, string name, string group, int id)
        {
            Campaign = campaign;
            _name = name;
            _group = group;
            Id = id;
            Auras = new ObservableCollection<string>();
        }
        #endregion
        #region Properties
        private string _name;
        /// <summary>
        /// Gets or sets the name of this magic item
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

        private int _casterLevel;
        /// <summary>
        /// Gets or sets the item's caster level
        /// </summary>
        public int CasterLevel
        {
            get { return _casterLevel; }
            set
            {
                if (_casterLevel != value)
                {
                    _casterLevel = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string? _slot;
        /// <summary>
        /// Gets or sets the slot the item takes up, if any
        /// </summary>
        public string? Slot
        {
            get { return _slot; }
            set
            {
                if (!string.Equals(_slot, value, StringComparison.CurrentCulture))
                {
                    _slot = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private int _priceInCopper;
        /// <summary>
        /// Gets or sets the price of this item in copper
        /// </summary>
        public int PriceInCopper
        {
            get { return _priceInCopper; }
            set
            {
                if (_priceInCopper != value)
                {
                    _priceInCopper = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string? _weight;
        /// <summary>
        /// Gets or sets the weight of this item
        /// </summary>
        public string? Weight
        {
            get { return _weight; }
            set
            {
                if (!string.Equals(_weight, value, StringComparison.CurrentCulture))
                {
                    _weight = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string? _description;
        /// <summary>
        /// Gets or sets the description of this item
        /// </summary>
        public string? Description
        {
            get { return _description; }
            set
            {
                if (!string.Equals(_description, value, StringComparison.CurrentCulture))
                {
                    _description = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string? _requirements;
        /// <summary>
        /// Gets or sets the requirements to make the item
        /// </summary>
        public string? Requirements
        {
            get { return _requirements; }
            set
            {
                if (!string.Equals(_requirements, value, StringComparison.CurrentCulture))
                {
                    _requirements = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private int _costInCopper;
        /// <summary>
        /// Gets or sets the price to create the item in copper
        /// </summary>
        public int CostInCopper
        {
            get { return _costInCopper; }
            set
            {
                if (_costInCopper != value)
                {
                    _costInCopper = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string? _group;
        /// <summary>
        /// Gets or sets the group (weapon, armor, rings, etc)
        /// </summary>
        public string? Group
        {
            get { return _group; }
            set
            {
                if (!string.Equals(_group, value, StringComparison.CurrentCulture))
                {
                    _group = value;
                    this.RaisePropertiesChanged(nameof(Group), nameof(SourceType));
                }
            }
        }



        /// <summary>
        /// Gets the campaign this item is in
        /// </summary>
        public CampaignSettings Campaign { get; private set; }
        /// <summary>
        /// Gets the ID of this item
        /// </summary>
        public int Id { get; private set; }
        private string? _source;
        /// <summary>
        /// Gets or sets the source of this item
        /// </summary>
        public string? Source
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
        /// Gets whether or not this item is intelligent
        /// </summary>
        public bool IsIntelligent { get { return _alignment != Alignment.Unknown; } }

        private Alignment _alignment;
        /// <summary>
        /// Gets or sets the alignment of this item, if any
        /// </summary>
        public Alignment Alignment
        {
            get { return _alignment; }
            set
            {
                if (_alignment != value)
                {
                    _alignment = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private int _intelligence;
        /// <summary>
        /// Gets or sets the intelligence of this item
        /// </summary>
        public int Intelligence
        {
            get { return _intelligence; }
            set
            {
                if (_intelligence != value)
                {
                    _intelligence = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private int _wisdom;
        /// <summary>
        /// Gets or sets the wisdom of this item
        /// </summary>
        public int Wisdom
        {
            get { return _wisdom; }
            set
            {
                if (_wisdom != value)
                {
                    _wisdom = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private int _charisma;
        /// <summary>
        /// Gets or sets the charisma of this item
        /// </summary>
        public int Charisma
        {
            get { return _charisma; }
            set
            {
                if (_charisma != value)
                {
                    _charisma = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private int _ego;
        /// <summary>
        /// Gets or sets this item's ego
        /// </summary>
        public int Ego
        {
            get { return _ego; }
            set
            {
                if (_ego != value)
                {
                    _ego = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string? _communication;
        /// <summary>
        /// Gets or sets the method(s) of communication this item can employ
        /// </summary>
        public string? Communication
        {
            get { return _communication; }
            set
            {
                if (!string.Equals(_communication, value))
                {
                    _communication = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string? _senses;
        /// <summary>
        /// Gets or sets this item's senses
        /// </summary>
        public string? Senses
        {
            get { return _senses; }
            set
            {
                if (!string.Equals(_senses, value, StringComparison.CurrentCulture))
                {
                    _senses = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string? _powers;
        /// <summary>
        /// Gets or sets the powers of this item
        /// </summary>
        public string? Powers
        {
            get { return _powers; }
            set
            {
                if (!string.Equals(_powers, value, StringComparison.CurrentCulture))
                {
                    _powers = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string? _languages;
        /// <summary>
        /// Gets or sets the languages
        /// </summary>
        public string? Languages
        {
            get { return _languages; }
            set
            {
                if (!string.Equals(_languages, value, StringComparison.CurrentCulture))
                {
                    _languages = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string? _destruction;
        /// <summary>
        /// Gets or sets the requirements to destroy this item (artifacts only)
        /// </summary>
        public string? Descruction
        {
            get { return _destruction; }
            set
            {
                if (!string.Equals(_destruction, value, StringComparison.CurrentCulture))
                {
                    _destruction = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private ArtifactLevel _artifactLevel;
        /// <summary>
        /// Gets or sets whether or not and what level this artifact is
        /// </summary>
        public ArtifactLevel ArtifactLevel
        {
            get { return _artifactLevel; }
            set
            {
                if (_artifactLevel != value)
                {
                    _artifactLevel = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets a collection of auras this item projects
        /// </summary>
        public ObservableCollection<string> Auras { get; private set; }

        private string? _auraStrength;
        /// <summary>
        /// Gets or sets the strength of the aura
        /// </summary>
        public string? AuraStrength
        {
            get { return _auraStrength; }
            set
            {
                if (!string.Equals(_auraStrength, value, StringComparison.CurrentCulture))
                {
                    _auraStrength = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool _mythic;
        /// <summary>
        /// Gets or sets whether or not this item is mythic
        /// </summary>
        public bool Mythic
        {
            get { return _mythic; }
            set
            {
                if (_mythic != value)
                {
                    _mythic = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the source type for this item
        /// </summary>
        public string? SourceType => Group;
        #endregion
        #region Methods
        /// <summary>
        /// Gets whether or not the item matches the filter text
        /// </summary>
        /// <param name="filterText">Text to match against</param>
        /// <returns>Whether or not to display the item</returns>
        public bool CanDisplay(string filterText)
        {
            return string.IsNullOrWhiteSpace(filterText)
                || this.MatchesFilter(filterText, Name)
                || string.Equals(filterText, Group);
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
