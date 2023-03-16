using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// A conditional that specifies an attribute value
    /// </summary>
    public sealed class ConditionalAttributeRequirement : IConditional
    {
        #region Constructors
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the operator for this conditional in a group
        /// </summary>
        public ConditionalLogicalOperator Operator { get; set; }
        /// <summary>
        /// Gets or sets the attribute to test against
        /// </summary>
        public AttributeDefinition Attribute { get; set; }
        /// <summary>
        /// Gets or sets the rquired value for this attribute
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// Gets or sets the relation of the value to the operator
        /// </summary>
        /// <remarks>
        /// The relation is set up such that the attribute is on the left, and the value is on the right.
        /// </remarks>
        public RelationalOperator ValueRelation { get; set; }
        #endregion
        #region Methods
        /// <summary>
        /// Checks the given character to determine if the character meets the condition as-is
        /// </summary>
        /// <param name="character">Character to test against</param>
        /// <returns>Whether or not the condition is met</returns>
        public async Task<bool> ConditionMetAsync(Character character)
        {
            Exceptions.ThrowIfArgumentNull(character, nameof(character));
            CalculatedAttribute attribute = await character.InnerGetAttributeAsync(Attribute);

            return ValueRelation.Evaluate(attribute.Modifier, Value);
        }

        /// <summary>
        /// Checks to see if the current calculations for a character meets the condition
        /// </summary>
        /// <param name="calculationData">Calculation data representing the current state of calculations</param>
        /// <returns>Whether or not conditions are met</returns>
        public async Task<bool> ConditionMetAsync(AttributeCalculationData calculationData)
        {
            Exceptions.ThrowIfArgumentNull(calculationData, nameof(calculationData));
            CalculatedAttribute attribute = await calculationData.Attributes.AttributeManager.GetOrCalculateAttributeAsync(Attribute, calculationData);

            return ValueRelation.Evaluate(attribute.Modifier, Value);
        }
        #endregion
    }
}
