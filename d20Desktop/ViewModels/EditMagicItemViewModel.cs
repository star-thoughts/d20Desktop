using Fiction.GameScreen.Equipment;
using Fiction.GameScreen.Monsters;
using System.Collections.ObjectModel;
using System;
using System.ComponentModel;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for editing magic items
    /// </summary>
    public sealed class EditMagicItemViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="EditMagicItemViewModel"/> for an existing item
        /// </summary>
        /// <param name="item">Item to edit</param>
        public EditMagicItemViewModel(CampaignSettings campaign, MagicItem item)
        {
            _item = item;
            Campaign = campaign;

            _name = item.Name;
            CasterLevel = item.CasterLevel;
            _slot = item.Slot;
            PriceInCopper = item.PriceInCopper;
            _weight = item.Weight;
            _description = item.Description;
            _requirements = item.Requirements;
            CostInCopper = item.CostInCopper;
            _group = item.Group;
            _source = item.Source;
            IsIntelligent = item.IsIntelligent;
            Alignment = item.Alignment;
            Intelligence = item.Intelligence;
            Wisdom = item.Wisdom;
            Charisma = item.Charisma;
            Ego = item.Ego;
            _communication = item.Communication;
            _senses = item.Senses;
            _powers = item.Powers;
            _languages = item.Languages;
            _destruction = item.Descruction;
            ArtifactLevel = item.ArtifactLevel;
            _auraStrength = item.AuraStrength;
            Mythic = item.Mythic;

            Auras = new ObservableCollection<string>();
            foreach (string aura in item.Auras)
                Auras.Add(aura);
        }
        /// <summary>
        /// Constructs a new <see cref="EditMagicItemViewModel"/> for a new item
        /// </summary>
        /// <param name="campaign">Campaign to add the item to</param>
        public EditMagicItemViewModel(CampaignSettings campaign)
        {
            Campaign = campaign;
            Auras = new ObservableCollection<string>();
        }
        #endregion
        #region Member Variables
        private MagicItem? _item;
        #endregion
        #region Properties
        /// <summary>
        /// Gets the campaign
        /// </summary>
        public CampaignSettings Campaign { get; private set; }
        private string? _name;
        /// <summary>
        /// Gets or sets the name of this magic item
        /// </summary>
        public string? Name
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
                if (!string.Equals(_slot, value))
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
                if (!string.Equals(_weight, value))
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
                if (!string.Equals(_description, value))
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
                if (!string.Equals(_requirements, value))
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
                if (!string.Equals(_group, value))
                {
                    _group = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private string? _source;
        /// <summary>
        /// Gets or sets the source of this item
        /// </summary>
        public string? Source
        {
            get { return _source; }
            set
            {
                if (!string.Equals(_source, value))
                {
                    _source = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool _isIntelligent;
        /// <summary>
        /// Gets whether or not this item is intelligent
        /// </summary>
        public bool IsIntelligent
        {
            get { return _isIntelligent; }
            set
            {
                if (_isIntelligent != value)
                {
                    _isIntelligent = value;
                    this.RaisePropertyChanged();
                }
            }
        }

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
                if (!string.Equals(_senses, value))
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
                if (!string.Equals(_powers, value))
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
                if (!string.Equals(_languages, value))
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
                if (!string.Equals(_destruction, value))
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
                if (!string.Equals(_auraStrength, value))
                {
                    _auraStrength = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool _isMythic;
        /// <summary>
        /// Gets or sets whether or not this item is mythic
        /// </summary>
        public bool Mythic
        {
            get { return _isMythic; }
            set
            {
                if (_isMythic != value)
                {
                    _isMythic = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets whether or not the data in this view model is valid
        /// </summary>
        public override bool IsValid => !string.IsNullOrWhiteSpace(Name);
        #endregion
        #region Methods
        /// <summary>
        /// Marks this item as a copy
        /// </summary>
        public void MarkAsCopy()
        {
            _item = null;
        }
        /// <summary>
        /// Saves the item in the campaign
        /// </summary>
        public void Save()
        {
            if (!IsValid)
                throw new InvalidOperationException("Cannot save magic item without a proper name.");

            if (_item == null)
            {
                _item = new MagicItem(Campaign);
                Campaign.EquipmentManager.MagicItems.Add(_item);
            }

            _item.Name = Name!;
            _item.CasterLevel = CasterLevel;
            _item.Slot = Slot;
            _item.PriceInCopper = PriceInCopper;
            _item.Weight = Weight;
            _item.Description = Description;
            _item.Requirements = Requirements;
            _item.CostInCopper = CostInCopper;
            _item.Group = Group;
            _item.Source = Source;
            _item.Alignment = Alignment;
            _item.Intelligence = Intelligence;
            _item.Wisdom = Wisdom;
            _item.Charisma = Charisma;
            _item.Ego = Ego;
            _item.Communication = Communication;
            _item.Senses = Senses;
            _item.Powers = Powers;
            _item.Languages = Languages;
            _item.Descruction = Descruction;
            _item.ArtifactLevel = ArtifactLevel;
            _item.AuraStrength = AuraStrength;
            _item.Mythic = Mythic;

            _item.Auras.Clear();
            foreach (string aura in Auras)
                _item.Auras.Add(aura);

            Campaign.ReconcileSources();
            Campaign.EquipmentManager.Reconcile();
        }
        #endregion
    }
}
