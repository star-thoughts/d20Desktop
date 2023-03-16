using System;
using System.Collections.Generic;
using System.Text;

namespace Fiction.GameScreen.Characters
{
    public sealed class FeatDefinition : SpecialDefinition
    {
        /// <summary>
        /// Constructs a new <see cref="FeatDefinition"/>
        /// </summary>
        /// <param name="name">Name of the definition</param>
        /// <param name="modifiers">Modifiers this definition gives</param>
        public FeatDefinition(string name, params AttributeModifier[] modifiers)
            : base(name, modifiers)
        {
        }
    }
}
