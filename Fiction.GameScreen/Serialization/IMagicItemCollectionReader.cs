using Fiction.GameScreen.Equipment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Serialization
{
    /// <summary>
    /// Interface for reading a collection of magic items
    /// </summary>
    public interface IMagicItemCollectionReader
    {
        /// <summary>
        /// Reads a collection of magic items
        /// </summary>
        /// <returns>Collection of magic items</returns>
        Task<MagicItem[]> ReadMagicItems();
    }
}
