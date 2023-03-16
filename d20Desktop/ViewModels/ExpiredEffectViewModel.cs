using Fiction.GameScreen.Combat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for displaying an expired effect during combat
    /// </summary>
    public sealed class ExpiredEffectViewModel
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ExpiredEffectViewModel"/>
        /// </summary>
        /// <param name="initiativeCombatant">Combatant who's initiative it expired on</param>
        /// <param name="effect">Effect that expired</param>
        public ExpiredEffectViewModel(ICombatant initiativeCombatant, Effect effect)
        {
            Combatant = initiativeCombatant;
            Effect = effect;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the combatant who's initiative it expired on
        /// </summary>
        public ICombatant Combatant { get; private set; }
        /// <summary>
        /// Gets the effect that expired
        /// </summary>
        public Effect Effect { get; private set; }
        #endregion
    }
}
