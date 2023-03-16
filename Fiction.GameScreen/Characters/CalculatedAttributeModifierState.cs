namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Enumeration of possible states of a <see cref="CalculatedAttributeModifier"/>
    /// </summary>
    public enum CalculatedAttributeModifierState
    {
        /// <summary>
        /// Modifier should be used in calculations
        /// </summary>
        Used,
        /// <summary>
        /// Modifier is overridden by a higher modifier
        /// </summary>
        Overridden,
        /// <summary>
        /// Modifier's source could not be applied to the character (see <see cref="Definition.ToApply"/>)
        /// </summary>
        NotMet,
    }
}