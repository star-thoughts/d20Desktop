namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Enumeration of possible stacking values
    /// </summary>
    public enum ModifierStackingType
    {
        /// <summary>
        /// Default value, dependant on type using this enum
        /// </summary>
        Default,
        /// <summary>
        /// Does not stack with others of the same type
        /// </summary>
        NonStacking,
        /// <summary>
        /// Stacks with others of hte same type
        /// </summary>
        Stacking,
    }
}