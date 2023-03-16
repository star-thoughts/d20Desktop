using Fiction.GameScreen.Monsters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Serialization
{
    /// <summary>
    /// Interface for getting a collection of monsters from an outside source
    /// </summary>
    public interface IMonsterCollectionReader
    {
        #region Methods
        /// <summary>
        /// Reads all of the monsters from the source
        /// </summary>
        /// <returns>Collection of monsters read from the source</returns>
        Task<Monster[]> ReadMonsters();
        #endregion
    }
}
