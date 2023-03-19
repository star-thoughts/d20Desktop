using Fiction.GameScreen.Equipment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Extension methods for <see cref="ArtifactLevel"/>
    /// </summary>
    public static class ArtifactLevelExtensions
    {
        /// <summary>
        /// Gets a display string for the given alignment
        /// </summary>
        /// <param name="level">Alignment to get display string for</param>
        /// <returns>String to use for display purposes</returns>
        public static string ToDisplayString(this ArtifactLevel level)
        {
            switch (level)
            {
                case ArtifactLevel.None:
                    return Resources.Resources.ArtifactLevelNoneLabel;
                case ArtifactLevel.Minor:
                    return Resources.Resources.ArtifactLevelMinorLabel;
                case ArtifactLevel.Major:
                    return Resources.Resources.ArtifactLevelMajorLabel;
                default:
                    return Resources.Resources.UnknownLabel;
            }
        }

        private static IEnumerable? _itemsSource;
        /// <summary>
        /// Gets an items source for a selector
        /// </summary>
        public static IEnumerable? ItemsSource
        {
            get
            {
                if (_itemsSource == null)
                {
                    //  Make sure they're in the order we want
                    ArtifactLevel[] alignments = new ArtifactLevel[]
                    {
                        ArtifactLevel.None, ArtifactLevel.Minor, ArtifactLevel.Major,
                    };
                    _itemsSource = alignments
                        .Select(p => new { Display = p.ToDisplayString(), Value = p })
                        .ToArray();
                }

                return _itemsSource;
            }
        }
    }
}
