using Fiction.GameScreen.Spells;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// Filter for spells
    /// </summary>
    public sealed class SpellFilterViewModel : FilterViewModelCore<Spell>
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SpellFilterViewModel"/>
        /// </summary>
        /// <param name="campaign">Campaign to filter spells in</param>
        public SpellFilterViewModel(CampaignSettings campaign)
        {
            IEnumerable<Spell> spells = campaign.Spells.Spells;

            SchoolOptions = spells
                .Select(p => p.School)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToArray();

            SubSchoolOptions = spells
                .Select(p => p.SubSchool)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToArray();

            ClassOptions = spells
                .SelectMany(p => p.Levels)
                .Select(p => p.Class)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToArray();

            CastingTimeOptions = spells
                .Select(p => p.CastingTime)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToArray();

            RangeOptions = spells
                .Select(p => p.Range)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToArray();

            AreaOptions = spells
                .Select(p => p.Area)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToArray();

            SavingThrowOptions = spells
                .Select(p => p.SavingThrow)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToArray();

            SpellResistanceOptions = spells
                .Select(p => p.SpellResistance)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToArray();

            EffectTypeOptions = spells
                .SelectMany(p => p.EffectTypes)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToArray();

            EffectTypes = new ObservableCollection<string>();
        }
        #endregion
        #region Properties
        private string _name;
        /// <summary>
        /// Gets or sets the text to filter name by
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.Equals(_name, value, StringComparison.Ordinal))
                {
                    _name = value;
                    this.RaisePropertiesChanged(nameof(Name), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        private string _school;
        /// <summary>
        /// Gets or sets the school to filter
        /// </summary>
        public string School
        {
            get { return _school; }
            set
            {
                if (!string.Equals(_school, value, StringComparison.Ordinal))
                {
                    _school = value;
                    this.RaisePropertiesChanged(nameof(School), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        /// <summary>
        /// Gets a collection of options for <see cref="School"/>
        /// </summary>
        public string[] SchoolOptions { get; private set; }
        private string _subSchool;
        /// <summary>
        /// Gets or sets the subschool to filter
        /// </summary>
        public string SubSchool
        {
            get { return _subSchool; }
            set
            {
                if (!string.Equals(_subSchool, value, StringComparison.Ordinal))
                {
                    _subSchool = value;
                    this.RaisePropertiesChanged(nameof(SubSchool), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        /// <summary>
        /// Gets a collection of options for <see cref="SubSchool"/>
        /// </summary>
        public string[] SubSchoolOptions { get; private set; }
        private string _class;
        /// <summary>
        /// Gets or sets the class to show spells for
        /// </summary>
        public string Class
        {
            get { return _class; }
            set
            {
                if (!string.Equals(_class, value, StringComparison.Ordinal))
                {
                    _class = value;
                    this.RaisePropertiesChanged(nameof(Class), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        /// <summary>
        /// Gets a collection of options for <see cref="Class"/>
        /// </summary>
        public string[] ClassOptions { get; private set; }
        private string _classLevel;
        /// <summary>
        /// Gets or sets the class level to show spells for
        /// </summary>
        public string ClassLevel
        {
            get { return _classLevel; }
            set
            {
                if (!string.Equals(_classLevel, value, StringComparison.Ordinal))
                {
                    _classLevel = value;
                    this.RaisePropertiesChanged(nameof(ClassLevel), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        private string _castingTime;
        /// <summary>
        /// Gets or sets the casting time
        /// </summary>
        public string CastingTime
        {
            get { return _castingTime; }
            set
            {
                if (!string.Equals(_castingTime, value, StringComparison.Ordinal))
                {
                    _castingTime = value;
                    this.RaisePropertiesChanged(nameof(CastingTime), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        /// <summary>
        /// Gets a collection of options for <see cref="CastingTime"/>
        /// </summary>
        public string[] CastingTimeOptions { get; private set; }
        private string _range;
        /// <summary>
        /// Gets or sets the range to show spells for
        /// </summary>
        public string Range
        {
            get { return _range; }
            set
            {
                if (!string.Equals(_range, value, StringComparison.Ordinal))
                {
                    _range = value;
                    this.RaisePropertiesChanged(nameof(Range), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        /// <summary>
        /// Gets a collection of options for <see cref="Range"/>
        /// </summary>
        public string[] RangeOptions { get; private set; }
        private string _area;
        /// <summary>
        /// Gets or sets the area to show spells for
        /// </summary>
        public string Area
        {
            get { return _area; }
            set
            {
                if (!string.Equals(_area, value, StringComparison.Ordinal))
                {
                    _area = value;
                    this.RaisePropertiesChanged(nameof(Area), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        /// <summary>
        /// Gets a collection of options for <see cref="Area"/>
        /// </summary>
        public string[] AreaOptions { get; private set; }
        private bool? _shapeable;
        /// <summary>
        /// Gets or sets whether shapeable spells should be included
        /// </summary>
        public bool? Shapeable
        {
            get { return _shapeable; }
            set
            {
                if (_shapeable != value)
                {
                    _shapeable = value;
                    this.RaisePropertiesChanged(nameof(Shapeable), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        private string _savingThrow;
        /// <summary>
        /// Gets or sets the saving throw to show spells for
        /// </summary>
        public string SavingThrow
        {
            get { return _savingThrow; }
            set
            {
                if (!string.Equals(_savingThrow, value, StringComparison.Ordinal))
                {
                    _savingThrow = value;
                    this.RaisePropertiesChanged(nameof(SavingThrow), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        /// <summary>
        /// Gets a collection of options for <see cref="SavingThrow"/>
        /// </summary>
        public string[] SavingThrowOptions { get; private set; }
        private string _spellResistance;
        /// <summary>
        /// Gets or sets the spell resistance to show spells for
        /// </summary>
        public string SpellResistance
        {
            get { return _spellResistance; }
            set
            {
                if (!string.Equals(_spellResistance, value, StringComparison.Ordinal))
                {
                    _spellResistance = value;
                    this.RaisePropertiesChanged(nameof(SpellResistance), nameof(HasFilter), nameof(IsValid));
                }
            }
        }
        /// <summary>
        /// Gets a collection of options for <see cref="SpellResistance"/>
        /// </summary>
        public string[] SpellResistanceOptions { get; private set; }
        /// <summary>
        /// Gets a collection of effect types to show spells for
        /// </summary>
        public ObservableCollection<string> EffectTypes { get; private set; }
        /// <summary>
        /// Gets a collection of options for <see cref="EffectTypes"/>
        /// </summary>
        public string[] EffectTypeOptions { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Gets whether or not the filter is valid
        /// </summary>
        public override bool HasFilter
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Name)
                    || !string.IsNullOrWhiteSpace(School)
                    || !string.IsNullOrWhiteSpace(SubSchool)
                    || !string.IsNullOrWhiteSpace(Class)
                    || (!string.IsNullOrWhiteSpace(ClassLevel) && Int32.TryParse(ClassLevel, NumberStyles.Integer, CultureInfo.CurrentCulture, out int level) && level >= 0)
                    || !string.IsNullOrWhiteSpace(CastingTime)
                    || !string.IsNullOrWhiteSpace(Range)
                    || !string.IsNullOrWhiteSpace(Area)
                    || Shapeable.HasValue
                    || !string.IsNullOrWhiteSpace(SavingThrow)
                    || !string.IsNullOrWhiteSpace(SpellResistance)
                    || EffectTypes.Any();
            }
        }
        /// <summary>
        /// Gets whether or not the data in this view model is valid
        /// </summary>
        public override bool IsValid => HasFilter;
        /// <summary>
        /// Determines whether the given spell is a match
        /// </summary>
        /// <param name="spell">Spell to test against</param>
        /// <returns></returns>
        public override bool Matches(Spell spell)
        {
            bool match = true;

            if (string.IsNullOrEmpty(ClassLevel) || !Int32.TryParse(ClassLevel, NumberStyles.Integer, CultureInfo.CurrentCulture, out int level))
                level = -1;

            if (!string.IsNullOrWhiteSpace(Name))
                match &= IFilterableExtensions.MatchesFilter(Name, spell.Name);
            if (!string.IsNullOrWhiteSpace(School))
                match &= string.Equals(School, spell.School, StringComparison.CurrentCultureIgnoreCase);
            if (!string.IsNullOrWhiteSpace(SubSchool))
                match &= string.Equals(SubSchool, spell.SubSchool, StringComparison.CurrentCultureIgnoreCase);

            if (!string.IsNullOrWhiteSpace(Class))
            {
                if (level < 0)
                    match &= spell.Levels.Any(p => string.Equals(p.Class, Class, StringComparison.CurrentCultureIgnoreCase));
                else
                    match &= spell.Levels.Any(p => string.Equals(p.Class, Class, StringComparison.CurrentCultureIgnoreCase) && p.Level == level);
            }
            else
            {
                if (level >= 0)
                    match &= spell.Levels.Any(p => p.Level == level);
            }
            if (!string.IsNullOrWhiteSpace(CastingTime))
                match &= string.Equals(CastingTime, spell.CastingTime, StringComparison.CurrentCultureIgnoreCase);
            if (!string.IsNullOrWhiteSpace(Range))
                match &= string.Equals(Range, spell.Range, StringComparison.CurrentCultureIgnoreCase);
            if (!string.IsNullOrWhiteSpace(Area))
                match &= string.Equals(Area, spell.Area, StringComparison.CurrentCultureIgnoreCase);
            if (Shapeable.HasValue)
                match &= spell.Shapeable == Shapeable.Value;
            if (!string.IsNullOrWhiteSpace(SavingThrow))
                match &= string.Equals(SavingThrow, spell.SavingThrow, StringComparison.CurrentCultureIgnoreCase);
            if (!string.IsNullOrWhiteSpace(SpellResistance))
                match &= string.Equals(SpellResistance, spell.SpellResistance, StringComparison.CurrentCultureIgnoreCase);
            if (EffectTypes.Any())
                match &= spell.EffectTypes.Intersect(EffectTypes).Any();

            return match;
        }
        /// <summary>
        /// Resets the view model to a non-filtering state
        /// </summary>
        public override void Reset()
        {
            Name = string.Empty;
            School = string.Empty;
            SubSchool = string.Empty;
            Class = string.Empty;
            ClassLevel = string.Empty;
            CastingTime = string.Empty;
            Range = string.Empty;
            Area = string.Empty;
            Shapeable = null;
            SavingThrow = string.Empty;
            SpellResistance = string.Empty;
            EffectTypes.Clear();
        }
        #endregion
    }
}
