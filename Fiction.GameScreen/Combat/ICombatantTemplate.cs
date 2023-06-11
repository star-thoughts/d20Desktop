namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Base interface for combatants in a combat scenario
    /// </summary>
    public interface ICombatantTemplate : ICombatantBase, ICombatantSource
    {
        #region Properties
        public string? ServerID { get; set; }
        /// <summary>
        /// Gets or sets a string representation of the hit dice for this combatant template
        /// </summary>
        string HitDieString { get; set; }
        /// <summary>
        /// Gets or sets the rolling strategy to use when rolling hit dice for this combatant
        /// </summary>
        RollingStrategy HitDieRollingStrategy { get; set; }
        /// <summary>
        /// Gets or sets the initiative modifier for this template
        /// </summary>
        int InitiativeModifier { get; set; }
        /// <summary>
        /// Gets or sets the number of combatants to add to combat
        /// </summary>
        string Count { get; set; }
        /// <summary>
        /// Gets whether or not the information in this combatant is valid
        /// </summary>
        bool IsValid { get; }
        /// <summary>
        /// Gets or sets the amount of fast healing this combatant has
        /// </summary>
        int FastHealing { get; set; }
        /// <summary>
        /// Gets or sets the amount of hit points the combatant is dead at
        /// </summary>
        int DeadAt { get; set; }
        /// <summary>
        /// Gets or sets the amount of hit points the combatant is unconscious at
        /// </summary>
        int UnconsciousAt { get; set; }
        /// <summary>
        /// Gets or sets the source of this combatant template
        /// </summary>
        ICombatantTemplateSource? Source { get; set; }
        #endregion
        #region Methods
        /// <summary>
        /// Creates a combatant for an active combat
        /// </summary>
        /// <param name="preparer">Preparation information for the combatant</param>
        /// <returns>Combatant created</returns>
        ICombatant CreateCombatant(CombatantPreparer preparer);
        #endregion
    }
}
