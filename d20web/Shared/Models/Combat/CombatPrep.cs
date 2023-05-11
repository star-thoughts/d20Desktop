namespace d20Web.Models.Combat
{
    /// <summary>
    /// Information for preparing for combat
    /// </summary>
    public sealed class CombatPrep
    {
        /// <summary>
        /// Gets or sets the ID of the combat prep on the server
        /// </summary>
        public string? ID { get; set; }
        /// <summary>
        /// Gets or sets a collection of combatants
        /// </summary>
        public IEnumerable<CombatantPreparer>? Combatants { get; set; }
    }
}
