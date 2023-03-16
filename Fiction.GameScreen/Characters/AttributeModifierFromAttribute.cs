using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Modifies an attribute using the value from another attribute
    /// </summary>
    public sealed class AttributeModifierFromAttribute : AttributeModifier
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="AttributeModifierFromAttribute"/>
        /// </summary>
        /// <param name="source">Definition for the source attribute to get the modifier value from</param>
        /// <param name="type">Modifier type information</param>
        /// <param name="targets">Attributes that are the target of this attribute</param>
        public AttributeModifierFromAttribute(AttributeDefinition source, ModifierType type, params object[] targets)
            : base(type, targets)
        {
            Exceptions.ThrowIfArgumentNull(source, nameof(source));

            SourceAttribute = source;
        }
        #endregion
        #region Member Variables
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the source for this modifier
        /// </summary>
        public AttributeDefinition SourceAttribute { get; set; }
        #endregion
        #region Methods
        /// <summary>
        /// Calculates the total modifier value from this <see cref="AttributeModifier"/>
        /// </summary>
        /// <param name="data">Data to use to calculate the total value</param>
        /// <returns>Calculated total value</returns>
        public override async Task<CalculatedAttributeModifier> CalculateModifierAsync(AttributeCalculationData data)
        {
            ModifierType type = await data.Serializer.GetModifierType(ModifierTypeId);
            CalculatedAttribute sourceAttribute = await data.Attributes.AttributeManager.GetOrCalculateAttributeAsync(SourceAttribute, data.ModifierContainers);
            int value = sourceAttribute.Modifier;

            return new CalculatedAttributeModifier(this, type, value);
        }
        #endregion
    }
}
