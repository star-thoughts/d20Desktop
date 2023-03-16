using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen
{
    /// <summary>
    /// Strategy to use when rolling dice
    /// </summary>
    public enum RollingStrategy
    {
        /// <summary>
        /// Roll standard dice
        /// </summary>
        Standard = 0,
        /// <summary>
        /// Get the minimum value possible
        /// </summary>
        Minimum = 1,
        /// <summary>
        /// Get the maximum value possible
        /// </summary>
        Maximum = 2,
        /// <summary>
        /// Reroll any rolls in the upper 33%
        /// </summary>
        BelowAverage = 3,
        /// <summary>
        /// Reroll any rolls in the lower 33%
        /// </summary>
        AboveAverage = 4,
        /// <summary>
        /// Reroll any rolls below 50%
        /// </summary>
        Heroic = 5,
    }
}
