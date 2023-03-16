using System;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Enumeration of operators allowed for conditional statements
    /// </summary>
    public enum ConditionalLogicalOperator
    {
        /// <summary>
        /// This condition's logical operator is unset, if it is used an <see cref="InvalidOperationException"/> will be thrown
        /// </summary>
        Unset,
        /// <summary>
        /// This condition plus the next condition must both be true
        /// </summary>
        And,
        /// <summary>
        /// Either this condition or the next condition can be true
        /// </summary>
        Or
    }
}