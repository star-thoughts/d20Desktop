using System;
using System.Collections.Generic;
using System.Text;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Base interface for objects that contain modifiers
    /// </summary>
    public interface IModifierContainer
    {
        /// <summary>
        /// Gets all of the attribute modifiers for a given attribute
        /// </summary>
        /// <param name="data">Data used to determine which modifiers to use</param>
        /// <returns>Collection of modifiers for the given attribute</returns>
        AttributeModifier[] GetAttributeModifiers(AttributeCalculationData data);
    }
}
