using System;
using System.Collections.Generic;
using System.Text;

namespace Fiction.GameScreen
{
    /// <summary>
    /// Interface for an item that can be filtered
    /// </summary>
    public interface IFilterable
    {
        /// <summary>
        /// Gets whether or not the item matches the filter text
        /// </summary>
        /// <param name="filterText">Text to match against</param>
        /// <returns>Whether or not to display the item</returns>
        bool CanDisplay(string filterText);
    }
}
