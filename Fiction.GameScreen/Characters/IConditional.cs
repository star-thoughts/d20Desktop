using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Base interface for conditional statements
    /// </summary>
    public interface IConditional
    {
        /// <summary>
        /// Gets or sets the operator this conditional uses if it is in a group with other operators
        /// </summary>
        ConditionalLogicalOperator Operator { get; set; }
        /// <summary>
        /// Checks the given character to determine if the character meets the condition as-is
        /// </summary>
        /// <param name="character">Character to test against</param>
        /// <returns>Whether or not the condition is met</returns>
        Task<bool> ConditionMetAsync(Character character);
        /// <summary>
        /// Checks to see if the current calculations for a character meets the condition
        /// </summary>
        /// <param name="calculationData">Calculation data representing the current state of calculations</param>
        /// <returns>Whether or not conditions are met</returns>
        Task<bool> ConditionMetAsync(AttributeCalculationData calculationData);
    }
}