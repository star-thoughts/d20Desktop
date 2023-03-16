using Fiction.GameScreen.Serialization;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Container for data used to calculate attribute modifiers
    /// </summary>
    public sealed class AttributeCalculationData
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="AttributeCalculationData"/>
        /// </summary>
        /// <param name="serializer">Serializer to use to get campaign information</param>
        public AttributeCalculationData(ICampaignSerializer serializer)
        {
            Exceptions.ThrowIfArgumentNull(serializer, nameof(serializer));

            Serializer = serializer;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Serializer for character information to use
        /// </summary>
        public ICampaignSerializer Serializer { get; set; }
        /// <summary>
        /// Gets or sets the attribute container
        /// </summary>
        public IAttributeContainer Attributes { get; set; }
        /// <summary>
        /// Gets or sets the modifier containers to use for calculating attributes
        /// </summary>
        public IModifierContainer[] ModifierContainers { get; set; }
        #endregion
    }
}