using System.Threading.Tasks;

namespace Fiction.GameScreen.Characters
{
    /// <summary>
    /// Base interface for managing special qualities
    /// </summary>
    public interface ISpecialsManager
    {
        /// <summary>
        /// Rechecks all special qualities to make sure they are still apply-able
        /// </summary>
        /// <param name="items">Items that need recalculating</param>
        /// <param name="character">Character they should apply to</param>
        /// <returns>Task for task completion</returns>
        Task ReexamineQualities(CalculatedSpecial[] items, Character character);
    }
}