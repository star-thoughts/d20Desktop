using System.Collections.Generic;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Interface for objects that contain a collection of objects with an initiative order
    /// </summary>
    public interface IInitiativeCollection
    {
        IEnumerable<IActiveCombatant> Combatants { get; }
    }
}