using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Represents a group of conditional statements, joined by logical operators
    /// </summary>
    public sealed class ConditionalGroup : IConditional
    {
        #region Constructors
        /// <summary>
        /// Constructs a default, empty <see cref="ConditionalGroup"/>
        /// </summary>
        public ConditionalGroup()
        {
            Children = ImmutableArray<IConditional>.Empty;
        }
        /// <summary>
        /// Constructs a new <see cref="ConditionalGroup"/> with a collection of condition children
        /// </summary>
        /// <param name="conditions">Children of this group</param>
        public ConditionalGroup(params IConditional[] conditions)
        {
            Children = ImmutableArray.CreateRange(conditions);
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the operator this conditional uses if it is in a group with other operators
        /// </summary>
        public ConditionalLogicalOperator Operator { get; set; }
        /// <summary>
        /// Gets or sets a collection of child conditions
        /// </summary>
        public ImmutableArray<IConditional> Children { get; set; }
        #endregion
        #region Methods
        /// <summary>
        /// Checks the given character to determine if the character meets the condition as-is
        /// </summary>
        /// <param name="character">Character to test against</param>
        /// <returns>Whether or not the condition is met</returns>
        public async Task<bool> ConditionMetAsync(Character character)
        {
            if (!Children.Any())
                return true;

            IConditional last = Children[0];
            bool result = await last.ConditionMetAsync(character);

            for (int i = 1; i < Children.Length; i++)
            {
                switch (last.Operator)
                {
                    case ConditionalLogicalOperator.Unset:
                        throw new InvalidOperationException("An operator must be set for conditional groups.");
                    case ConditionalLogicalOperator.And:
                        result = result && await Children[i].ConditionMetAsync(character);
                        break;
                    case ConditionalLogicalOperator.Or:
                        result = result || await Children[i].ConditionMetAsync(character);
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// Checks to see if the current calculations for a character meets the condition
        /// </summary>
        /// <param name="calculationData">Calculation data representing the current state of calculations</param>
        /// <returns>Whether or not conditions are met</returns>
        public async Task<bool> ConditionMetAsync(AttributeCalculationData calculationData)
        {
            if (!Children.Any())
                return true;

            IConditional last = Children[0];
            bool result = await last.ConditionMetAsync(calculationData);

            for (int i = 1; i < Children.Length; i++)
            {
                switch (last.Operator)
                {
                    case ConditionalLogicalOperator.Unset:
                        throw new InvalidOperationException("An operator must be set for conditional groups.");
                    case ConditionalLogicalOperator.And:
                        result = result && await Children[i].ConditionMetAsync(calculationData);
                        break;
                    case ConditionalLogicalOperator.Or:
                        result = result || await Children[i].ConditionMetAsync(calculationData);
                        break;
                }
            }
            return result;
        }
        #endregion
    }
}
