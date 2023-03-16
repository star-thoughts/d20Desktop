using Fiction.GameScreen.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for handling healing of combatants during combat
    /// </summary>
    public sealed class HealCombatantsViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="HealCombatantsViewModel"/>
        /// </summary>
        /// <param name="combat">Combat that healing is taking place in</param>
        public HealCombatantsViewModel(ActiveCombatViewModel combat)
        {
            Combat = combat;
            Healing = new HealInformation();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the combat associated with this healing
        /// </summary>
        public ActiveCombatViewModel Combat { get; private set; }
        /// <summary>
        /// Gets the healing information
        /// </summary>
        public HealInformation Healing { get; private set; }
        /// <summary>
        /// Gets whether or not this healing data is valid
        /// </summary>
        public override bool IsValid => true;
        #endregion
        #region Methods
        /// <summary>
        /// Applies healing
        /// </summary>
        public void Apply()
        {
            Combat.Combat.ApplyHealing(Healing);
        }
        #endregion
    }
}
