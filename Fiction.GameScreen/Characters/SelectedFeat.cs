using System;
using System.Collections.Generic;
using System.Text;

namespace Fiction.GameScreen.Characters
{
    public sealed class SelectedFeat : SelectedItem<FeatDefinition>
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SelectedFeat"/>
        /// </summary>
        /// <param name="definition">Feat chosen</param>
        /// <param name="qualities">Information about special qualities associated with the feat</param>
        public SelectedFeat(FeatDefinition definition, params SelectedSpecialQuality[] qualities)
            : base(definition, qualities)
        {
        }
        #endregion
    }
}
