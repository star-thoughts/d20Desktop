using System;
using System.Collections.Generic;
using System.Text;

namespace Fiction.GameScreen
{
    /// <summary>
    /// Interface for managers that manage <see cref="ISourcedItem"/>s
    /// </summary>
    public interface ISourcedItemManager
    {
        /// <summary>
        /// Removes items from the given source
        /// </summary>
        /// <param name="source">Source to remove</param>
        void RemoveSource(string source);
        /// <summary>
        /// Gets all items of the given source
        /// </summary>
        /// <param name="source">Source to get items from</param>
        /// <returns>Collection of items from that source</returns>
        ISourcedItem[] GetItemsFromSource(string source);
    }
}
