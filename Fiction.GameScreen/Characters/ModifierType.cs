using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Information about a type of modifier
    /// </summary>
    public sealed class ModifierType
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ModifierType"/>
        /// </summary>
        /// <param name="type">Name of the modifier type</param>
        /// <param name="stacks">Whether or not the type stacks by default</param>
        public ModifierType(string type, bool stacks)
        {
            TypeName = type;
            Stacks = stacks ? ModifierStackingType.Stacking : ModifierStackingType.NonStacking;
            Id = type;
        }
        /// <summary>
        /// Initializes the <see cref="ModifierType"/>
        /// </summary>
        static ModifierType()
        {
            ClassLevelModifierType = new ModifierType(Resources.Resources.ClassLevelModifierType, true) { IsSystem = true };
            RacialModifierType = new ModifierType(Resources.Resources.RacialModifierType, true) { IsSystem = true };
        }
        #endregion
        #region Member Variables
        /// <summary>
        /// Modifier type used for determining class levels
        /// </summary>
        public static readonly ModifierType ClassLevelModifierType;
        /// <summary>
        /// Modifier type used for racial bonuses
        /// </summary>
        public static readonly ModifierType RacialModifierType;
        #endregion
        #region Properties
        /// <summary>
        /// Gets the ID of the modifier type
        /// </summary>
        public object Id { get; set; }
        /// <summary>
        /// Gets or sets the name of the type
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// Gets or sets whether or not this type stacks by default with other bonuses of the same type
        /// </summary>
        /// <remarks>
        /// If this value is <see cref="ModifierStackingType.Default"/> then it defaults to <see cref="ModifierStackingType.NonStacking"/>.
        /// </remarks>
        public ModifierStackingType Stacks { get; set; }
        /// <summary>
        /// Gets whether or not this is a system modifier type
        /// </summary>
        public bool IsSystem { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Determines whether two modifier types are considered equal
        /// </summary>
        /// <param name="type1">First type to consider</param>
        /// <param name="type2">Seconds type to consider</param>
        /// <returns>Whether or not the two types are equal</returns>
        public static bool Equals(ModifierType type1, ModifierType type2)
        {
            if (type1 == null && type2 == null)
                return true;
            if (type1 == null)
                return false;
            if (type2 == null)
                return false;
            return type1.Equals(type2);
        }
        /// <summary>
        /// Determines whether this object is the same as another
        /// </summary>
        /// <param name="obj">Other object to compare to</param>
        /// <returns>Whether or not they are equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is ModifierType type)
                return TypeName.Equals(type.TypeName);
            return false;
        }

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns>Hash code for this object</returns>
        public override int GetHashCode()
        {
            return TypeName.GetHashCode();
        }
        #endregion
    }
}
