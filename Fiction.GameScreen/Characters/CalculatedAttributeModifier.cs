namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Contains information about an attribute modifier's calculated totals
    /// </summary>
    public class CalculatedAttributeModifier
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CalculatedAttributeModifier"/>
        /// </summary>
        /// <param name="modifier">Modifier this was calculated for</param>
        /// <param name="modifierType">Gets the modifier type to use for this attribute modifier</param>
        /// <param name="value">Value that was calculated</param>
        public CalculatedAttributeModifier(AttributeModifier modifier, ModifierType modifierType, int value)
        {
            Modifier = modifier;
            ModifierType = modifierType;
            Value = value;
            State = CalculatedAttributeModifierState.Used;
            Override = null;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the modifier that this calculation is for
        /// </summary>
        public AttributeModifier Modifier { get; private set; }
        /// <summary>
        /// Gets the modifier type associated with this modifier
        /// </summary>
        public ModifierType ModifierType { get; private set; }
        /// <summary>
        /// Gets the calculated value
        /// </summary>
        public int Value { get; private set; }
        /// <summary>
        /// Gets whether or not this modifier will stack with others always
        /// </summary>
        /// <remarks>
        /// A value stacks always if it is a penalty, or if the modifier type always stacks, or the modifier itself
        /// overrides stacking rules.
        /// </remarks>
        public bool Stacks
        {
            get
            {
                return (Value < 0)
                    || (Modifier.Stacks == ModifierStackingType.Stacking) || 
                    (Modifier.Stacks == ModifierStackingType.Default && ModifierType.Stacks == ModifierStackingType.Stacking);
            }
        }
        /// <summary>
        /// Gets or sets the state of this calculation when applying to an attribute
        /// </summary>
        public CalculatedAttributeModifierState State { get; private set; }
        /// <summary>
        /// Gets the <see cref="CalculatedAttributeModifier"/> that overrode this one during calculations
        /// </summary>
        public CalculatedAttributeModifier Override { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Sets this modifier to be overridden by another modifier
        /// </summary>
        /// <param name="other">Modifier that overrides this modifier</param>
        /// <remarks>
        /// This modifier should not be used in calculating a total value for an attribute.  This is caused because
        /// another modifier that doesn't stack with this one has a greater value.
        /// </remarks>
        internal void SetOverride(CalculatedAttributeModifier other)
        {
            Override = other;
            State = CalculatedAttributeModifierState.Overridden;
        }
        /// <summary>
        /// Sets this modifier to be <see cref="CalculatedAttributeModifierState.NotMet"/> indicating that the source of this
        /// attribute could not be applied to this character.
        /// </summary>
        internal void SetNotMet()
        {
            State = CalculatedAttributeModifierState.NotMet;
        }
        #endregion
    }
}