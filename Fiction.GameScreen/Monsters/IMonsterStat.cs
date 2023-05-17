namespace Fiction.GameScreen.Monsters
{
    /// <summary>
    /// Base interface for a single stat
    /// </summary>
    public interface IMonsterStat
    {
        /// <summary>
        /// Gets the name of the stat
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets the value of the stat
        /// </summary>
        object? Value { get; }
        /// <summary>
        /// Copies these stats to a server representation of stats
        /// </summary>
        /// <returns>Server representation</returns>
        d20Web.Models.Bestiary.MonsterStat ToServerMonsterStat();
    }
}