using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Base interface with common elements
    /// </summary>
    public interface ICombatantBase : ICampaignObject
    {
        /// <summary>
        /// Gets or sets the name of this combatant
        /// </summary>
        new string Name { get; set; }
        /// <summary>
        /// Gets or sets the name of the combatant
        /// </summary>
        bool DisplayToPlayers { get; set; }
        /// <summary>
        /// Gets or sets the name to display to players
        /// </summary>
        string DisplayName { get; set; }
        /// <summary>
        /// Gets a collection of damage reductions that apply to this combatant
        /// </summary>
        ObservableCollection<DamageReduction> DamageReduction { get; }
    }
}
