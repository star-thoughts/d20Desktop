namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Base interface for a source for combatant templates
    /// </summary>
    public interface ICombatantTemplateSource : ICampaignObject
    {
        #region Properties
        /// <summary>
        /// Gets or sets a string representation of the hit dice for this combatant template
        /// </summary>
        string HitDieString { get; set; }
        /// <summary>
        /// Gets or sets the initiative modifier for this template
        /// </summary>
        int InitiativeModifier { get; set; }
        /// <summary>
        /// Gets whether or not the information in this combatant is valid
        /// </summary>
        bool IsValid { get; }
        /// <summary>
        /// Gets or sets the amount of fast healing this combatant has
        /// </summary>
        int FastHealing { get; set; }
        /// <summary>
        /// Gets whether or not this is a player character
        /// </summary>
        bool IsPlayer { get; }
        /// <summary>
        /// Gets the amount of hit points the combatant is dead at
        /// </summary>
        int DeadAt { get; }
        /// <summary>
        /// Gets the amount of hit points the combatant is dead at
        /// </summary>
        int UnconsciousAt { get; }
        #endregion
    }
}
