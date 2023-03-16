using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Fiction.GameScreen
{
    /// <summary>
    /// Extension methods for <see cref="IFilterable"/>
    /// </summary>
    public static class IFilterableExtensions
    {
        /// <summary>
        /// Determines if the given string matches the filter text
        /// </summary>
        /// <param name="filterText">Filter text to test against</param>
        /// <param name="itemToTest">String to test against</param>
        internal static bool MatchesFilter(this IFilterable item, string filterText, string itemToTest)
        {
            return MatchesFilter(filterText, itemToTest);
        }
        /// <summary>
        /// Determines if the given string matches the filter text
        /// </summary>
        /// <param name="filterText">Filter text to test against</param>
        /// <param name="itemToTest">String to test against</param>
        public static bool MatchesFilter(string filterText, string itemToTest)
        {
            return (!string.IsNullOrEmpty(itemToTest) && CultureInfo.CurrentCulture.CompareInfo.IndexOf(itemToTest, filterText, CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) >= 0)
                    || itemToTest.ToUpperInvariant().DistanceFrom(filterText.ToUpperInvariant()) < 2;
        }
    }
}
