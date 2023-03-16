using System;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Contains information about a special quality
    /// </summary>
    public sealed class CalculatedSpecial : IModifierContainer
    {
        #region Constructors
        /// <summary>
        /// Cosntructs a new <see cref="CalculatedSpecial"/>
        /// </summary>
        /// <param name="special">Special quality that has been calculated</param>
        public CalculatedSpecial(ISelectedItem special)
        {
            Selected = special;
            //  If there is no apply requirement, then apply it.  Otherwise don't apply it by default.
            Applied = !HasApplyRequirements;
        }
        #endregion
        #region Prpoerties
        /// <summary>
        /// Gets or sets the special quality that was calculated
        /// </summary>
        public ISelectedItem Selected { get; private set; }
        /// <summary>
        /// Gets or sets whether or not the selected special quality was applied
        /// </summary>
        public bool Applied { get; set; }
        /// <summary>
        /// Gets whether or not the special quality has requirements to be applied
        /// </summary>
        public bool HasApplyRequirements { get { return Selected.HasApplyRequirement; } }
        #region Methods
        #endregion
        /// <summary>
        /// Gets all of the attribute modifiers for a given attribute
        /// </summary>
        /// <param name="data">Data used to determine which modifiers to use</param>
        /// <returns>Collection of modifiers for the given attribute</returns>
        public AttributeModifier[] GetAttributeModifiers(AttributeCalculationData data)
        {
            if (Applied)
                return Selected.GetAttributeModifiers(data);

            return Array.Empty<AttributeModifier>();
        }
        /// <summary>
        /// Determines whether the conditions for this are met
        /// </summary>
        /// <param name="character">Character to use for determination</param>
        /// <returns></returns>
        public async Task<bool> ApplyConditionMetAsync(Character character)
        {
            return await Selected.ApplyConditionMetAsync(character);
        }
        #endregion
    }
}