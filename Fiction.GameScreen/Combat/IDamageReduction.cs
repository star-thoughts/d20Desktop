using System.Collections.ObjectModel;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Base interface used for damage reduction
    /// </summary>
    public interface IDamageReduction
    {
        /// <summary>
        /// Gets or sets the amount of damage reduction
        /// </summary>
        int Amount { get; set; }
        /// <summary>
        /// Gets or sets whether or not all types must be present in the damage to overcome the damage reduction
        /// </summary>
        bool RequiresAllTypes { get; set; }
        /// <summary>
        /// Gets a collection of types to overcome damage reduction
        /// </summary>
        ObservableCollection<string> Types { get; }
    }
}