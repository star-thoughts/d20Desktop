using System.IO;
using System.Threading.Tasks;
using Fiction.GameScreen.Combat;

namespace Fiction.GameScreen.Serialization
{
    /// <summary>
    /// Interface for backing up and restoring combat for undo/redo operations
    /// </summary>
    public interface IXmlActiveCombatSerializer
    {
        /// <summary>
        /// Backs up the combat and returns the backup in a Stream
        /// </summary>
        /// <param name="combat">Combat to back up</param>
        /// <returns>Stream with backup</returns>
        Task<Stream> Backup(ActiveCombat combat);
        /// <summary>
        /// Restores combat from a backup
        /// </summary>
        /// <param name="stream">Stream containing backup</param>
        /// <param name="combat">Combat to restore to</param>
        void Restore(Stream stream, ActiveCombat combat);
    }
}