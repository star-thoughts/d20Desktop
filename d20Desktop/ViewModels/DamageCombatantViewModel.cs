using Fiction.GameScreen.Combat;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    public sealed class DamageCombatantViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="DamageCombatantViewModel"/>
        /// </summary>
        /// <param name="combat">Combat this damage applies to</param>
        public DamageCombatantViewModel(ActiveCombatViewModel combat)
        {
            Combat = combat;
            Damage = new DamageInformation();
            Damage.Combatants.CollectionChanged += Combatants_CollectionChanged;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the currently active combat
        /// </summary>
        public ActiveCombatViewModel Combat { get; private set; }
        /// <summary>
        /// Gets information about the damage being dealt
        /// </summary>
        public DamageInformation Damage { get; private set; }
        /// <summary>
        /// Gets whether or not the data in this view model is valid
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return Damage.Combatants.Any();
            }
        }
        /// <summary>
        /// Gets whether or not any of the combatants selected has a damage modifier
        /// </summary>
        public bool HasDamageModifiers
        {
            get
            {
                return Damage.Combatants.Any(p => p.Combatant.DamageReduction.Any());
            }
        }
        #endregion
        #region Methods

        private void Combatants_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.RaisePropertiesChanged(nameof(IsValid), nameof(HasDamageModifiers));
        }
        #endregion
    }
}
