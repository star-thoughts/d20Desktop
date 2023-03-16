using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Fiction.GameScreen.Spells
{
    /// <summary>
    /// Extension methods for spell levels
    /// </summary>
    public static class SpellLevelExtensions
    {
        /// <summary>
        /// Gets a string representation all of the levels of the spell for serialization
        /// </summary>
        /// <param name="levels">Levels to write to string</param>
        /// <returns>String to use for serialization</returns>
        public static string GetInvariantSpellLevelString(this IEnumerable<SpellLevel> levels)
        {
            return string.Join(" ", levels.Select(p => String.Format("{0} {1}", p.Class, p.Level)));
        }

        /// <summary>
        /// Parses a string stored with <see cref="GetInvariantSpellLevelString(IEnumerable{SpellLevel})"/> and turns it back into a set of classes/levels
        /// </summary>
        /// <param name="levels">Level string to parse</param>
        /// <returns>Spell levels</returns>
        public static IEnumerable<SpellLevel> GetLevelsFromInvariantSpellLevelString(string levels)
        {
            Regex regex = new Regex(@"(?<class>[a-zA-Z]+):? (?<level>[0-9]+)");
            MatchCollection matches = regex.Matches(levels);
            foreach (Match match in matches)
            {
                if (match.Groups["class"].Success && match.Groups["level"].Success)
                {
                    if (Int32.TryParse(match.Groups["level"].Value, System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out int level))
                        yield return new SpellLevel() { Class = match.Groups["class"].Value, Level = level };
                }
            }
        }
    }
}
