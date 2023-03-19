using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiction.GameScreen.Monsters;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Extension methods for <see cref="Alignment"/>
    /// </summary>
    public static class AlignmentExtensions
    {
        /// <summary>
        /// Gets a display string for the given alignment
        /// </summary>
        /// <param name="alignment">Alignment to get display string for</param>
        /// <returns>String to use for display purposes</returns>
        public static string ToDisplayString(this Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Unknown:
                    return Resources.Resources.UnknownLabel;
                case Alignment.LawfulGood:
                    return Resources.Resources.LawfulGoodAlignment;
                case Alignment.NeutralGood:
                    return Resources.Resources.NeutralGoodAlignment;
                case Alignment.ChaoticGood:
                    return Resources.Resources.ChaoticGoodAlignment;
                case Alignment.LawfulNeutral:
                    return Resources.Resources.LawfulNeutralAlignment;
                case Alignment.TrueNeutral:
                    return Resources.Resources.NeutralNeutralAlignment;
                case Alignment.ChaoticNeutral:
                    return Resources.Resources.ChaoticNeutralAlignment;
                case Alignment.LawfulEvil:
                    return Resources.Resources.LawfulEvilAlignment;
                case Alignment.NeutralEvil:
                    return Resources.Resources.NeutralEvilAlignment;
                case Alignment.ChaoticEvil:
                    return Resources.Resources.ChaoticEvilAlignment;
                default:
                    throw new ArgumentException("Unknown alignment type of " + alignment.ToString(), nameof(alignment));
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
                    Alignment[] alignments = new Alignment[]
                    {
                        Alignment.Unknown, Alignment.LawfulGood, Alignment.NeutralGood, Alignment.ChaoticGood,  Alignment.LawfulNeutral,  Alignment.TrueNeutral, Alignment.ChaoticNeutral, Alignment.LawfulEvil, Alignment.NeutralEvil, Alignment.ChaoticEvil,
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
