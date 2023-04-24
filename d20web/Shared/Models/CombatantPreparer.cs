namespace d20Web.Models
{
    public sealed class CombatantPreparer
    {
        /// <summary>
        /// Gets or sets the ID of the combatant on the combat server
        /// </summary>
        public string? ID { get; set; }
        /// <summary>
        /// Gets or sets the name of this combatant
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Gets the ordinal for this combatant
        /// </summary>
        public int Ordinal { get; set; }
        /// <summary>
        /// Gets or sets the actual initiative roll
        /// </summary>
        public int InitiativeRoll { get; set; }
        /// <summary>
        /// Gets or sets the initiative modifier
        /// </summary>
        public int InitiativeModifier { get; set; }
        /// <summary>
        /// Gets whether or not this is a player character
        /// </summary>
        public bool IsPlayer { get; set; }
    }
}
