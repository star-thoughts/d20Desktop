using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Base class for definitions (such as special qualities, feats, etc)
    /// </summary>
    public abstract class Definition
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="Definition"/>
        /// </summary>
        /// <param name="name">Name of the definition</param>
        /// <param name="modifiers">Modifiers this definition gives</param>
        protected Definition(string name, params AttributeModifier[] modifiers)
        {
            Exceptions.ThrowIfArgumentNullOrEmpty(name, nameof(name));
            Exceptions.ThrowIfArgumentNull(modifiers, nameof(modifiers));

            Id = Guid.NewGuid();
            Name = name;
            Modifiers = ImmutableArray.Create(modifiers);
            SpecialQualities = ImmutableArray<SpecialQualityDefinition>.Empty;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the name of the definition
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Gets or sets the attribute modifiers inherent to the class
        /// </summary>
        public ImmutableArray<AttributeModifier> Modifiers { get; set; }
        /// <summary>
        /// Gets or sets any special qualities this definition sets
        /// </summary>
        public ImmutableArray<SpecialQualityDefinition> SpecialQualities { get; set; }
        /// <summary>
        /// Gets or sets the requirement to take this item
        /// </summary>
        /// <remarks>
        /// This property can refer to the same <see cref="IConditional"/> object as <see cref="ToApply"/>,
        /// which basically allows it to have the same requirement for both
        /// </remarks>
        public IConditional ToTakeRequirement { get; set; }
        /// <summary>
        /// Gets or sets hte requirements to apply this item
        /// </summary>
        /// <remarks>
        /// This property can refer to the same <see cref="IConditional"/> object as <see cref="ToTakeRequirement"/>,
        /// which basically allows it to have the same requirement for both
        /// </remarks>
        public IConditional ToApply { get; set; }
        /// <summary>
        /// Gets the internal ID for this definition
        /// </summary>
        public Guid Id { get; private set; }
        #endregion
    }
}
