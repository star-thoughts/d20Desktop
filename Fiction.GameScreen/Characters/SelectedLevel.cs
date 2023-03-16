using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Represents a level in a class that is selected
    /// </summary>
    public sealed class SelectedLevel : SelectedItem<ClassDefinition>
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SelectedLevel"/>
        /// </summary>
        /// <param name="definition">Definition of the selected item</param>
        /// <param name="actualLevel">Level of class taken</param>
        /// <param name="qualities"></param>
        public SelectedLevel(ClassDefinition definition, int actualLevel, params SelectedSpecialQuality[] qualities)
            : base(definition, qualities)
        {
            ActualLevel = actualLevel;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the level of the class this represents
        /// </summary>
        public int ActualLevel { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Gets all of the attribute modifiers for a given attribute
        /// </summary>
        /// <param name="data">Data used to determine which modifiers to use</param>
        /// <returns>Collection of modifiers for the given attribute</returns>
        public override AttributeModifier[] GetAttributeModifiers(AttributeCalculationData data)
        {
            AttributeModifier[] baseModifiers = base.GetAttributeModifiers(data);
            AttributeModifier[] levelModifiers = GetLevelModifiers();

            AttributeModifier[] results = new AttributeModifier[baseModifiers.Length + levelModifiers.Length + 2];
            Array.Copy(baseModifiers, results, baseModifiers.Length);
            Array.Copy(levelModifiers, 0, results, baseModifiers.Length, levelModifiers.Length);

            results[results.Length - 2] = new AttributeModifierStatic(ModifierType.ClassLevelModifierType, 1, Definition.LevelAttribute);
            results[results.Length - 1] = new AttributeModifierStatic(ModifierType.ClassLevelModifierType, 1, AttributeDefinition.TotalLevelAttribute);
            return results;
        }

        private AttributeModifier[] GetLevelModifiers()
        {
            ClassLevelDefinition level = null;
            if (ActualLevel <= Definition.Levels.Length)
                level = Definition.Levels[ActualLevel - 1];

            if (level != null)
                return level.Modifiers.ToArray();

            return Array.Empty<AttributeModifier>();
        }
        #endregion
    }
}
