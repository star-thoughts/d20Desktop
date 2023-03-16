using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Modifier for a single attribute
    /// </summary>
    public abstract class AttributeModifier
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="AttributeModifier"/>
        /// </summary>
        /// <param name="targets">Target attribute for this modifier</param>
        protected AttributeModifier(ModifierType type, params object[] targets)
            : this(type, (IEnumerable<object>)targets)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="AttributeModifier"/>
        /// </summary>
        /// <param name="targets">Target attribute for this modifier</param>
        protected AttributeModifier(ModifierType type, IEnumerable<object> targets)
        {
            Exceptions.ThrowIfArgumentNull(targets, nameof(targets));
            Exceptions.ThrowIfArgumentNull(type, nameof(type));

            if (!targets.Any())
                throw new ArgumentException("There must be at least 1 target specified.", nameof(targets));

            ModifierTypeId = type.Id;
            TargetAttributes = ImmutableArray.CreateRange(targets);
        }
        /// <summary>
        /// Initializes the <see cref="AttributeModifier"/> class
        /// </summary>
        static AttributeModifier()
        {
            TotalHitDiceFromLevels = new AttributeModifierFromAttribute(AttributeDefinition.TotalLevelAttribute, ModifierType.ClassLevelModifierType, AttributeDefinition.TotalHitDiceAttribute) { IsSystem = true };
        }
        #endregion
        #region Member Variables
        /// <summary>
        /// Modifier used for adding the total class levels to the total hit dice of a character
        /// </summary>
        public static readonly AttributeModifierFromAttribute TotalHitDiceFromLevels;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the ID of this modifier
        /// </summary>
        public object Id { get; set; }
        /// <summary>
        /// Gets or sets the type of modifier this is for
        /// </summary>
        public object ModifierTypeId { get; set; }
        /// <summary>
        /// Gets the ID of the <see cref="AttributeDefinition"/>s for the attributes being targetted by this modifier
        /// </summary>
        public ImmutableArray<object> TargetAttributes { get; private set; }
        /// <summary>
        /// Gets or sets whether or not this modifier stacks with all others
        /// </summary>
        /// <remarks>
        /// If the value is <see cref="ModifierStackingType.Default"/> then whether or not it stacks depends on the <see cref="ModifierSource"/>.
        /// </remarks>
        public ModifierStackingType Stacks { get; set; }
        /// <summary>
        /// Gets or sets the source of this modifier.   If null, then it is the campaign or the character.
        /// </summary>
        public Definition ModifierSource { get; set; }
        /// <summary>
        /// Gets whether or not this is a system level attribute modifier
        /// </summary>
        public bool IsSystem { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Calculates the total modifier value from this <see cref="AttributeModifier"/>
        /// </summary>
        /// <param name="data">Data to use to calculate the total value</param>
        /// <returns>Calculated total value</returns>
        public abstract Task<CalculatedAttributeModifier> CalculateModifierAsync(AttributeCalculationData data);
        #endregion
    }
}
