using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fiction.GameScreen
{
    /// <summary>
    /// Helpers to parse damage done
    /// </summary>
    public static class DamageParser
    {
        /// <summary>
        /// Attempts to parse a damage string and returns the result
        /// </summary>
        /// <param name="damageString">String dealing damage</param>
        /// <param name="damageReduction">Amount of damage reduction to apply to each hit</param>
        /// <param name="applyDamageReductionToEach">If true, applies damage reduction to each number, otherwise applies it to the total</param>
        /// <param name="amount">Amount of damage dealt</param>
        /// <returns>Whether or not the string was successfully parsed</returns>
        /// <remarks>
        /// <para>
        /// This parses a string containing one or more numbers, separated by a plus or minus.  Damage reduction
        /// is applied separatly to each hit, unless <paramref name="applyDamageReductionToEach"/> is false.  If a number
        /// is surrounded by curley braces {}, then it damage reduction is ignored for that number.
        /// </para>
        /// <para>
        /// Will never return a number less than 0.
        /// </para>
        /// </remarks>
        public static bool TryParse(string damageString, int damageReduction, bool applyDamageReductionToEach, out int amount)
        {
            amount = 0;

            if (damageString == null)
                return true;

            int total = 0;
            int eachDamageReduction = applyDamageReductionToEach ? damageReduction : 0;
            bool ignoreDamageReduction = false;
            int item = 0;

            for (int i = 0; i < damageString.Length; i++)
            {
                char c = damageString[i];

                if (c == '+')
                {
                    total += ApplyDamageReduction(ref item, damageReduction, applyDamageReductionToEach, ignoreDamageReduction);
                }
                else if (c == '}')
                {
                    //  We didn't see an open brace {
                    if (!ignoreDamageReduction)
                        return false;
                    total += ApplyDamageReduction(ref item, damageReduction, applyDamageReductionToEach, ignoreDamageReduction);
                    ignoreDamageReduction = false;
                }
                else if (c == '{')
                    ignoreDamageReduction = true;
                //  See if we had a closing brace without an opening brace;
                //  Figure out the item's amount
                else if (char.IsDigit(c))
                    item = item * 10 + Convert.ToInt32(char.GetNumericValue(c));
                else
                    return false;
            }
            //  This means we had an {, but never saw a }
            if (ignoreDamageReduction)
                return false;

            total += ApplyDamageReduction(ref item, damageReduction, applyDamageReductionToEach, ignoreDamageReduction);

            amount = total;

            if (!applyDamageReductionToEach)
                amount -= damageReduction;

            //  Must do at least 0 damage
            amount = Math.Max(0, amount);

            return true;
        }

        private static int ApplyDamageReduction(ref int amount, int damageReduction, bool applyDamageReductionToEach, bool ignoreDamageReduction)
        {
            int item = amount;
            amount = 0;
            
            if (applyDamageReductionToEach && !ignoreDamageReduction)
                item -= damageReduction;
            item = Math.Max(0, item);

            return item;
        }
    }
}
