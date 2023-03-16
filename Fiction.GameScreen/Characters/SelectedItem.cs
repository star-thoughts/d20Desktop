using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Base class for selected items
    /// </summary>
    /// <typeparam name="T">Type of definition this selection is for</typeparam>
    public abstract class SelectedItem<T> : ISelectedItem where T : Definition
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SelectedItem{T}"/>
        /// </summary>
        /// <param name="definition">Definition that was selected</param>
        /// <param name="qualities">Information about the special qualities selected</param>
        public SelectedItem(T definition, params SelectedSpecialQuality[] qualities)
        {
            Exceptions.ThrowIfArgumentNull(definition, nameof(definition));
            Exceptions.ThrowIfArgumentNull(qualities, nameof(qualities));

            Definition = definition;
            SpecialQualities = ImmutableArray.CreateRange(qualities);
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the definition of the selected item
        /// </summary>
        public T Definition { get; private set; }
        /// <summary>
        /// Gets or sets the definition of this selected item
        /// </summary>
        Definition ISelectedItem.Definition { get { return this.Definition; } set { this.Definition = value as T; } }
        /// <summary>
        /// Gets or sets the special qualities that were selected for this
        /// </summary>
        public ImmutableArray<SelectedSpecialQuality> SpecialQualities { get; set; }
        /// <summary>
        /// Gets whether or not this item has requirements to apply any bonuses
        /// </summary>
        public bool HasApplyRequirement { get { return Definition.ToApply != null || (Owner != null && Owner.HasApplyRequirement); } }
        /// <summary>
        /// Gets or sets the selected item that owns this item (for example, a feat giving a special quality)
        /// </summary>
        public ISelectedItem Owner { get; set; }
        #endregion
        #region Methods
        /// <summary>
        /// Gets all of the attribute modifiers for a given attribute
        /// </summary>
        /// <param name="data">Data used to determine which modifiers to use</param>
        /// <returns>Collection of modifiers for the given attribute</returns>
        public virtual AttributeModifier[] GetAttributeModifiers(AttributeCalculationData data)
        {
            return Definition.Modifiers.ToArray();
        }
        /// <summary>
        /// Determinse whether or not this selected item's apply conditions are met
        /// </summary>
        /// <param name="character">Character to test against</param>
        /// <returns>Whether or not the conditions are met</returns>
        public async Task<bool> ApplyConditionMetAsync(Character character)
        {
            bool result = true;

            if (Owner != null && Owner.HasApplyRequirement)
                result = await Owner.ApplyConditionMetAsync(character);

            if (HasApplyRequirement)
                result = result && await Definition.ToApply.ConditionMetAsync(character);

            return result;
        }
        #endregion

    }
}
