using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for bypassing all damage modifiers
    /// </summary>
    public sealed class BypassDamageModifiersViewModel : ViewModelCore, IDamageModifiersViewModel
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="BypassDamageModifiersViewModel"/>
        /// </summary>
        /// <param name="damage"></param>
        public BypassDamageModifiersViewModel(DamageCombatantViewModel damage)
        {
            Damage = damage;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the damage view model
        /// </summary>
        public DamageCombatantViewModel Damage { get; }
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid => true;
        #endregion
        #region Methods
        /// <summary>
        /// Applies this damage modifier to the damage information
        /// </summary>
        public void Apply()
        {
            Damage.Damage.BypassDamageReduction = true;
        }
        #endregion
    }
}
