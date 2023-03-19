using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Fiction.GameScreen
{
    /// <summary>
    /// Class for rolling dice
    /// </summary>
    public static class Dice
    {
        private static Random _random = new Random();

        /// <summary>
        /// Determinse if the given string is a valid die string
        /// </summary>
        /// <param name="dieString">String to test</param>
        /// <returns>Whether or not the string is valid</returns>
        public static bool IsValidString([NotNullWhen(true)] string? dieString)
        {
            if (string.IsNullOrWhiteSpace(dieString))
                return false;

            StringBuilder sb = new StringBuilder(dieString.TrimStart('+'));
            sb.Replace("(", "").Replace(")", "").Replace("{", "").Replace("}", "");

            string[] parts = sb.ToString().Split(StringSplitOptions.RemoveEmptyEntries, '+', '-');
            return parts.All(p => IsValidPart(p));
        }

        private static bool IsValidPart(string dieString)
        {
            return dieString.IsInteger() || dieString.Split(StringSplitOptions.RemoveEmptyEntries, 'd', 'D').All(p => p.IsInteger());
        }

        /// <summary>
        /// Rolls dice using a string for the source
        /// </summary>
        /// <param name="dieString">Dice string source</param>
        /// <returns>Total roll</returns>
        public static int Roll(string dieString)
        {
            return Roll(dieString, RollingStrategy.Standard);
        }

        private static string[] GetDieStringParts(string dieString)
        {
            StringBuilder sb = new StringBuilder(dieString);
            sb.Replace("(", "").Replace(")", "").Replace("{", "").Replace("}", "");

            string[] parts = sb.ToString().Split(StringSplitOptions.RemoveEmptyEntries, '+', '-');
            return parts;
        }

        /// <summary>
        /// Rolls dice using ta string for the source
        /// </summary>
        /// <param name="dieString">Dice string source</param>
        /// <param name="strategy">Strategy to use for rolling</param>
        /// <returns>Total roll</returns>
        public static int Roll(string dieString, RollingStrategy strategy)
        {
            string[] parts = GetDieStringParts(dieString);

            int total = 0;
            for (int i = 0; i < parts.Length; i++)
                total += RollOrAdd(parts[i], strategy);
            return total;
        }
        private static int RollOrAdd(string part, RollingStrategy strategy)
        {
            if (!Int32.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
            {
                string[] numbers = part.Split(StringSplitOptions.RemoveEmptyEntries, 'd', 'D');
                if (numbers.Length != 2)
                    throw new ArgumentException("Unable to parse die string", part);
                if (!Int32.TryParse(numbers[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int times))
                    throw new ArgumentException("Unable to parse die string", part);
                if (!Int32.TryParse(numbers[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int die))
                    throw new ArgumentException("Unable to parse die string", part);

                return Roll(times, die, strategy);
            }
            return result;
        }

        /// <summary>
        /// Rolls a given die the given number of times
        /// </summary>
        /// <param name="times">Times to roll the die</param>
        /// <param name="die">Die to roll</param>
        /// <param name="strategy">Strategy to use for rolling</param>
        /// <returns>Total result</returns>
        public static int Roll(int times, int die, RollingStrategy strategy)
        {
            int total = 0;
            for (int i = 0; i < times; i++)
                total += Roll(die, strategy);
            return total;
        }/// <summary>
         /// Rolls a given die the given number of times
         /// </summary>
         /// <param name="times">Times to roll the die</param>
         /// <param name="die">Die to roll</param>
         /// <returns>Total result</returns>
        public static int Roll(int times, int die)
        {
            return Roll(times, die, RollingStrategy.Standard);
        }

        private static int Roll(int die, RollingStrategy strategy)
        {
            int result = 0;
            //  _random.Next's upper bound is exclusive, so add one here instead of on every line that uses it
            die = die + 1;

            switch (strategy)
            {
                case RollingStrategy.Standard:
                    result = _random.Next(1, die);
                    break;
                case RollingStrategy.AboveAverage:
                    while (result <= die / 3)
                        result = _random.Next(1, die);
                    break;
                case RollingStrategy.BelowAverage:
                    do
                        result = _random.Next(1, die);
                    while (result >= die - (die / 3));
                    break;
                case RollingStrategy.Heroic:
                    while (result <= die - (die / 3))
                        result = _random.Next(1, die);
                    break;
                case RollingStrategy.Maximum:
                    result = die - 1;
                    break;
                case RollingStrategy.Minimum:
                    result = 1;
                    break;
                default:
                    throw new ArgumentException("Unknown rolling strategy", "strategy");
            }

            return result;
        }
    }
}
