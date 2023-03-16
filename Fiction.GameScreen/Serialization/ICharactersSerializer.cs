using Fiction.GameScreen.Characters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Serialization
{
    /// <summary>
    /// Interface for the serializer used to read/write character information
    /// </summary>
    public interface ICharactersSerializer : IDisposable
    {
        /// <summary>
        /// Gets the definition of an attribute
        /// </summary>
        /// <param name="id">ID of the definition to retrieve</param>
        /// <returns>Attribute definition read</returns>
        Task<AttributeDefinition> ReadAttributeDefinition(object id);
        /// <summary>
        /// Writes an attribute definition
        /// </summary>
        /// <param name="id">ID of hte attribute definition</param>
        /// <param name="definition">Definition to write</param>
        /// <returns>Task for asynchronous completion</returns>
        Task WriteAttributeDefinition(object id, AttributeDefinition definition);
        /// <summary>
        /// Gets a collection of attribute definitions from the data source
        /// </summary>
        /// <returns>Collection containing all attributes</returns>
        Task<AttributeDefinition[]> GetAllAttributeDefinitions();
        /// <summary>
        /// Gets the modifier type for the given ID
        /// </summary>
        /// <param name="id">ID of the modifier type</param>
        /// <returns>Modifier type</returns>
        Task<ModifierType> GetModifierType(object id);
        /// <summary>
        /// Gets a collection of modifier types
        /// </summary>
        /// <returns>Collection of all modifier types</returns>
        Task<ModifierType[]> GetAllModifierTypes();
        /// <summary>
        /// Gets a collection of attribute modifiers in the campaign
        /// </summary>
        /// <returns>Collection of attribute modifiers</returns>
        Task<AttributeModifier[]> GetAllCampaignAttributeModifiers();
    }
}
