using Fiction.GameScreen.Spells;
using System.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for editing a spell
    /// </summary>
    public sealed class EditSpellViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Creates a new <see cref="EditSpellViewModel"/> for editing an existing spell
        /// </summary>
        /// <param name="spell">Spell to edit</param>
        /// <param name="createCopy">Creating a copy of the spell passed in</param>
        public EditSpellViewModel(Spell spell, bool createCopy)
        {
            Spell = spell;
            IsCopy = createCopy;
            Campaign = spell.Campaign;
            AssignSpell(spell);
        }

        /// <summary>
        /// Creates a new <see cref="EditSpellViewModel"/> for creating a new spell
        /// </summary>
        /// <param name="campaign">Campaign to add the spell to</param>
        public EditSpellViewModel(CampaignSettings campaign)
        {
            Campaign = campaign;
            SpellLevels = new ObservableCollection<SpellLevel>();
            EffectTypes = new ObservableCollection<string>();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the original spell to edit, if any
        /// </summary>
        public Spell Spell { get; private set; }
        /// <summary>
        /// Gets whether or not this is a copy of another spell
        /// </summary>
        public bool IsCopy { get; private set; }
        /// <summary>
        /// Gets the campaign this spell is in
        /// </summary>
        public CampaignSettings Campaign { get; private set; }
        private string _name;
        /// <summary>
        /// Gets or sets the name of this spell
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.Equals(_name, value, StringComparison.Ordinal))
                {
                    _name = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _school;
        /// <summary>
        /// Gets or sets the school for this spell
        /// </summary>
        public string School
        {
            get { return _school; }
            set
            {
                if (!string.Equals(_school, value, StringComparison.Ordinal))
                {
                    _school = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _subSchool;
        /// <summary>
        /// Gets or sets the subschool for this spell
        /// </summary>
        public string SubSchool
        {
            get { return _subSchool; }
            set
            {
                if (!string.Equals(_subSchool, value, StringComparison.Ordinal))
                {
                    _subSchool = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private ObservableCollection<SpellLevel> _spellLevels;
        /// <summary>
        /// Gets or sets spell levels for this spell
        /// </summary>
        public ObservableCollection<SpellLevel> SpellLevels
        {
            get { return _spellLevels; }
            private set
            {
                if (!ReferenceEquals(_spellLevels, value))
                {
                    _spellLevels = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _castingTime;
        /// <summary>
        /// Gets or sets the casting time of this spell
        /// </summary>
        public string CastingTime
        {
            get { return _castingTime; }
            set
            {
                if (!string.Equals(_castingTime, value, StringComparison.Ordinal))
                {
                    _castingTime = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _components;
        /// <summary>
        /// Gets or sets the components for this spell
        /// </summary>
        public string Components
        {
            get { return _components; }
            set
            {
                if (!string.Equals(_components, value, StringComparison.Ordinal))
                {
                    _components = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _costlyComponents;
        /// <summary>
        /// Gets or sets the costly components for this spell
        /// </summary>
        public string CostlyComponents
        {
            get { return _costlyComponents; }
            set
            {
                if (!string.Equals(_costlyComponents, value, StringComparison.Ordinal))
                {
                    _costlyComponents = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _range;
        /// <summary>
        /// Gets or sets the range of this spell
        /// </summary>
        public string Range
        {
            get { return _range; }
            set
            {
                if (!string.Equals(_range, value, StringComparison.Ordinal))
                {
                    _range = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _area;
        /// <summary>
        /// Gets or sets the spell's area
        /// </summary>
        public string Area
        {
            get { return _area; }
            set
            {
                if (!string.Equals(_area, value, StringComparison.Ordinal))
                {
                    _area = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _effect;
        /// <summary>
        /// Gets or sets the effect of the spell
        /// </summary>
        public string Effect
        {
            get { return _effect; }
            set
            {
                if (!string.Equals(_effect, value, StringComparison.Ordinal))
                {
                    _effect = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _targets;
        /// <summary>
        /// Gets or sets the targets
        /// </summary>
        public string Targets
        {
            get { return _targets; }
            set
            {
                if (!string.Equals(_targets, value, StringComparison.Ordinal))
                {
                    _targets = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _duration;
        /// <summary>
        /// Gets or sets the spell's duration
        /// </summary>
        public string Duration
        {
            get { return _duration; }
            set
            {
                if (!string.Equals(_duration, value, StringComparison.Ordinal))
                {
                    _duration = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private bool _dismissable;
        /// <summary>
        /// Gets or sets whether or not this spell is dismissible
        /// </summary>
        public bool Dismissible
        {
            get { return _dismissable; }
            set
            {
                if (_dismissable != value)
                {
                    _dismissable = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private bool _shapeable;
        /// <summary>
        /// Gets or sets whether this spell is shapeable
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
        private string _savingThrow;
        /// <summary>
        /// Gets or sets the saving throw for this spell
        /// </summary>
        public string SavingThrow
        {
            get { return _savingThrow; }
            set
            {
                if (!string.Equals(_savingThrow, value, StringComparison.Ordinal))
                {
                    _savingThrow = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _spellResistance;
        /// <summary>
        /// Gets or sets if spell resistance applies to this spell
        /// </summary>
        public string SpellResistance
        {
            get { return _spellResistance; }
            set
            {
                if (!string.Equals(_spellResistance, value, StringComparison.Ordinal))
                {
                    _spellResistance = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _description;
        /// <summary>
        /// Gets or sets the description of this spell
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                if (!string.Equals(_description, value, StringComparison.Ordinal))
                {
                    _description = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _source;
        /// <summary>
        /// Gets or sets the source of this spell
        /// </summary>
        public string Source
        {
            get { return _source; }
            set
            {
                if (!string.Equals(_source, value, StringComparison.Ordinal))
                {
                    _source = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string _shortDescription;
        /// <summary>
        /// Gets or sets the short description of this spell
        /// </summary>
        public string ShortDescription
        {
            get { return _shortDescription; }
            set
            {
                if (!string.Equals(_shortDescription, value, StringComparison.Ordinal))
                {
                    _shortDescription = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private ObservableCollection<string> _effectTypes;
        /// <summary>
        /// Gets or sets the types of effects for this spell
        /// </summary>
        public ObservableCollection<string> EffectTypes
        {
            get { return _effectTypes; }
            set
            {
                if (!ReferenceEquals(_effectTypes, value))
                {
                    _effectTypes = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid => true;
        #endregion
        #region Methods

        /// <summary>
        /// Saves the spell's information
        /// </summary>
        public void Save()
        {
            if (Spell == null || IsCopy)
            {
                Spell = new Spell(Campaign);
                Campaign.Spells.Spells.Add(Spell);
            }

            Spell.Area = Area;
            Spell.CastingTime = CastingTime;
            Spell.Components = Components;
            Spell.CostlyComponents = CostlyComponents;
            Spell.Description = Description;
            Spell.Dismissible = Dismissible;
            Spell.Duration = Duration;
            Spell.Effect = Effect;
            Spell.Name = Name;
            Spell.Range = Range;
            Spell.SavingThrow = SavingThrow;
            Spell.School = School;
            Spell.Shapeable = Shapeable;
            Spell.ShortDescription = ShortDescription;
            Spell.Source = Source;
            Spell.SpellResistance = SpellResistance;
            Spell.SubSchool = SubSchool;
            Spell.Targets = Targets;

            Spell.EffectTypes.Clear();
            foreach (string effect in EffectTypes)
                Spell.EffectTypes.Add(effect);

            Spell.Levels.Clear();
            foreach (SpellLevel level in SpellLevels)
                Spell.Levels.Add(level);
        }

        private void AssignSpell(Spell spell)
        {
            Area = spell.Area;
            CastingTime = spell.CastingTime;
            Components = spell.Components;
            CostlyComponents = spell.CostlyComponents;
            Description = spell.Description;
            Dismissible = spell.Dismissible;
            Duration = spell.Duration;
            Effect = spell.Effect;
            EffectTypes = spell.EffectTypes.ToObservableCollection();
            Name = spell.Name;
            Range = spell.Range;
            SavingThrow = spell.SavingThrow;
            School = spell.School;
            Shapeable = spell.Shapeable;
            ShortDescription = spell.ShortDescription;
            Source = spell.Source;
            SpellLevels = spell.Levels.Select(p => new SpellLevel() { Class = p.Class, Level = p.Level }).ToObservableCollection();
            SpellResistance = spell.SpellResistance;
            SubSchool = spell.SubSchool;
            Targets = spell.Targets;
        }
        #endregion
    }
}
