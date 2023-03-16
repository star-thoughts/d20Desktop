using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Definition of a class
    /// </summary>
    public sealed class ClassDefinition : Definition
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ClassDefinition"/>
        /// </summary>
        /// <param name="name">Name of the class</param>
        /// <param name="modifiers">Base modifiers from this class</param>
        public ClassDefinition(string name, params AttributeModifier[] modifiers)
            : base(name, modifiers)
        {
            Levels = ImmutableArray<ClassLevelDefinition>.Empty;
        }
        #endregion
        #region Member Variables
        private AttributeDefinition _levelAttribute;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the name of the definition
        /// </summary>
        public override string Name
        {
            get { return base.Name; }
            set
            {
                if (!string.Equals(base.Name, value))
                {
                    base.Name = value;
                    _levelAttribute = null;
                }
            }
        }
        /// <summary>
        /// Gets the attribute used for determining class level
        /// </summary>
        public AttributeDefinition LevelAttribute
        {
            get
            {
                if (_levelAttribute == null)
                    _levelAttribute = new AttributeDefinition(Name + "Level", Name, AttributeDefinition.ClassLevelType, AttributeModifierType.Value);
                return _levelAttribute;
            }
        }
        /// <summary>
        /// Gets or sets information about each level in the class
        /// </summary>
        public ImmutableArray<ClassLevelDefinition> Levels { get; set; }
        #endregion
    }
}
