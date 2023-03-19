using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Fiction.GameScreen.Spells
{
    /// <summary>
    /// Contains information about a spell
    /// </summary>
    public class Spell : INotifyPropertyChanged, ICampaignObject, ISourcedItem, IFilterable
    {
        #region Constructors
        /// <summary>
        /// Constructs an existing <see cref="Spell"/>
        /// </summary>
        /// <param name="campaign">Campaign this spell is in</param>
        /// <param name="id">ID of this spell</param>
        public Spell(CampaignSettings campaign, int id)
        {
            Campaign = campaign;
            Id = id;
            Levels = new ObservableCollection<SpellLevel>();
            EffectTypes = new ObservableCollection<string>();
        }
        /// <summary>
        /// Constructs a new <see cref="Spell"/> in the campaign
        /// </summary>
        /// <param name="campaign">Campaign this spell is in</param>
        public Spell(CampaignSettings campaign)
        {
            Campaign = campaign;
            Id = Campaign.GetNextId();
            Levels = new ObservableCollection<SpellLevel>();
            EffectTypes = new ObservableCollection<string>();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the campaign this spell is in
        /// </summary>
        public CampaignSettings Campaign { get; private set; }
        /// <summary>
        /// Gets the ID of this spell
        /// </summary>
        public int Id { get; private set; }
        private string? _name;
        /// <summary>
        /// Gets or sets the name of the spell
        /// </summary>
        public string? Name
        {
            get { return _name; }
            set
            {
                if (!string.Equals(_name, value, System.StringComparison.CurrentCulture))
                {
                    _name = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _school;
        /// <summary>
        /// Gets or sets the school of this spell
        /// </summary>
        public string? School
        {
            get { return _school; }
            set
            {
                if (!string.Equals(_school, value, System.StringComparison.CurrentCulture))
                {
                    _school = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _subSchool;
        /// <summary>
        /// Gets or sets the sub school for this spell
        /// </summary>
        public string? SubSchool
        {
            get { return _subSchool; }
            set
            {
                if (!string.Equals(_subSchool, value, System.StringComparison.CurrentCulture))
                {
                    _subSchool = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets a collection of level information for this spell
        /// </summary>
        public ObservableCollection<SpellLevel> Levels { get; private set; }
        private string? _castingTime;
        /// <summary>
        /// Gets the casting time of this spell
        /// </summary>
        public string? CastingTime
        {
            get { return _castingTime; }
            set
            {
                if (!string.Equals(_castingTime, value, System.StringComparison.CurrentCulture))
                {
                    _castingTime = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _components;
        /// <summary>
        /// Gets or sets the components for the spell
        /// </summary>
        public string? Components
        {
            get { return _components; }
            set
            {
                if (!string.Equals(_components, value, System.StringComparison.CurrentCulture))
                {
                    _components = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _costlyComponents;
        /// <summary>
        /// Gets or sets any costly components for this spell
        /// </summary>
        public string? CostlyComponents
        {
            get { return _costlyComponents; }
            set
            {
                if (!string.Equals(_costlyComponents, value, System.StringComparison.CurrentCulture))
                {
                    _costlyComponents = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _range;
        /// <summary>
        /// Gets or sets the range of this spell
        /// </summary>
        public string? Range
        {
            get { return _range; }
            set
            {
                if (!string.Equals(_range, value, System.StringComparison.CurrentCulture))
                {
                    _range = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _area;
        /// <summary>
        /// Gets or sets the area of effect of the spell
        /// </summary>
        public string? Area
        {
            get { return _area; }
            set
            {
                if (!string.Equals(_area, value, System.StringComparison.CurrentCulture))
                {
                    _area = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _effect;
        /// <summary>
        /// Gets or sets the effect of the spell
        /// </summary>
        public string? Effect
        {
            get { return _effect; }
            set
            {
                if (!string.Equals(_effect, value, System.StringComparison.CurrentCulture))
                {
                    _effect = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _targets;
        /// <summary>
        /// Gets or sets the targets of the spell
        /// </summary>
        public string? Targets
        {
            get { return _targets; }
            set
            {
                if (!string.Equals(_targets, value, System.StringComparison.CurrentCulture))
                {
                    _targets = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _duration;
        /// <summary>
        /// Gets or sets the duration of the spell
        /// </summary>
        public string? Duration
        {
            get { return _duration; }
            set
            {
                if (!string.Equals(_duration, value, System.StringComparison.CurrentCulture))
                {
                    _duration = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private bool _dismissible;
        /// <summary>
        /// Gets or sets whether or not this spell is dismissable
        /// </summary>
        public bool Dismissible
        {
            get { return _dismissible; }
            set
            {
                if (_dismissible != value)
                {
                    _dismissible = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private bool _shapeable;
        /// <summary>
        /// Gets or sets whether or not this spell can be shaped
        /// </summary>
        public bool Shapeable
        {
            get { return _shapeable; }
            set
            {
                if (_shapeable != value)
                {
                    _shapeable = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _savingThrow;
        /// <summary>
        /// Gets or sets the saving throw for this spell
        /// </summary>
        public string? SavingThrow
        {
            get { return _savingThrow; }
            set
            {
                if (!string.Equals(_savingThrow, value, System.StringComparison.CurrentCulture))
                {
                    _savingThrow = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _spellResistance;
        /// <summary>
        /// Gets or sets whether or not spell resistance applies to this spell
        /// </summary>
        public string? SpellResistance
        {
            get { return _spellResistance; }
            set
            {
                if (!string.Equals(_spellResistance, value, System.StringComparison.CurrentCulture))
                {
                    _spellResistance = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _description;
        /// <summary>
        /// Gets or sets the descriptive text for the spell
        /// </summary>
        public string? Description
        {
            get { return _description; }
            set
            {
                if (!string.Equals(_description, value, System.StringComparison.CurrentCulture))
                {
                    _description = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _source;
        /// <summary>
        /// Gets or sets the reference source this spell came from
        /// </summary>
        public string? Source
        {
            get { return _source; }
            set
            {
                if (!string.Equals(_source, value, System.StringComparison.CurrentCulture))
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
            get { return Resources.Resources.SpellSourceType; }
        }

        private string? _shortDescription;
        /// <summary>
        /// Gets or sets the short description for this spell
        /// </summary>
        public string? ShortDescription
        {
            get { return _shortDescription; }
            set
            {
                if (!string.Equals(_shortDescription, value, System.StringComparison.CurrentCulture))
                {
                    _shortDescription = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets types of effects of this spell (darkness, light, chaos, evil, disease, mind-effecting, etc)
        /// </summary>
        public ObservableCollection<string> EffectTypes { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Gets whether or not the item matches the filter text
        /// </summary>
        /// <param name="filterText">Text to match against</param>
        /// <returns>Whether or not to display the item</returns>
        public bool CanDisplay(string filterText)
        {
            return string.IsNullOrEmpty(filterText)
                || this.MatchesFilter(filterText, Name)
                || string.Equals(filterText, School, System.StringComparison.CurrentCultureIgnoreCase);
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