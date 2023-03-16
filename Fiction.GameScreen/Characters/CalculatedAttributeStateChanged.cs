using System;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Information about the change of the <see cref="CalculatedAttribute.State"/> of a <see cref="CalculatedAttribute"/>
    /// </summary>
    public class CalculatedAttributeStateChanged : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CalculatedAttributeStateChanged"/>
        /// </summary>
        /// <param name="oldState">Previous state of the attribute</param>
        /// <param name="newState">New state for the attribute</param>
        public CalculatedAttributeStateChanged(AttributeCalculationState oldState, AttributeCalculationState newState)
        {
            OldState = oldState;
            NewState = newState;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the previous state of the <see cref="CalculatedAttribute"/>
        /// </summary>
        public AttributeCalculationState OldState { get; private set; }
        /// <summary>
        /// Gets the new state of the <see cref="CalculatedAttribute"/>
        /// </summary>
        public AttributeCalculationState NewState { get; private set; }
        #endregion
    }
}