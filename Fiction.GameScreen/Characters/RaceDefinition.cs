using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Contains the definition of a race
    /// </summary>
    public class RaceDefinition : Definition
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="RaceDefinition"/>
        /// </summary>
        /// <param name="name">Name of the definition</param>
        /// <param name="modifiers">Modifiers this definition gives</param>
        public RaceDefinition(string name, params AttributeModifier[] modifiers)
            : base(name, modifiers)
        {
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the attribute associated with this race's hit dice
        /// </summary>
        public AttributeDefinition HitDieAttribute
        {
            get
            {
                return AttributeDefinition.HitDieAttributes[HitDieType];
            }
        }
        /// <summary>
        /// Gets or sets the type of die to roll for hit points
        /// </summary>
        public DieType HitDieType { get; set; }
        /// <summary>
        /// Gets or sets the number of hit dice this race gives
        /// </summary>
        public int HitDice { get; set; }
        #endregion
    }
}
