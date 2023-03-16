using System.Linq;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Contains the results of a call to <see cref="ActiveCombat.GotoNext"/>
    /// </summary>
    public sealed class GotoNextResult
    {
        /// <summary>
        /// Constructs a new <see cref="GotoNextResult"/>
        /// </summary>
        /// <param name="completed">Whether or not combat should be completed</param>
        /// <param name="combatants">Information about each combatant that went</param>
        public GotoNextResult(bool completed, params GotoNextCombatant[] combatants)
        {
            Completed = completed;
            Combatants = combatants.ToArray();
        }
        /// <summary>
        /// Gets or sets the combatants, in order, that were passed through attempting to find the next combatant
        /// </summary>
        public GotoNextCombatant[] Combatants { get; private set; }
        /// <summary>
        /// Gets whether or not combat was completed by this (no more combatants can go)
        /// </summary>
        public bool Completed { get; private set; }
    }
}