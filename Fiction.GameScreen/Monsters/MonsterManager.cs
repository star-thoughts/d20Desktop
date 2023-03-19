using System.Collections.ObjectModel;

namespace Fiction.GameScreen.Monsters
{
    /// <summary>
    /// Manages a collection of monsters in a campaign
    /// </summary>
    public sealed class MonsterManager : ISourcedItemManager
    {
        #region Constructors
        /// <summary>
        /// Constructs a new empty <see cref="MonsterManager"/>
        /// </summary>
        public MonsterManager()
        {
            Monsters = new ObservableCollection<Monster>();
            Types = new ObservableCollection<string>();
            SubTypes = new ObservableCollection<string>();
            Groups = new ObservableCollection<string>();
        }
        /// <summary>
        /// Constructs a new <see cref="MonsterManager"/>
        /// </summary>
        /// <param name="monsters">Monsters to pre-populate with</param>
        public MonsterManager(IEnumerable<Monster> monsters)
        {
            Monsters = monsters.ToObservableCollection();
            Types = new ObservableCollection<string>();
            SubTypes = new ObservableCollection<string>();
            Groups = new ObservableCollection<string>();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a collection of monsters
        /// </summary>
        public ObservableCollection<Monster> Monsters { get; private set; }
        /// <summary>
        /// Gets a collection of monster types
        /// </summary>
        public ObservableCollection<string> Types { get; private set; }
        /// <summary>
        /// Gets a collection of monster subtypes
        /// </summary>
        public ObservableCollection<string> SubTypes { get; private set; }
        /// <summary>
        /// Gets a collection of monster groups
        /// </summary>
        public ObservableCollection<string> Groups { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Assigns <see cref="Types"/> and <see cref="SubTypes"/> based on existing monsters
        /// </summary>
        public void Reconcile()
        {
            Types.Clear();
            foreach (string? type in Monsters
                .Select(p => p.Stats["type"]?.Value as string)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToObservableCollection())
            {
                if (type != null)
                    Types.Add(type);
            }

            SubTypes.Clear();
            foreach (string? type in Monsters
                .SelectMany(p => p.Stats["subType"]?.Value as IEnumerable<string> ?? Array.Empty<string>())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToObservableCollection())
            {
                if (type != null)
                    SubTypes.Add(type);
            }

            Groups.Clear();
            foreach (string? group in Monsters
                .Select(p => p.Stats["group"]?.Value as string)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct()
                .ToObservableCollection())
            {
                if (group != null)
                    Groups.Add(group);
            }
        }

        /// <summary>
        /// Removes the given subtype from all monsters
        /// </summary>
        /// <param name="subType">Subtype to remove</param>
        public void RemoveSubType(string subType)
        {
            foreach (Monster monster in Monsters)
            {
                IList<string>? stat = monster.Stats["subType"]?.Value as IList<string>;
                stat?.Remove(subType);
            }
        }
        /// <summary>
        /// Removes the given type from all monsters
        /// </summary>
        /// <param name="type">Type to remove</param>
        public void RemoveType(string type)
        {
            foreach (Monster monster in Monsters)
            {
                if (monster.Stats["type"] is MonsterStat stat)
                {
                    if (string.Equals(stat.Value as string, type, StringComparison.CurrentCultureIgnoreCase))
                        stat.Value = string.Empty;
                }
            }
        }

        /// <summary>
        /// Removes all monsters of the given source
        /// </summary>
        void ISourcedItemManager.RemoveSource(string source)
        {
            Exceptions.ThrowIfArgumentNull(source, nameof(source));

            Monster[] monsters = GetMonstersFromSource(source);

            foreach (Monster monster in monsters)
                Monsters.Remove(monster);
        }

        private Monster[] GetMonstersFromSource(string source)
        {
            return ((ISourcedItemManager)this).GetItemsFromSource(source)
                .OfType<Monster>()
                .ToArray();
        }

        ISourcedItem[] ISourcedItemManager.GetItemsFromSource(string source)
        {
            return Monsters
                .Where(p => string.Equals(source, p.Source, StringComparison.CurrentCultureIgnoreCase))
                .ToArray();
        }
        #endregion
    }
}
