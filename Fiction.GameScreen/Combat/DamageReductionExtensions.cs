using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Extension methods for <see cref="DamageReduction"/>
    /// </summary>
    public static class DamageReductionExtensions
    {
        /// <summary>
        /// Creates a copy of Damage Reduction objects
        /// </summary>
        /// <param name="items">Damage reduction objects to copy</param>
        /// <returns>Observable collection of damage reduction objects</returns>
        public static ObservableCollection<DamageReduction> Copy(this IEnumerable<DamageReduction> items)
        {
            ObservableCollection<DamageReduction> result = new ObservableCollection<DamageReduction>();
            foreach (DamageReduction item in items)
            {
                result.Add(new DamageReduction(item.Amount, item.RequiresAllTypes, item.Types));
            }
            return result;
        }

        /// <summary>
        /// Copies a collection into this collection
        /// </summary>
        /// <param name="collection">Collection to copy into</param>
        /// <param name="items">Collection to copy from</param>
        public static void CopyFrom(this ObservableCollection<DamageReduction> collection, IEnumerable<DamageReduction> items)
        {
            collection.Clear();
            foreach (DamageReduction dr in items)
            {
                collection.Add(new DamageReduction(dr.Amount, dr.RequiresAllTypes, dr.Types));
            }
        }

        /// <summary>
        /// Creates a string representation of a collection of damage reduction
        /// </summary>
        /// <param name="items">Damage reduction objects to convert</param>
        /// <returns>Converted damage reduction</returns>
        public static string AsDamageReductionString(this IEnumerable<DamageReduction> items)
        {
            return string.Join(";", items.Select(p => p.ToString()));
        }

        /// <summary>
        /// Applies damage reduction modifiers to the given amount of damage
        /// </summary>
        /// <param name="items">Collection of damage reduction objects to apply</param>
        /// <param name="amount">Amount of damage to apply damage reduction to</param>
        /// <param name="types">Types of damage to apply</param>
        /// <returns></returns>
        public static int Apply(this IEnumerable<DamageReduction> items, int amount, params string[] types)
        {
            return items.Select(p => p.Apply(amount, types)).Min();
        }
        /// <summary>
        /// Gets the best match for damage reduction
        /// </summary>
        /// <param name="items">Damage reduction items</param>
        /// <param name="types">Damage types</param>
        /// <returns>Best match damage reduction</returns>
        public static DamageReduction GetDamageReduction(this IEnumerable<DamageReduction> items, params string[] types)
        {
            return items
                ?.Where(p => p.DoesApply(types))
                .OrderByDescending(p => p.Amount)
                .FirstOrDefault();
        }
    }
}
