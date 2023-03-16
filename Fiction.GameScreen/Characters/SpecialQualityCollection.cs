using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Maintains a collection of special qualities applied to a character
    /// </summary>
    public sealed class SpecialQualityCollection
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SpecialQualityCollection"/>
        /// </summary>
        public SpecialQualityCollection()
        {
            Qualities = ImmutableArray<CalculatedSpecial>.Empty;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets a collection of special qualities
        /// </summary>
        public ImmutableArray<CalculatedSpecial> Qualities { get; set; }
        #endregion
        #region Methods
        /// <summary>
        /// Gets a collection of <see cref="CalculatedSpecial"/> that have requirements for applying modifiers
        /// </summary>
        /// <returns>Collection of Special Qualities that have apply requirements</returns>
        public CalculatedSpecial[] GetQualitiesWithApplyRequirements()
        {
            return Qualities
                .Where(p => p.HasApplyRequirements)
                .ToArray();
        }
        #endregion
    }
}
