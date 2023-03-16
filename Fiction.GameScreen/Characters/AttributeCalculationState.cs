namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Enumeration of possible states for an attribute, or the attributes on an <see cref="IAttributeContainer"/>
    /// </summary>
    public enum AttributeCalculationState
    {
        /// <summary>
        /// Current state of the attribute is unknown and needs calculating
        /// </summary>
        Unknown,
        /// <summary>
        /// Attribute or attributes are being calculated
        /// </summary>
        Calculating,
        /// <summary>
        /// The Attribute or attributes have been calculated
        /// </summary>
        Calculated,
        /// <summary>
        /// The attribute needs to be recalculated
        /// </summary>
        Recalculate,
    }
}