using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Contains the definition of an attribute on a character
    /// </summary>
    public sealed class AttributeDefinition
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="AttributeDefinition"/>
        /// </summary>
        /// <param name="id">ID of this attribute definition</param>
        /// <param name="name">Name of the attribute</param>
        /// <param name="type">Type of attribute</param>
        /// <param name="modifierType">Type of modifier created by this attribute</param>
        public AttributeDefinition(object id, string name, string type, AttributeModifierType modifierType)
        {
            Exceptions.ThrowIfArgumentNullOrEmpty(name, nameof(name));
            Exceptions.ThrowIfArgumentNullOrEmpty(type, nameof(type));

            Name = name;
            AttributeType = type;
            ModifierType = modifierType;
            Id = id;
        }
        /// <summary>
        /// Constructs a new <see cref="AttributeDefinition"/> that's a system attribute
        /// </summary>
        /// <param name="id">ID of this attribute definition</param>
        /// <param name="name">Name of the attribute</param>
        /// <param name="type">Type of attribute</param>
        /// <param name="modifierType">Type of modifier created by this attribute</param>
        /// <param name="isSystem">Whether or not this is a system attribute</param>
        public AttributeDefinition(object id, string name, string type, AttributeModifierType modifierType, bool isSystem)
            :this(id, name, type, modifierType)
        {
            IsSystem = isSystem;
        }
        /// <summary>
        /// Initializes the <see cref="AttributeDefinition"/> class
        /// </summary>
        static AttributeDefinition()
        {
            ClassLevelType = Resources.Resources.ClassLevelAttributeName;

            Dictionary<DieType, AttributeDefinition> attributes = Enum.GetValues(typeof(DieType))
                .OfType<DieType>()
                .ToDictionary(p => p, GenerateHitDieAttribute);

            HitDieAttributes = new ReadOnlyDictionary<DieType, AttributeDefinition>(attributes);

            //  Generates attribute for hit die
            AttributeDefinition GenerateHitDieAttribute(DieType dieType)
            {
                string name = string.Format(CultureInfo.CurrentCulture, Resources.Resources.HitDiceByType, dieType);
                return new AttributeDefinition(
                    name,
                    name,
                    Resources.Resources.HitDieAttributeType,
                    AttributeModifierType.Value,
                    true);
            }

            TotalLevelAttribute = new AttributeDefinition(Resources.Resources.TotalLevelAttributeName, Resources.Resources.TotalLevelAttributeName, Resources.Resources.HitDieAttributeType, AttributeModifierType.Value);
            TotalHitDiceAttribute = new AttributeDefinition(Resources.Resources.TotalHitDiceAttributeName, Resources.Resources.TotalHitDiceAttributeName, Resources.Resources.HitDieAttributeType, AttributeModifierType.Value);
        }
        #endregion
        #region Member Variables
        /// <summary>
        /// Type of attribute for class levels
        /// </summary>
        public static readonly string ClassLevelType;
        /// <summary>
        /// Attribute definitions for various hit die types
        /// </summary>
        public static readonly IReadOnlyDictionary<DieType, AttributeDefinition> HitDieAttributes;
        /// <summary>
        /// Attribute definition for total class levels
        /// </summary>
        public static readonly AttributeDefinition TotalLevelAttribute;
        /// <summary>
        /// Attribute definition for total character hit dice (racial + class levels)
        /// </summary>
        public static readonly AttributeDefinition TotalHitDiceAttribute;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the ID of this AttributeDefinition
        /// </summary>
        public object Id { get; set; }
        /// <summary>
        /// Gets or sets the name of the definition
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets how the modifier for the attribute is calculated
        /// </summary>
        public AttributeModifierType ModifierType { get; set; }
        /// <summary>
        /// Gets or sets the type of attribute (case-insensitive)
        /// </summary>
        public string AttributeType { get; set; }
        /// <summary>
        /// Gets whether or not this is a system level attribute that can't be modified or removed
        /// </summary>
        public bool IsSystem { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Gets a modifier value for a given attribute score
        /// </summary>
        /// <param name="value">Value to calculate into a modifier</param>
        /// <returns>Modifier calculated from the given value</returns>
        public int CalculateModifier(int value)
        {
            //  Either it's just the value
            if (ModifierType == AttributeModifierType.Value)
                return value;

            //  Or its calculated
            return value / 2 - 5;
        }

        /// <summary>
        /// Determines whether this object is equivalent to another object
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Whether or not the two objects are equivalent</returns>
        public override bool Equals(object obj)
        {
            if (obj is AttributeDefinition definition)
                return string.Equals(Name, definition.Name);

            return false;
        }
        /// <summary>
        /// Determines whether two attribute definitions are equivalent
        /// </summary>
        /// <param name="definition1">First definition to compare</param>
        /// <param name="definition2">Second definition to compare</param>
        /// <returns>Whether or not the two definitions are equivalent</returns>
        public static bool Equals(AttributeDefinition definition1, AttributeDefinition definition2)
        {
            if (definition1 != null)
                return definition1.Equals(definition2);

            if (definition2 == null)
                return true;

            return false;
        }
        /// <summary>
        /// Gets a hash code to use for this object
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        #endregion
        #region Events
#pragma warning disable 67
        /// <summary>
        /// Event that is triggered when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67
        #endregion
    }
}
