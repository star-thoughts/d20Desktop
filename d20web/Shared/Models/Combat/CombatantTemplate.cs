namespace d20Web.Models.Combat
{
    /// <summary>
    /// Template for creating combatants in combat
    /// </summary>
    public class CombatantTemplate
    {
        /// <summary>
        /// Gets the ID of this combatant
        /// </summary>
        public string? ID { get; set; }
        /// <summary>
        /// Gets or sets the name of the combatant
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the string used for Hit Dice
        /// </summary>
        public string? HitDieString { get; set; }
        /// <summary>
        /// Gets or sets the rolling strategy to use
        /// </summary>
        public RollingStrategy HitDieRollingStrategy { get; set; }
        /// <summary>
        /// Gets or sets the default initiative modifier for this combatant
        /// </summary>
        public int InitiativeModifier { get; set; }
        /// <summary>
        /// Gets or sets the number of combatants to add
        /// </summary>
        public string? Count { get; set; }
        /// <summary>
        /// Gets or sets whether or not to display this combatant to the players
        /// </summary>
        public bool DisplayToPlayers { get; set; }
        /// <summary>
        /// Gets or sets the name to display to the players during combat
        /// </summary>
        public string? DisplayName { get; set; }
        /// <summary>
        /// Gets or sets the amount of fast healing the combatant will have
        /// </summary>
        public int FastHealing { get; set; }
        /// <summary>
        /// Gets or sets the amount of hit points the combatant is dead at
        /// </summary>
        public int DeadAt { get; set; }
        /// <summary>
        /// Gets or sets the amount of hit points the combatant is unconscious at
        /// </summary>
        public int UnconsciousAt { get; set; }
        /// <summary>
        /// Gets or sets the source for this combatant template, or null if no source.
        /// </summary>
        public string? SourceID { get; set; }
        /// <summary>
        /// Gets a collection of damage reductions
        /// </summary>
        public DamageReduction[]? DamageReduction { get; set; }
    }
}