using System;
using System.Collections.Generic;
using System.Text;

namespace Fiction.GameScreen
{
    /// <summary>
    /// Base interface for items that have a source
    /// </summary>
    public interface ISourcedItem
    {
        /// <summary>
        /// Gets the source of this item
        /// </summary>
        string Source { get; set; }
        /// <summary>
        /// Gets the source type for display purposes
        /// </summary>
        string SourceType { get; }
    }
}
