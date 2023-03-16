using Fiction.GameScreen.Monsters;
using System.Windows;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Routed event args for monster related events
    /// </summary>
    public class MonsterEventArgs : RoutedEventArgs
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="MonsterEventArgs"/>
        /// </summary>
        /// <param name="routedEvent">Routed event that is being raised</param>
        /// <param name="source">Source of the event</param>
        /// <param name="monster">Monster that the event is being raised for</param>
        public MonsterEventArgs(RoutedEvent routedEvent, object source, Monster monster)
            : base(routedEvent, source)
        {
            Monster = monster;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the monster that the event is for
        /// </summary>
        public Monster Monster { get; private set; }
        #endregion
    }
}