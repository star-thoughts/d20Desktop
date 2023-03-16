namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Enumeration of relational operators
    /// </summary>
    public enum RelationalOperator
    {
        /// <summary>
        /// Operator is unset, and should throw an exception if it is used
        /// </summary>
        Unset,
        /// <summary>
        /// Values are equal to each other, or equivalent
        /// </summary>
        Equals,
        /// <summary>
        /// The left item in the operation is less than the right item
        /// </summary>
        LessThan,
        /// <summary>
        /// The left item in the operation is less than the right item, or they are equal
        /// </summary>
        LessThanOrEqual,
        /// <summary>
        /// The left item in the operator is greater than the right item
        /// </summary>
        GreaterThan,
        /// <summary>
        /// The left item in the operator is greater than the right item, or they are equal
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// The two items are not equal to each other, nor are they equivalent to each other
        /// </summary>
        NotEqual,
    }
}