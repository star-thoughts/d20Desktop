using d20Web.Models.Combat;

namespace d20Web.Models
{
    /// <summary>
    /// Extension methods for dealing with combatants
    /// </summary>
    internal static class CombatantExtensions
    {
        /// <summary>
        /// Gets whether or not this combatant can be displayed to players
        /// </summary>
        /// <param name="combatant">Combatant to test against</param>
        /// <returns>Whether or not the given combatant can be displayed to players</returns>
        public static bool CanDisplayToPlayers(this Combatant combatant)
        {
            if (combatant.IsPlayer)
                return true;

            return combatant.IncludeInCombat && combatant.DisplayToPlayers && combatant.HasGoneOnce;
        }

        public static Combatant ToPlayerView(this Combatant combatant)
        {
            string? name = combatant.DisplayName;
            if (string.IsNullOrWhiteSpace(name))
                name = combatant.Name;

            return new Combatant()
            {
                Name = name,
                ID = combatant.ID,
                DisplayName = name,
                IncludeInCombat = combatant.IncludeInCombat,
                InitiativeOrder = combatant.InitiativeOrder,
                IsCurrent = combatant.IsCurrent,
                DisplayToPlayers = combatant.DisplayToPlayers,
                IsPlayer = combatant.IsPlayer,
                Ordinal = combatant.Ordinal,
                HasGoneOnce = combatant.HasGoneOnce,
            };
        }
    }
}
