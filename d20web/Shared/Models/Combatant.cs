namespace d20Web.Models
{
    /// <summary>
    /// Contains information about a combatant
    /// </summary>
    public sealed class Combatant
    {
        /// <summary>
        /// Gets or sets the ID of the combatant
        /// </summary>
        public string? ID { get; set; }
        /// <summary>
        /// Gets or sets the name of the combatant
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the name to display to players
        /// </summary>
        public string? DisplayName { get; set; }
        /// <summary>
        /// Gets or sets the ordinal for this combatant, for multiple combatants with the same name
        /// </summary>
        public int Ordinal { get; set; }
        /// <summary>
        /// Gets or sets the health related information for this combatant
        /// </summary>
        public CombatantHealth? Health { get; set; }
        /// <summary>
        /// Gets or sets the initiative order of this combatant
        /// </summary>
        public int InitiativeOrder { get; set; }
        /// <summary>
        /// Gets or sets whether or not to display this combatant to players
        /// </summary>
        public bool DisplayToPlayers { get; set; }
        /// <summary>
        /// Gets or sets whether or not this combatant is the current combatant
        /// </summary>
        public bool IsCurrent { get; set; }
        /// <summary>
        /// Gets or sets whether or not this combatant has gone at least once
        /// </summary>
        public bool HasGoneOnce { get; set; }
        /// <summary>
        /// Gets or sets whether or not this combatant is a player character
        /// </summary>
        public bool IsPlayer { get; set; }
        /// <summary>
        /// Gets or sets whether or not to include this combatant in combat (mainly to remove killed combatants)
        /// </summary>
        public bool IncludeInCombat { get; set; }
        /// <summary>
        /// Gets or sets a collection of descriptors for damage reduction for this combatant
        /// </summary>
        public IEnumerable<DamageReduction>? DamageReduction { get; set; }
        /// <summary>
        /// Gets or sets conditions that have been applied to this combatant
        /// </summary>
        public IEnumerable<AppliedCondition>? AppliedConditions { get; set; }
    }
}
