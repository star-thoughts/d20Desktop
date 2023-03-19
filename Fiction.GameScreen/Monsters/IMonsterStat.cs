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
    }
}