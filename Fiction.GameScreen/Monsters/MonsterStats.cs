using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Fiction.GameScreen.Monsters
{
    /// <summary>
    /// Contains information about a monster with stats
    /// </summary>
    public sealed class MonsterStats : IMonsterStats, INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Cosntructs a new <see cref="MonsterStats"/>
        /// </summary>
        public MonsterStats()
        {
            _stats = new Dictionary<string, IMonsterStat>();
        }
        /// <summary>
        /// Constructs a new <see cref="MonsterStats"/> pre-filled with stats
        /// </summary>
        /// <param name="stats">Stats to prefill with</param>
        public MonsterStats(IEnumerable<IMonsterStat> stats)
        {
            _stats = stats.ToDictionary(p => p.Name, p => p);
        }
        #endregion
        #region Member Variables
        private Dictionary<string, IMonsterStat> _stats;
        #endregion
        #region Properties
        /// <summary>
        /// Gets the stat of the given name for the monster
        /// </summary>
        /// <param name="name">Name of the stat to get</param>
        /// <returns>Information about the statistic</returns>
        /// <remarks>
        /// This has the IndexerName attribute to ensure that the indexer is called "Item" so <see cref="PropertyChanged"/>
        /// events will work on "Item[]".
        /// </remarks>
        [IndexerName("Item")]
        public IMonsterStat? this[string name]
        {
            get
            {
                if (_stats.TryGetValue(name, out IMonsterStat? value))
                    return value;
                return null;
            }
        }
        /// <summary>
        /// Adds a given stat
        /// </summary>
        /// <param name="stat">Stat to add</param>
        public void AddStat(IMonsterStat stat)
        {
            _stats[stat.Name] = stat;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        }
        /// <summary>
        /// Adds the given stats
        /// </summary>
        /// <param name="stats">Stats to add</param>
        public void AddStats(params IMonsterStat[] stats)
        {
            foreach (IMonsterStat stat in stats)
                _stats[stat.Name] = stat;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        }
        /// <summary>
        /// Gets an enumerator for the stats
        /// </summary>
        /// <returns>Stat enumerator</returns>
        public IEnumerator<IMonsterStat> GetEnumerator()
        {
            return _stats.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
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
