using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Fiction.GameScreen.Monsters
{
    /// <summary>
    /// Base interface for displaying the stats of a monster
    /// </summary>
    public interface IMonsterStats : IEnumerable<IMonsterStat>
    {
        /// <summary>
        /// Gets the attribute value associated with the name of the attribute
        /// </summary>
        /// <param name="name">Name of the attribute to retrieve</param>
        /// <returns>Value of the attribute</returns>
        IMonsterStat this[string name] { get; }
        /// <summary>
        /// Adds a given stat
        /// </summary>
        /// <param name="stat">Stat to add</param>
        void AddStat(IMonsterStat stat);
    }
}
