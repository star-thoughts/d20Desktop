using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Combatant in an active combat
    /// </summary>
    public interface ICombatant : IActiveCombatant
    {
        #region Properties
        /// <summary>
        /// Gets the source information for this combatant
        /// </summary>
        ICombatantTemplate Source { get; }
        /// <summary>
        /// Gets the information used to prepare this combatant for combat
        /// </summary>
        CombatantPreparer PreparedInfo { get; }
        /// <summary>
        /// Gets health information for the combatant
        /// </summary>
        CombatantHealth Health { get; }
        /// <summary>
        /// Gets or sets whether this is the current combatant
        /// </summary>
        bool IsCurrent { get; set; }
        /// <summary>
        /// Gets or sets whether the combatant has gone at least one time in combat
        /// </summary>
        bool HasGoneOnce { get; set; }
        /// <summary>
        /// Gets or sets whether or not the combatant should be included in combat
        /// </summary>
        bool IncludeInCombat { get; set; }
        /// <summary>
        /// Gets whether or not the players should be able to see this combatant
        /// </summary>
        bool CanPlayersSee { get; }
        /// <summary>
        /// Gets whether or not this combatant is for a player
        /// </summary>
        bool IsPlayer { get; }
        /// <summary>
        /// Gets a collection of conditions applied to this combatant
        /// </summary>
        ObservableCollection<AppliedCondition> Conditions { get; }
        /// <summary>
        /// Handles all beginning of turn events and determines whether the combatant can take a turn
        /// </summary>
        /// <param name="settings">Settings for the combat</param>
        /// <returns>Whether or not the combatant can take a turn</returns>
        bool TryBeginTurn(CombatSettings settings);
        #endregion
    }
}
