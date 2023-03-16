using System;
using System.Collections.Generic;
using System.Text;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Extension moethods for the <see cref="RelationalOperator"/> enum
    /// </summary>
    public static class RelationalOperatorExtensions
    {
        /// <summary>
        /// Evaluates a relational operator using the given values
        /// </summary>
        /// <param name="op">Operator to evaluate</param>
        /// <param name="left">Left value of the operator</param>
        /// <param name="right">Right value of the operator</param>
        /// <returns>Whether or not the conditional is valid</returns>
        public static bool Evaluate(this RelationalOperator op, int left, int right)
        {
            switch (op)
            {
                case RelationalOperator.Equals:
                    return left == right;
                case RelationalOperator.GreaterThan:
                    return left > right;
                case RelationalOperator.GreaterThanOrEqual:
                    return left >= right;
                case RelationalOperator.LessThan:
                    return left < right;
                case RelationalOperator.LessThanOrEqual:
                    return left <= right;
                case RelationalOperator.NotEqual:
                    return left != right;
            }
            throw new InvalidOperationException("Relational operators must be set before being used.");
        }
    }
}
