namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Base interface for an active combatant
    /// </summary>
    public interface IActiveCombatant : ICombatantBase
    {
        /// <summary>
        /// Gets the ordinal for this combatant
        /// </summary>
        int Ordinal { get; }
        /// <summary>
        /// Gets or sets the initiative order for this combatant
        /// </summary>
        int InitiativeOrder { get; set; }
    }
}
