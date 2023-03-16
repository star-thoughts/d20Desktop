using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Defines information about a special quality
    /// </summary>
    public sealed class SpecialQualityDefinition : SpecialDefinition
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SpecialQualityDefinition"/>
        /// </summary>
        /// <param name="name">Name of the definition</param>
        /// <param name="modifiers">Modifiers this definition gives</param>
        public SpecialQualityDefinition(string name, params AttributeModifier[] modifiers)
            : base(name, modifiers)
        {
        }
        #endregion
    }
}
