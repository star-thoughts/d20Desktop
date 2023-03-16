using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Manages calculating whether or not to apply special qualities and feats
    /// </summary>
    public sealed class SpecialsManager : ISpecialsManager
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SpecialsManager"/>
        /// </summary>
        public SpecialsManager()
        {
        }
        #endregion
        #region Methods
        /// <summary>
        /// Rechecks all specials to make sure they are still apply-able
        /// </summary>
        /// <param name="items">Specials that need recalculating</param>
        /// <param name="character">Character they should apply to</param>
        /// <returns>Task for task completion.</returns>
        public async Task ReexamineQualities(CalculatedSpecial[] items, Character character)
        {
            bool changed = false;
            CalculatedSpecial[] withRequirements = items.Where(p => p.HasApplyRequirements).ToArray();
            do
            {
                changed = false;

                foreach (CalculatedSpecial quality in withRequirements)
                {
                    bool oldValue = quality.Applied;
                    bool newValue = await quality.ApplyConditionMetAsync(character);
                    quality.Applied = newValue;

                    changed = changed || (oldValue != newValue);
                }
            } while (changed);
        }
        #endregion
    }
}
