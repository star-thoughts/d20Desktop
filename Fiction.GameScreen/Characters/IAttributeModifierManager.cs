using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Base interface for attribute modifier managers
    /// </summary>
    public interface IAttributeModifierManager
    {
        /// <summary>
        /// Calculates modifiers and filters out unnecessary ones
        /// </summary>
        /// <param name="data">Data used to calculate modifiers</param>
        /// <param name="modifiers">Modifiers to calculate</param>
        /// <returns>Calculated attribute modifiers to use</returns>
        Task<CalculatedAttributeModifier[]> GetApplicableModifiersAsync(AttributeCalculationData data, params AttributeModifier[] modifiers);
    }
}
