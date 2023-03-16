using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    public interface IAttributeManager
    {
        /// <summary>
        /// Calculates all attributes for this <see cref="IAttributeContainer"/>
        /// </summary>
        /// <param name="modifiers">Containers for modifiers to use for calculating attributes</param>
        /// <returns>Task for task completion.</returns>
        Task CalculateAttributesAsync(params IModifierContainer[] modifiers);
        /// <summary>
        /// Gets an already calculated attribute from the character, or calculates the attribute and returns the result
        /// </summary>
        /// <param name="definition">Definition of the attribute to get or calculate</param>
        /// <param name="modifiers">Modifiers to use to calculate the attribute</param>
        /// <returns>The attribute information requested</returns>
        Task<CalculatedAttribute> GetOrCalculateAttributeAsync(AttributeDefinition definition, params IModifierContainer[] modifiers);
        /// <summary>
        /// Gets an already calculated attribute from the character, or calculates the attribute and returns the result
        /// </summary>
        /// <param name="definition">Definition of the attribute to get or calculate</param>
        /// <param name="data">Information necessary to compute the calculations</param>
        /// <returns>The attribute information requested</returns>
        Task<CalculatedAttribute> GetOrCalculateAttributeAsync(AttributeDefinition definition, AttributeCalculationData data);
    }
}