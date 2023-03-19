using Fiction;
using System.Globalization;

namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns whether or not this string is null or empty
        /// </summary>
        /// <param name="s">String to test</param>
        /// <returns>Whether or not this string is null or empty</returns>
        public static bool IsNullOrEmpty(this string? s)
        {
            return String.IsNullOrEmpty(s);
        }
        /// <summary>
        /// Returns whether or not this string is null or Whitespace
        /// </summary>
        /// <param name="s">String to test</param>
        /// <returns>Whether or not this string is null or Whitespace</returns>
        public static bool IsNullOrWhiteSpace(this string? s)
        {
            return String.IsNullOrWhiteSpace(s);
        }
        /// <summary>
        /// Gets the levenshtein distance between two strings
        /// </summary>
        /// <param name="source">Source string to compare with</param>
        /// <param name="from">String to compare to</param>
        /// <returns>Distance between two strings</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body")]
        public static int DistanceFrom(this string source, string from)
        {
            Exceptions.ThrowIfArgumentNullOrEmpty(source, nameof(source));
            Exceptions.ThrowIfArgumentNullOrEmpty(from, nameof(from));

            // Levenshtein Algorithm Revisited - WebReflection
            if (source == from)
                return 0;
            if (source.Length == 0 || from.Length == 0)
                return source.Length == 0 ? from.Length : source.Length;
            int len1 = source.Length + 1,
                len2 = from.Length + 1,
                I = 0,
                i = 0,
                c, j, J;
            int[,] d = new int[len1, len2];
            while (i < len2)
                d[0, i] = i++;
            i = 0;
            while (++i < len1)
            {
                J = j = 0;
                c = source[I];
                d[i, 0] = i;
                while (++j < len2)
                {
                    d[i, j] = Math.Min(Math.Min(d[I, j] + 1, d[i, J] + 1), d[I, J] + (c == from[J] ? 0 : 1));
                    ++J;
                };
                ++I;
            };
            return d[len1 - 1, len2 - 1];
        }

        /// <summary>
        /// Splits the string using the given string split characters
        /// </summary>
        /// <param name="s">String to split</param>
        /// <param name="options">Splitting options</param>
        /// <param name="splitStrings">Characters to use to split the string</param>
        /// <returns>Array of strings split up from the original string</returns>
        public static string[] Split(this string s, StringSplitOptions options, params char[] splitChars)
        {
            Exceptions.ThrowIfArgumentNull(s, nameof(s));

            return s.Split(splitChars, options);
        }
        /// <summary>
        /// Splits the string using the given string split characters
        /// </summary>
        /// <param name="s">String to split</param>
        /// <param name="options">Splitting options</param>
        /// <param name="splitStrings">Strings to use to split the string</param>
        /// <returns>Array of strings split up from the original string</returns>
        public static string[] Split(this string s, StringSplitOptions options, params string[] splitStrings)
        {
            Exceptions.ThrowIfArgumentNull(s, nameof(s));

            return s.Split(splitStrings, options);
        }
        /// <summary>
        /// Determinse whether the string contains an integer
        /// </summary>
        /// <param name="s">String to test</param>
        /// <returns>Whether or not the string contains an integer</returns>
        public static bool IsInteger(this string s)
        {
            return Int32.TryParse(s, System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out int result);
        }
        /// <summary>
        /// Determinse whether the string contains an integer
        /// </summary>
        /// <param name="s">String to test</param>
        /// <param name="culture">Culture to use for parsing</param>
        /// <returns>Whether or not the string contains an integer</returns>
        public static bool IsInteger(this string s, CultureInfo culture)
        {
            return Int32.TryParse(s, System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out int result);
        }
        /// <summary>
        /// Given a string, removes from the beginning and end another substring
        /// </summary>
        /// <param name="s">String to trim</param>
        /// <param name="trimText">Text to trim from the beginning</param>
        /// <param name="comparison">Comparison to use for finding text</param>
        /// <returns>Trimmed text</returns>
        public static string Trim(this string s, string trimText, StringComparison comparison = StringComparison.CurrentCulture)
        {
            if (string.Equals(s, trimText, comparison))
                return string.Empty;

            if (s.StartsWith(trimText, comparison))
                s = s.Substring(trimText.Length);

            if (string.Equals(s, trimText, comparison))
                return string.Empty;

            if (s.EndsWith(trimText, comparison))
                s = s.Substring(0, s.Length - trimText.Length);

            return s;
        }
        /// <summary>
        /// Given a string, removes from the beginning another substring
        /// </summary>
        /// <param name="s">String to trim</param>
        /// <param name="trimText">Text to trim from the beginning</param>
        /// <param name="comparison">Comparison to use for finding text</param>
        /// <returns>Trimmed text</returns>
        public static string TrimStart(this string s, string trimText, StringComparison comparison = StringComparison.CurrentCulture)
        {
            if (string.Equals(s, trimText, comparison))
                return string.Empty;

            if (s.StartsWith(trimText, comparison))
                s = s.Substring(trimText.Length);

            return s;
        }
        /// <summary>
        /// Given a string, removes from the end another substring
        /// </summary>
        /// <param name="s">String to trim</param>
        /// <param name="trimText">Text to trim from the beginning</param>
        /// <param name="comparison">Comparison to use for finding text</param>
        /// <returns>Trimmed text</returns>
        public static string TrimEnd(this string s, string trimText, StringComparison comparison)
        {
            if (string.Equals(s, trimText, comparison))
                return string.Empty;

            if (s.EndsWith(trimText, comparison))
                s = s.Substring(0, s.Length - trimText.Length);

            return s;
        }
    }
}
