using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Extension methods for the <see cref="IInitiativeCollection"/> class
    /// </summary>
    public static class IInitiativeCollectionExtensions
    {
        /// <summary>
        /// Gets the combatant at the given initiative order
        /// </summary>
        /// <typeparam name="T">Type of combatant expected</typeparam>
        /// <param name="collection">Collection of combatants</param>
        /// <param name="initOrder">Initiative order to get</param>
        /// <returns>Combatant at the given initiative order, or null if no combatant at that initiative order</returns>
        public static T? CombatantAtInitiative<T>(this IInitiativeCollection collection, int initOrder) where T : IActiveCombatant
        {
            return collection.Combatants.OfType<T>().FirstOrDefault(p => p.InitiativeOrder == initOrder);
        }
        /// <summary>
        /// Gets the combatant that comes after the combatant given by <paramref name="combatant"/>
        /// </summary>
        /// <typeparam name="T">Type of combatant expected</typeparam>
        /// <param name="collection">Collection of combatants</param>
        /// <param name="combatant">Source combatant</param>
        /// <returns>Combatant after the given combatant, or null if none available</returns>
        public static T? CombatantAfter<T>(this IInitiativeCollection collection, T combatant) where T : IActiveCombatant
        {
            return collection.CombatantAtInitiative<T>(combatant.InitiativeOrder + 1);
        }
        /// <summary>
        /// Gets the combatant that comes before the combatant given by <paramref name="combatant"/>
        /// </summary>
        /// <typeparam name="T">Type of combatant expected</typeparam>
        /// <param name="collection">Collection of combatants</param>
        /// <param name="combatant">Source combatant</param>
        /// <returns>Combatant before the given combatant, or null if none available</returns>
        public static T? CombatantBefore<T>(this IInitiativeCollection collection, T combatant) where T : IActiveCombatant
        {
            return collection.CombatantAtInitiative<T>(combatant.InitiativeOrder - 1);
        }

        /// <summary>
        /// Swaps the position of two combatants in initiative
        /// </summary>
        /// <param name="combatant1">Combatant 1</param>
        /// <param name="combatant2">Combatant 2</param>
        public static void Swap(this IInitiativeCollection collection, IActiveCombatant combatant1, IActiveCombatant combatant2)
        {
            Exceptions.ThrowIfArgumentNull(collection, nameof(collection));
            Exceptions.ThrowIfArgumentNull(combatant1, nameof(combatant1));
            Exceptions.ThrowIfArgumentNull(combatant2, nameof(combatant2));

            int temp = combatant1.InitiativeOrder;
            combatant1.InitiativeOrder = combatant2.InitiativeOrder;
            combatant2.InitiativeOrder = temp;
        }
        /// <summary>
        /// Moves the combatant up in initiative
        /// </summary>
        /// <param name="combatant">Combatant to move up</param>
        public static void MoveUp(this IInitiativeCollection collection, IActiveCombatant combatant)
        {
            Exceptions.ThrowIfArgumentNull(collection, nameof(collection));
            Exceptions.ThrowIfArgumentNull(combatant, nameof(combatant));

            IActiveCombatant? combatant2 = collection.CombatantBefore(combatant);
            if (combatant2 != null)
                collection.Swap(combatant, combatant2);
        }
        /// <summary>
        /// Moves the combatant down in initiative
        /// </summary>
        /// <param name="combatant">Combatant to move down</param>
        public static void MoveDown(this IInitiativeCollection collection, IActiveCombatant combatant)
        {
            Exceptions.ThrowIfArgumentNull(collection, nameof(collection));
            Exceptions.ThrowIfArgumentNull(combatant, nameof(combatant));

            IActiveCombatant? combatant2 = collection.CombatantAfter(combatant);
            if (combatant2 != null)
                collection.Swap(combatant, combatant2);
        }
        /// <summary>
        /// Determines whether or not the given combatant can be moved before the combatant specified by <paramref name="before"/>
        /// </summary>
        /// <param name="collection">Combatant collection</param>
        /// <param name="combatant">Combatant to move</param>
        /// <param name="before">Combatant to move <paramref name="combatant"/> before</param>
        /// <returns>Whether or not the combatant can be moved</returns>
        public static bool CanMoveBefore(this IInitiativeCollection collection, IActiveCombatant combatant, IActiveCombatant before)
        {
            Exceptions.ThrowIfArgumentNull(collection, nameof(collection));
            Exceptions.ThrowIfArgumentNull(combatant, nameof(combatant));
            Exceptions.ThrowIfArgumentNull(before, nameof(before));

            return (combatant.InitiativeOrder != before.InitiativeOrder)
                && (combatant.InitiativeOrder != before.InitiativeOrder - 1);
        }
        /// <summary>
        /// Moves the combatant give in <paramref name="combatant"/> before the combatnt given in <paramref name="before"/> in initiative
        /// </summary>
        /// <param name="collection">Combatant collection</param>
        /// <param name="combatant">Combatant to move</param>
        /// <param name="before">Combatant to move <paramref name="combatant"/> before</param>
        public static void MoveBefore(this IInitiativeCollection collection, IActiveCombatant combatant, IActiveCombatant before)
        {
            Exceptions.ThrowIfArgumentNull(collection, nameof(collection));
            Exceptions.ThrowIfArgumentNull(combatant, nameof(combatant));
            Exceptions.ThrowIfArgumentNull(before, nameof(before));

            if (collection.CanMoveBefore(combatant, before))
            {
                List<IActiveCombatant> combatants = collection.Combatants
                    .OrderBy(p => p.InitiativeOrder)
                    .ToList();

                combatants.Remove(combatant);
                combatants.InsertBefore(combatant, before);
                for (int i = 0; i < combatants.Count; i++)
                    combatants[i].InitiativeOrder = i + 1;
            }
        }
        /// <summary>
        /// Determines if the given combatant can be moved to after the combatant given by <paramref name="after"/>
        /// </summary>
        /// <param name="collection">Collection of combatants</param>
        /// <param name="combatant">Combatant to move</param>
        /// <param name="after">Combatant to move after</param>
        /// <returns>Whether or not the combatant can be moved</returns>
        public static bool CanMoveAfter(this IInitiativeCollection collection, IActiveCombatant combatant, IActiveCombatant after)
        {
            Exceptions.ThrowIfArgumentNull(collection, nameof(collection));
            Exceptions.ThrowIfArgumentNull(combatant, nameof(combatant));
            Exceptions.ThrowIfArgumentNull(after, nameof(after));

            return (combatant.InitiativeOrder != after.InitiativeOrder)
                && (combatant.InitiativeOrder != after.InitiativeOrder + 1);
        }
        /// <summary>
        /// Moves the given combatant to after the combatant given by <paramref name="after"/>
        /// </summary>
        /// <param name="collection">Collection of combatants</param>
        /// <param name="combatant">Combatant to move</param>
        /// <param name="after">Combatant to move after</param>
        public static void MoveAfter(this IInitiativeCollection collection, IActiveCombatant combatant, IActiveCombatant after)
        {
            Exceptions.ThrowIfArgumentNull(collection, nameof(collection));
            Exceptions.ThrowIfArgumentNull(combatant, nameof(combatant));
            Exceptions.ThrowIfArgumentNull(after, nameof(after));

            IActiveCombatant? before = collection.CombatantAfter(after);

            //  If there is no next, then move to the end
            if (before == null)
                collection.MoveToEnd(combatant);
            else
                collection.MoveBefore(combatant, before);
        }
        /// <summary>
        /// Moves the combatant give in <paramref name="combatant"/> to the end of initiative
        /// </summary>
        /// <param name="collection">Combatant collection</param>
        /// <param name="combatant">Combatant to move</param>
        public static void MoveToEnd(this IInitiativeCollection collection, IActiveCombatant combatant)
        {
            Exceptions.ThrowIfArgumentNull(collection, nameof(collection));
            Exceptions.ThrowIfArgumentNull(combatant, nameof(combatant));

            List<IActiveCombatant> combatants = collection.Combatants
                .OrderBy(p => p.InitiativeOrder)
                .ToList();

            combatants.Remove(combatant);
            combatants.Add(combatant);
            for (int i = 0; i < combatants.Count; i++)
                combatants[i].InitiativeOrder = i + 1;
        }

        /// <summary>
        /// Determines whether the given combatant can be moved up in combat
        /// </summary>
        /// <param name="collection">Collection the combatant is in</param>
        /// <param name="combatant">Combatant to test</param>
        /// <returns>Whether or not the combatant can be moved up</returns>
        public static bool CanMoveUp(this IInitiativeCollection collection, IActiveCombatant combatant)
        {
            Exceptions.ThrowIfArgumentNull(collection, nameof(collection));
            Exceptions.ThrowIfArgumentNull(combatant, nameof(combatant));

            return combatant.InitiativeOrder != 1;
        }
        /// <summary>
        /// Determines whether the given combatant can be moved down in combat
        /// </summary>
        /// <param name="collection">Collection the combatant is in</param>
        /// <param name="combatant">Combatant to test</param>
        /// <returns>Whether or not the combatant can be moved down</returns>
        public static bool CanMoveDown(this IInitiativeCollection collection, IActiveCombatant combatant)
        {
            Exceptions.ThrowIfArgumentNull(collection, nameof(collection));
            Exceptions.ThrowIfArgumentNull(combatant, nameof(combatant));

            return combatant.InitiativeOrder != collection.Combatants.Max(p => p.InitiativeOrder);
        }
    }
}
