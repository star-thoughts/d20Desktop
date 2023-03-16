using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Base interface for a selection (such as a quality, feat, class level, etc)
    /// </summary>
    public interface ISelectedItem : IModifierContainer
    {
        Definition Definition { get; set; }
        /// <summary>
        /// Gets or sets the special qualities that were selected for this
        /// </summary>
        ImmutableArray<SelectedSpecialQuality> SpecialQualities { get; }
        /// <summary>
        /// Gets whether or not the selected item has a requirement for it's definition
        /// </summary>
        bool HasApplyRequirement { get; }
        /// <summary>
        /// Gets a selected item that owns this item (for example, a special quality given by a feat)
        /// </summary>
        ISelectedItem Owner { get; }
        /// <summary>
        /// Determinse whether or not this selected item's apply conditions are met
        /// </summary>
        /// <param name="character">Character to test against</param>
        /// <returns>Whether or not the conditions are met</returns>
        Task<bool> ApplyConditionMetAsync(Character character);
    }
}
