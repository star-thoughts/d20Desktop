using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for managing the combatants in a combat, other than normal combat stuff
    /// </summary>
    public sealed class ManageCombatantsViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ManageCombatantsViewModel"/>
        /// </summary>
        /// <param name="combat">Combat to manage</param>
        public ManageCombatantsViewModel(ActiveCombatViewModel combat)
        {
            Combat = combat;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the combat being managed
        /// </summary>
        public ActiveCombatViewModel Combat { get; private set; }
        /// <summary>
        /// Gets whether or not the data in this view model is valid
        /// </summary>
        public override bool IsValid => true;
        #endregion
    }
}
