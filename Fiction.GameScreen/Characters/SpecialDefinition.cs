using System.Collections.Immutable;

namespace Fiction.GameScreen.Characters
{
    public abstract class SpecialDefinition : Definition
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SpecialDefinition"/>
        /// </summary>
        /// <param name="name">Name of the special</param>
        /// <param name="modifiers">Modifiers for the special</param>
        protected SpecialDefinition(string name, params AttributeModifier[] modifiers)
            : base(name, modifiers)
        {
            Tags = ImmutableArray<string>.Empty;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets a collection of tags on this definition
        /// </summary>
        public ImmutableArray<string> Tags { get; set; }
        #endregion
    }
}