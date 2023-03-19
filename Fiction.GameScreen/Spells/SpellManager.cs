using System.Collections.ObjectModel;

namespace Fiction.GameScreen.Spells
{
    /// <summary>
    /// Manages spells in a campaign
    /// </summary>
    public sealed class SpellManager : ISourcedItemManager
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SpellManager"/>
        /// </summary>
        public SpellManager()
        {
            Spells = new ObservableCollection<Spell>();
            _spellEffectTypes = new ObservableCollection<string>();
            _schools = new ObservableCollection<string>();
            _subSchools = new ObservableCollection<string>();

            SpellEffectTypes = new ReadOnlyObservableCollection<string>(_spellEffectTypes);
            Schools = new ReadOnlyObservableCollection<string>(_schools);
            SubSchools = new ReadOnlyObservableCollection<string>(_subSchools);
        }
        #endregion
        #region Member Variables
        private ObservableCollection<string> _spellEffectTypes;
        private ObservableCollection<string> _schools;
        private ObservableCollection<string> _subSchools;
        #endregion
        #region Properties
        /// <summary>
        /// Gets a collection of spells in the campaign
        /// </summary>
        public ObservableCollection<Spell> Spells { get; private set; }
        /// <summary>
        /// Gets a global collection of effect types in spells
        /// </summary>
        public ReadOnlyObservableCollection<string> SpellEffectTypes { get; private set; }
        /// <summary>
        /// Gets a global collection of schools in use
        /// </summary>
        public ReadOnlyObservableCollection<string> Schools { get; private set; }
        /// <summary>
        /// Gets a global collection of SubSchools in use
        /// </summary>
        public ReadOnlyObservableCollection<string> SubSchools { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// <summary>
        /// Resets the list of <see cref="_spellEffectTypes"/> to all of the types in the spells
        /// </summary>
        public void ReconcileTypes()
        {
            _spellEffectTypes.Clear();
            foreach (string type in Spells.SelectMany(p => p.EffectTypes).Distinct(StringComparer.CurrentCultureIgnoreCase))
                _spellEffectTypes.Add(type);
        }
        /// <summary>
        /// Resets the list of <see cref="_schools"/> to all of the schools in the spells
        /// </summary>
        public void ReconcileSchools()
        {
            _schools.Clear();
            foreach (string school in Spells.Select(p => p.School).Distinct(StringComparer.CurrentCultureIgnoreCase).OfType<string>())
                _schools.Add(school);
        }
        /// <summary>
        /// Resets the list of <see cref="_subSchools"/> to all of the subschools in the spells
        /// </summary>
        public void ReconcileSubSchools()
        {
            _subSchools.Clear();
            foreach (string subSchool in Spells.Select(p => p.SubSchool).Distinct(StringComparer.CurrentCultureIgnoreCase).OfType<string>())
                _subSchools.Add(subSchool);
        }
        /// <summary>
        /// Reconciles all of the lists
        /// </summary>
        public void Reconcile()
        {
            ReconcileTypes();
            ReconcileSchools();
            ReconcileSubSchools();
        }
        /// Adds the given school to the spell manager
        /// </summary>
        /// <param name="subschool">School to add</param>
        public void AddSchool(string school)
        {
            Exceptions.ThrowIfArgumentNull(school, nameof(school));

            if (!_schools.Contains(school, StringComparer.CurrentCultureIgnoreCase))
                _schools.Add(school);
        }
        /// <summary>
        /// Adds the given school to the spell manager
        /// </summary>
        /// <param name="subschool">School to add</param>
        public void AddSubSchool(string subschool)
        {
            Exceptions.ThrowIfArgumentNull(subschool, nameof(subschool));

            if (!_subSchools.Contains(subschool, StringComparer.CurrentCultureIgnoreCase))
                _subSchools.Add(subschool);
        }
        /// <summary>
        /// Adds the given type to the spell manager
        /// </summary>
        /// <param name="effectType">Type to add</param>
        public void AddEffectType(string effectType)
        {
            Exceptions.ThrowIfArgumentNull(effectType, nameof(effectType));

            if (!_spellEffectTypes.Contains(effectType, StringComparer.CurrentCultureIgnoreCase))
                _spellEffectTypes.Add(effectType);
        }
        /// <summary>
        /// Removes the given school from the spell manager
        /// </summary>
        /// <param name="school">School to remove</param>
        public void RemoveSchool(string school)
        {
            Exceptions.ThrowIfArgumentNullOrEmpty(school, nameof(school));

            foreach (Spell spell in Spells)
            {
                if (string.Equals(spell.School, school, StringComparison.CurrentCultureIgnoreCase))
                    spell.School = string.Empty;
            }
        }
        /// <summary>
        /// Removes the given subschool from the spell manager
        /// </summary>
        /// <param name="subSchool">Subschool to remove</param>
        public void RemoveSubSchool(string subSchool)
        {
            Exceptions.ThrowIfArgumentNullOrEmpty(subSchool, nameof(subSchool));

            foreach (Spell spell in Spells)
            {
                if (string.Equals(spell.SubSchool, subSchool, StringComparison.CurrentCultureIgnoreCase))
                    spell.SubSchool = string.Empty;
            }
        }
        /// <summary>
        /// Removes the given effect type from the spell manager
        /// </summary>
        /// <param name="effectType">Effect type to remove</param>
        public void RemoveEffectType(string effectType)
        {
            Exceptions.ThrowIfArgumentNullOrEmpty(effectType, nameof(effectType));

            foreach (Spell spell in Spells)
            {
                string? type = spell.EffectTypes.FirstOrDefault(p => string.Equals(p, effectType, StringComparison.CurrentCultureIgnoreCase));

                if (!string.IsNullOrEmpty(type))
                    spell.EffectTypes.Remove(type);
            }
        }
        /// <summary>
        /// Replaces <paramref name="oldSchool"/> with <paramref name="newSchool"/>
        /// </summary>
        /// <param name="oldSchool">The school to replace</param>
        /// <param name="newSchool">The school to replace with</param>
        public void ReplaceSchool(string oldSchool, string newSchool)
        {
            Exceptions.ThrowIfArgumentNullOrEmpty(oldSchool, nameof(oldSchool));
            Exceptions.ThrowIfArgumentNullOrEmpty(newSchool, nameof(newSchool));

            foreach (Spell spell in Spells)
            {
                if (string.Equals(spell.School, oldSchool, StringComparison.CurrentCultureIgnoreCase))
                    spell.School = newSchool;
            }

            _schools.Remove(oldSchool);
            _schools.Add(newSchool);
        }
        /// <summary>
        /// Replaces <paramref name="oldSubSchool"/> with <paramref name="newSubSchool"/>
        /// </summary>
        /// <param name="oldSubSchool">The school to replace</param>
        /// <param name="newSubSchool">The school to replace with</param>
        public void ReplaceSubSchool(string oldSubSchool, string newSubSchool)
        {
            Exceptions.ThrowIfArgumentNullOrEmpty(oldSubSchool, nameof(oldSubSchool));
            Exceptions.ThrowIfArgumentNullOrEmpty(newSubSchool, nameof(newSubSchool));

            foreach (Spell spell in Spells)
            {
                if (string.Equals(spell.SubSchool, oldSubSchool, StringComparison.CurrentCultureIgnoreCase))
                    spell.SubSchool = newSubSchool;
            }

            _subSchools.Remove(oldSubSchool);
            _subSchools.Add(newSubSchool);
        }
        /// <summary>
        /// Replaces <paramref name="oldType"/> with <paramref name="newType"/>
        /// </summary>
        /// <param name="oldType">The type to replace</param>
        /// <param name="newType">The type to replace with</param>
        public void ReplaceEffectType(string oldType, string newType)
        {
            Exceptions.ThrowIfArgumentNullOrEmpty(oldType, nameof(oldType));
            Exceptions.ThrowIfArgumentNullOrEmpty(newType, nameof(newType));

            foreach (Spell spell in Spells)
            {
                string? type = spell.EffectTypes.FirstOrDefault(p => string.Equals(p, oldType, StringComparison.CurrentCultureIgnoreCase));
                if (!string.IsNullOrEmpty(type))
                {
                    spell.EffectTypes.Remove(type);
                    spell.EffectTypes.Add(newType);
                }
            }

            _spellEffectTypes.Remove(oldType);
            _spellEffectTypes.Add(newType);
        }
        /// <summary>
        /// Removes all items from the given source
        /// </summary>
        /// <param name="source">Source of items to remove</param>
        void ISourcedItemManager.RemoveSource(string source)
        {
            Exceptions.ThrowIfArgumentNull(source, nameof(source));

            Spell[] spells = GetSpellsFromSource(source);

            foreach (Spell spell in spells)
                Spells.Remove(spell);
        }

        private Spell[] GetSpellsFromSource(string source)
        {
            return ((ISourcedItemManager)this).GetItemsFromSource(source)
                .Cast<Spell>()
                .ToArray();
        }

        /// <summary>
        /// Gets all spells of the given source
        /// </summary>
        /// <param name="source">Source to get spells from</param>
        /// <returns>Spells from the source</returns>
        ISourcedItem[] ISourcedItemManager.GetItemsFromSource(string source)
        {
            Exceptions.ThrowIfArgumentNull(source, nameof(source));

            return Spells
                .Where(p => string.Equals(source, p.Source, StringComparison.CurrentCultureIgnoreCase))
                .ToArray();
        }
        #endregion
    }
}
