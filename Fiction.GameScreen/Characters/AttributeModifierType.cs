namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Enumeration of types of attributes
    /// </summary>
    public enum AttributeModifierType
    {
        /// <summary>
        /// Attribute modifier is a normal attribute that is summed
        /// </summary>
        Value,
        /// <summary>
        /// Attribute modifier is calculated like an ability score (Value / 2 - 5)
        /// </summary>
        AbilityScore,
    }
}