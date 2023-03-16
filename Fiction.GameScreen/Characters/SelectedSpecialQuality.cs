namespace Fiction.GameScreen.Characters
{
    public sealed class SelectedSpecialQuality : SelectedItem<SpecialQualityDefinition>
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SelectedSpecialQuality"/>
        /// </summary>
        /// <param name="definition">Definition for the quality that was selected</param>
        public SelectedSpecialQuality(SpecialQualityDefinition definition)
            : base(definition)
        {
        }
        #endregion
    }
}