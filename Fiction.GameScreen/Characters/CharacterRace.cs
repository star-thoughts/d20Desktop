using System;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// 
    /// </summary>
    public class CharacterRace : SelectedItem<RaceDefinition>
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CharacterRace"/>
        /// </summary>
        /// <param name="definition">Race definition this selected race is for</param>
        /// <param name="qualities">Information about the special qualities selected</param>
        public CharacterRace(RaceDefinition definition, params SelectedSpecialQuality[] qualities)
            : base(definition, qualities)
        {
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets all of the attribute modifiers for a given attribute
        /// </summary>
        /// <param name="data">Data used to determine which modifiers to use</param>
        /// <returns>Collection of modifiers for the given attribute</returns>
        public override AttributeModifier[] GetAttributeModifiers(AttributeCalculationData data)
        {
            AttributeModifier[] modifiers = base.GetAttributeModifiers(data);
            AttributeModifier[] withRace = new AttributeModifier[modifiers.Length + 2];

            Array.Copy(modifiers, withRace, modifiers.Length);
            //  Modifier for the type of hit dice
            withRace[withRace.Length - 2] = new AttributeModifierStatic(ModifierType.RacialModifierType, Definition.HitDice, Definition.HitDieAttribute);
            //  Modifier for the total hit dice
            withRace[withRace.Length - 1] = new AttributeModifierStatic(ModifierType.RacialModifierType, Definition.HitDice, AttributeDefinition.TotalHitDiceAttribute);

            return withRace;
        }
        #endregion
    }
}