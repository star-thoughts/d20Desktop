using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Extension methods for <see cref="RollingStrategy"/>
    /// </summary>
    public static class RollingStrategyExtensions
    {
        /// <summary>
        /// Gets a display string for a given rolling strategy
        /// </summary>
        /// <param name="strategy">Rolling strategy to get the display string for</param>
        /// <returns>Display string for this strategy</returns>
        public static string ToDisplayString(this RollingStrategy strategy)
        {
            switch (strategy)
            {
                case RollingStrategy.AboveAverage:
                    return Resources.Resources.RollingStrategyAboveAverage;
                case RollingStrategy.BelowAverage:
                    return Resources.Resources.RollingStrategyBelowAverage;
                case RollingStrategy.Heroic:
                    return Resources.Resources.RollingStrategyHeroic;
                case RollingStrategy.Maximum:
                    return Resources.Resources.RollingStrategyMaximum;
                case RollingStrategy.Minimum:
                    return Resources.Resources.RollingStrategyMinimum;
                case RollingStrategy.Standard:
                    return Resources.Resources.RollingStrategyStandard;
                default:
                    throw new InvalidOperationException("Unknown RollingStrategy.");
            }
        }

        private static IEnumerable _itemsSource;
        /// <summary>
        /// Gets a collection of items for a ItemsControl
        /// </summary>
        /// <returns>Collection of objects with a Display/Value for an ItemsControl's ItemsSource</returns>
        public static IEnumerable ItemsSource
        {
            get
            {
                if (_itemsSource == null)
                    _itemsSource = Enum.GetValues(typeof(RollingStrategy))
                        .OfType<RollingStrategy>()
                        .Select(p => new { Display = p.ToDisplayString(), Value = p })
                        .ToArray();

                return _itemsSource;
            }
        }
    }
}
