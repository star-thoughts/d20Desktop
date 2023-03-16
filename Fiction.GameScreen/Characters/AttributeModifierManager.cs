using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Contains methods to handle attribute modifiers
    /// </summary>
    public class AttributeModifierManager : IAttributeModifierManager
    {
        #region Methods
        /// <summary>
        /// Calculates modifiers and filters out unnecessary ones
        /// </summary>
        /// <param name="data">Data used to calculate modifiers</param>
        /// <param name="modifiers">Modifiers to calculate</param>
        /// <returns>Calculated attribute modifiers to use</returns>
        public async Task<CalculatedAttributeModifier[]> GetApplicableModifiersAsync(AttributeCalculationData data, params AttributeModifier[] modifiers)
        {
            CalculatedAttributeModifier[] calculated = new CalculatedAttributeModifier[modifiers.Length];
            for (int i = 0; i < modifiers.Length; i++)
            {
                calculated[i] = await modifiers[i].CalculateModifierAsync(data);
                //  If the source of the calculation cannot be used, then mark it is not met
                if (calculated[i].Modifier.ModifierSource != null && !(await calculated[i].Modifier.ModifierSource.ToApply.ConditionMetAsync(data)))
                    calculated[i].SetNotMet();
            }

            CalculatedAttributeModifier[] toUse = calculated.Where(p => p.State == CalculatedAttributeModifierState.Used)
                .ToArray();

            List<ModifierType> types = new List<ModifierType>(toUse.Count());

            foreach (object id in toUse.Select(p => p.Modifier.ModifierTypeId))
                types.Add(await data.Serializer.GetModifierType(id));

            types = types
                .Distinct()
                .ToList();

            //  modifiersByType has each type ordered so that the largest is first, and only contains non-stacking modifiers
            foreach (ModifierType type in types)
            {
                CalculatedAttributeModifier[] nonStacking = toUse
                    .Where(p => !p.Stacks)
                    .Where(p => p.ModifierType.Equals(type))
                    .OrderByDescending(p => p.Value)
                    .ToArray();

                CalculatedAttributeModifier greatest = nonStacking.FirstOrDefault();
                foreach (CalculatedAttributeModifier modifier in nonStacking.Skip(1))
                    modifier.SetOverride(greatest);
            }

            return calculated;
        }
        #endregion
    }
}
