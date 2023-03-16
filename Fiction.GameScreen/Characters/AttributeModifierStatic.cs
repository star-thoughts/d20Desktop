using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Modifier for an attribute using a static modifier value
    /// </summary>
    public sealed class AttributeModifierStatic : AttributeModifier
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="AttributeModifierStatic"/>
        /// </summary>
        /// <param name="value">Value to use for calculation</param>
        /// <param name="targets">Definitions of the target attributes</param>
        public AttributeModifierStatic(ModifierType type, int value, IEnumerable<object> targets)
            : base(type, targets)
        {
            Value = value;
        }
        /// <summary>
        /// Constructs a new <see cref="AttributeModifierStatic"/>
        /// </summary>
        /// <param name="targets">Definitions of the target attributes</param>
        /// <param name="value">Value to use for calculation</param>
        public AttributeModifierStatic(ModifierType type, int value, params object[] targets)
            : base(type, targets)
        {
            Value = value;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the value to modify the target attributes
        /// </summary>
        public int Value { get; set; }
        #endregion
        #region Methods
        /// <summary>
        /// Calculates the total modifier value from this <see cref="AttributeModifier"/>
        /// </summary>
        /// <param name="data">Data to use to calculate the total value</param>
        /// <returns>Calculated total value</returns>
        public async override Task<CalculatedAttributeModifier> CalculateModifierAsync(AttributeCalculationData data)
        {
            ModifierType type = await data.Serializer.GetModifierType(ModifierTypeId);
            return new CalculatedAttributeModifier(this, type, Value);
        }
        #endregion
    }
}
