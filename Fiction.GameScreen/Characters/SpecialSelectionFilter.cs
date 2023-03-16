using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Contains information about what special abilities are available for selection when a choice can be made
    /// </summary>
    /// <remarks>
    /// The special selection filter contains various requirements for the filter, if any of them match then the special item is
    /// included.  If no filters are set, then all items of the given type are allowed.
    /// </remarks>
    public sealed class SpecialSelectionFilter
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SpecialSelectionFilter"/>
        /// </summary>
        public SpecialSelectionFilter()
        {
            Specials = ImmutableArray<SpecialDefinition>.Empty;
            Tags = ImmutableArray<string>.Empty;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the type(s) of specials allowed for this selection
        /// </summary>
        public SpecialDefinitionType SpecialType { get; set; }
        /// <summary>
        /// Gets or sets the list of specials that are allowed
        /// </summary>
        public ImmutableArray<SpecialDefinition> Specials { get; set; }
        /// <summary>
        /// Gets or sets a collection of tags that are allowed
        /// </summary>
        /// <remarks>
        /// A special is allowed if any of the tags on the special are contained in this collection of tags.
        /// </remarks>
        public ImmutableArray<string> Tags { get; set; }
        public bool IsEmpty
        {
            get
            {
                return Specials.IsEmpty && Tags.IsEmpty;
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Filters out the collection of special definitions, returning only those allowed
        /// </summary>
        /// <param name="definitions">Definitions to consider</param>
        /// <returns>Filtered list of definitions, with only those allowed contained</returns>
        /// <remarks>
        /// This filters out the given specials.  This filtering doesn't include comparing based on <see cref="SpecialType"/>.
        /// After the items are filtered out, the guaranteed items from the <see cref="Specials"/> list is included as well.
        /// </remarks>
        public SpecialDefinition[] GetAllowedSpecials(params SpecialDefinition[] definitions)
        {
            return GetAllowedSpecials((IEnumerable<SpecialDefinition>)definitions);
        }
        /// <summary>
        /// Filters out the collection of special definitions, returning only those allowed
        /// </summary>
        /// <param name="definitions">Definitions to consider</param>
        /// <returns>Filtered list of definitions, with only those allowed contained</returns>
        /// <remarks>
        /// <para>This filters out the given specials.  This filtering doesn't include comparing based on <see cref="SpecialType"/>.  They
        /// should already be filtered by type.</para>
        /// <para>After the items are filtered out, the guaranteed items from the <see cref="Specials"/> list is included as well.</para>
        /// </remarks>
        public SpecialDefinition[] GetAllowedSpecials(IEnumerable<SpecialDefinition> definitions)
        {
            if (IsEmpty)
                return definitions.ToArray();

            return definitions
                //  Check tags
                .Where(p => p.Tags.Intersect(Tags).Any())
                //  Append list that is always present
                .Concat(Specials)
                //  Make sure items only appear a single time
                .Distinct()
                .ToArray();
        }
        #endregion
    }
}
