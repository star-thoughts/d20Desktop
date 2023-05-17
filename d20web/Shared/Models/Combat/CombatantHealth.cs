namespace d20Web.Models.Combat
{
    /// <summary>
    /// Contains health information for a combatant
    /// </summary>
    public sealed class CombatantHealth
    {
        /// <summary>
        /// Gets or sets the maximum hit points this combatant can have
        /// </summary>
        public int MaxHealth { get; set; }
        /// <summary>
        /// Gets or sets the lethal damage applied to this combatant
        /// </summary>
        public int LethalDamage { get; set; }
        /// <summary>
        /// Gets or sets the non-lethal damage applied to this combatant
        /// </summary>
        public int NonLethalDamage { get; set; }
        /// <summary>
        /// Gets or sets the temporary hit points applied to this combatant
        /// </summary>
        public int TemporaryHitPoints { get; set; }
        /// <summary>
        /// Gets or sets at what "current" health this combatant is considered dead
        /// </summary>
        public int DeadAt { get; set; }
        /// <summary>
        /// Gets or sets at what "current" health this combatant is considered unconscious
        /// </summary>
        public int UnconsciousAt { get; set; }
    }
}