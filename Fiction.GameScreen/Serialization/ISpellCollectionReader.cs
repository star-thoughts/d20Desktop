using Fiction.GameScreen.Spells;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Serialization
{
    /// <summary>
    /// Base interface for a reader of spells
    /// </summary>
    public interface ISpellCollectionReader
    {
        /// <summary>
        /// Reads a collection of spells from the source
        /// </summary>
        /// <returns>Collection of spells read from the source</returns>
        Task<Spell[]> ReadSpells();
    }
}
