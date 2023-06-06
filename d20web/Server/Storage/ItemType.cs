namespace d20Web.Storage
{
    /// <summary>
    /// Type of item that can be used by the system
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// Campaign item
        /// </summary>
        Campaign = 1,
        /// <summary>
        /// Combat item
        /// </summary>
        Combat = 2,
        /// <summary>
        /// Combatant in a combat
        /// </summary>
        Combatant = 3,
        /// <summary>
        /// Combatant preparation
        /// </summary>
        CombatPrep = 4,
        /// <summary>
        /// Combatant in a combat prep
        /// </summary>
        CombatantPrep = 5,
        /// <summary>
        /// Monster in the bestiary
        /// </summary>
        Monster = 6,
        /// <summary>
        /// Player character
        /// </summary>
        PlayerCharacter = 7,
    }
}