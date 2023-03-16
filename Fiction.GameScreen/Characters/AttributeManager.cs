using Fiction.GameScreen.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Represents a manager that handles attributes for a character
    /// </summary>
    public sealed class AttributeManager : IAttributeManager
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="AttributeManager"/>
        /// </summary>
        public AttributeManager(ICampaignSerializer serializer, IAttributeModifierManager manager, IAttributeContainer attributes)
        {
            Exceptions.ThrowIfArgumentNull(attributes, nameof(attributes));

            _serializer = serializer;
            _attributes = attributes;
            _modifierManager = manager;
        }
        #endregion
        #region Member Variables
        private ICampaignSerializer _serializer;
        private IAttributeContainer _attributes;
        private IAttributeModifierManager _modifierManager;
        #endregion
        #region Properties
        #endregion
        #region Methods
        /// <summary>
        /// Gets an already calculated attribute from the character, or calculates the attribute and returns the result
        /// </summary>
        /// <param name="definition">Definition of the attribute to get or calculate</param>
        /// <param name="modifiers">Modifiers to use to calculate the attribute</param>
        /// <returns>The attribute information requested</returns>
        public async Task<CalculatedAttribute> GetOrCalculateAttributeAsync(AttributeDefinition definition, params IModifierContainer[] modifiers)
        {
            AttributeCalculationData data = new AttributeCalculationData(_serializer);
            data.Attributes = _attributes;
            data.ModifierContainers = modifiers;

            return await GetOrCalculateAttributeAsync(definition, data);
        }
        /// <summary>
        /// Gets an already calculated attribute from the character, or calculates the attribute and returns the result
        /// </summary>
        /// <param name="definition">Definition of the attribute to get or calculate</param>
        /// <param name="data">Information necessary to compute the calculations</param>
        /// <returns>The attribute information requested</returns>
        public async Task<CalculatedAttribute> GetOrCalculateAttributeAsync(AttributeDefinition definition, AttributeCalculationData data)
        {
            CalculatedAttribute attribute = await _attributes.Attributes.GetAttributeAsync(definition);
            if (attribute == null)
            {
                attribute = GetCalculatedAttribute(definition);
                await _attributes.Attributes.SetAttributeValueAsync(attribute);
            }
            if (attribute.State == AttributeCalculationState.Unknown || attribute.State == AttributeCalculationState.Recalculate)
            {
                await CalculateAttributeAsync(attribute, data);
            }
            return await _attributes.Attributes.GetAttributeAsync(definition);
        }

        private CalculatedAttribute GetCalculatedAttribute(AttributeDefinition definition)
        {
            CalculatedAttribute attribute = new CalculatedAttribute(definition, 0, AttributeCalculationState.Unknown);
            attribute.StateChanged += Attribute_StateChanged;
            return attribute;
        }

        private void Attribute_StateChanged(object sender, CalculatedAttributeStateChanged e)
        {
            if (e.NewState == AttributeCalculationState.Recalculate)
            {
                DetachAll();
                _attributes.Attributes.Clear();
            }
        }

        private void DetachAll()
        {
            CalculatedAttribute[] attributes = _attributes.Attributes.GetAttributes();
            foreach (CalculatedAttribute attribute in attributes)
                attribute.StateChanged -= Attribute_StateChanged;
        }

        /// <summary>
        /// Calculates all attributes for this <see cref="IAttributeContainer"/>
        /// </summary>
        /// <param name="modifiers">Containers for modifiers to use for calculating attributes</param>
        /// <returns>Task for task completion.</returns>
        public async Task CalculateAttributesAsync(params IModifierContainer[] modifiers)
        {
            //  Calculate each attribute
            foreach (AttributeDefinition attribute in await _serializer.GetAllAttributeDefinitions())
            {
                await GetOrCalculateAttributeAsync(attribute, modifiers);
            }
        }

        /// <summary>
        /// Calculates the total value of an attribute
        /// </summary>
        /// <param name="attribute">Attribute to calculate</param>
        /// <param name="data">Information necessary to calculate the attribute</param>
        /// <returns>Task for asynchronous completion.</returns>
        private async Task CalculateAttributeAsync(CalculatedAttribute attribute, AttributeCalculationData data)
        {
            //  Get modifers for the definition and filter out unneeded ones
            AttributeModifier[] modifierList = data.ModifierContainers
                .SelectMany(p => p.GetAttributeModifiers(data))
                .Where(p => p.TargetAttributes.Contains(attribute.Definition))
                .ToArray();

            //  Calculate the total
            CalculatedAttributeModifier[] calculated = await _modifierManager.GetApplicableModifiersAsync(data, modifierList);
            int value = calculated
                .Where(p => p.State == CalculatedAttributeModifierState.Used)
                .Sum(p => p.Value);

            attribute.UpdateValue(value, calculated);
        }
        #endregion
    }
}
