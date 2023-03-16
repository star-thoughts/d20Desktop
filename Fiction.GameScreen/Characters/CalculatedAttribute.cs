using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Text;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Represents the total calculated value for an attribute on a character
    /// </summary>
    public sealed class CalculatedAttribute
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CalculatedAttribute"/>
        /// </summary>
        /// <param name="definition">Definition for this attribute</param>
        /// <param name="value">The calculated value for this attribute</param>
        /// <param name="state">The current state of this attribute calculation</param>
        public CalculatedAttribute(AttributeDefinition definition, int value, AttributeCalculationState state)
        {
            Exceptions.ThrowIfArgumentNull(definition, nameof(definition));

            Definition = definition;
            Value = value;
            State = state;
            Modifiers = ImmutableArray<CalculatedAttributeModifier>.Empty;
        }
        public CalculatedAttribute(AttributeDefinition definition, int value, params CalculatedAttributeModifier[] modifiers)
            : this(definition, value, AttributeCalculationState.Calculated)
        {
            Modifiers = ImmutableArray.CreateRange(modifiers);
        }
        #endregion
        #region Member Variables
        private AttributeCalculationState _state;
        #endregion
        #region Properties
        /// <summary>
        /// Gets the state of the attribute calculation
        /// </summary>
        public AttributeCalculationState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    AttributeCalculationState old = _state;
                    _state = value;
                    StateChanged?.Invoke(this, new CalculatedAttributeStateChanged(old, value));
                }
            }
        }
        /// <summary>
        /// Gets the definition associated with this attribute
        /// </summary>
        public AttributeDefinition Definition { get; private set; }
        /// <summary>
        /// Gets the value for this attribute
        /// </summary>
        public int Value { get; private set; }
        /// <summary>
        /// Gets the modifier for this calculated attribute
        /// </summary>
        public int Modifier { get { return Definition.CalculateModifier(Value); } }
        /// <summary>
        /// Gets a collection of modifiers that were used for this attribute calculation
        /// </summary>
        public ImmutableArray<CalculatedAttributeModifier> Modifiers { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Sets the current value of the attribute and updates it's modifier list and state to <see cref="AttributeCalculationState.Calculated"/>
        /// </summary>
        /// <param name="value">Current value of the attribute</param>
        /// <param name="modifiers">Modifiers used to calculate the value</param>
        internal void UpdateValue(int value, params CalculatedAttributeModifier[] modifiers)
        {
            Modifiers = ImmutableArray.CreateRange(modifiers);
            Value = value;
            State = AttributeCalculationState.Calculated;
        }
        /// <summary>
        /// Adds the modifiers to this attribute and changes it's state to <see cref="AttributeCalculationState.Recalculate"/>
        /// </summary>
        /// <param name="modifiers">Modifiers to add to the attribute</param>
        public void SetNeedsRecalculating()
        {
            State = AttributeCalculationState.Recalculate;
        }
        #endregion
        #region Events
        /// <summary>
        /// Event that is triggered when the state of this <see cref="CalculatedAttribute"/> changed
        /// </summary>
        public event EventHandler<CalculatedAttributeStateChanged> StateChanged;
        #endregion
    }
}
