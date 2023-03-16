namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// Base interface for applying damage modification information
    /// </summary>
    public interface IDamageModifiersViewModel
    {
        /// <summary>
        /// Applies this damage modifier to the damage information
        /// </summary>
        void Apply();
    }
}