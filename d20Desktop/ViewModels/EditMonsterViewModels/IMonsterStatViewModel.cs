using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels.EditMonsterViewModels
{
    /// <summary>
    /// Base interface for monster stats
    /// </summary>
    public interface IMonsterStatViewModel
    {
        /// <summary>
        /// Gets the name for storage
        /// </summary>
        string StatName { get; }
        /// <summary>
        /// Gets the name of the stat
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets the category for the stat
        /// </summary>
        string Category { get; }
        /// <summary>
        /// Gets the value of the stat
        /// </summary>
        object Value { get; }
    }
}
