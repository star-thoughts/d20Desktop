using System.Linq;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Contains information about a single combatant's attempt to go next
    /// </summary>
    public class GotoNextCombatant
    {
        /// <summary>
        /// Constructs a new <see cref="GotoNextCombatant"/>
        /// </summary>
        /// <param name="combatant">Combatant that was attempted</param>
        /// <param name="effects">Effects that expired as a result of this combatant going</param>
        public GotoNextCombatant(ICombatant? combatant, params Effect[] effects)
        {
            Combatant = combatant;
            ExpiredEffects = effects.ToArray();
        }
        /// <summary>
        /// Gets the combatant that went
        /// </summary>
        public ICombatant? Combatant { get; private set; }
        /// <summary>
        /// Gets a collection of effects that expired
        /// </summary>
        public Effect[] ExpiredEffects { get; private set; }
    }
}