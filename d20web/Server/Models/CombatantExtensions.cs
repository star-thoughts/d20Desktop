using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
            return combatant.IncludeInCombat && combatant.DisplayToPlayers && (combatant.IsPlayer || combatant.HasGoneOnce);
        }

        public static Combatant ToPlayerView(this Combatant combatant)
        {
            return new Combatant()
            {
                Name = combatant.DisplayName,
                ID = combatant.ID,
                DisplayName = combatant.DisplayName,
                IncludeInCombat = combatant.IncludeInCombat,
                InitiativeOrder = combatant.InitiativeOrder,
                IsCurrent   = combatant.IsCurrent,
                IsPlayer = combatant.IsPlayer,
                Ordinal = combatant.Ordinal,
            };
        }
    }
}
