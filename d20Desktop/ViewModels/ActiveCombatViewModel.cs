using Fiction.GameScreen.Combat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for an active combat
    /// </summary>
    public sealed class ActiveCombatViewModel : CampaignViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ActiveCombatViewModel"/>
        /// </summary>
        /// <param name="factory">Factory for creating view models</param>
        /// <param name="combat">Combat this view model is for</param>
        public ActiveCombatViewModel(IViewModelFactory factory, ActiveCombat combat)
            : base(factory)
        {
            Combat = combat;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the combat associated with this view model
        /// </summary>
        public ActiveCombat Combat { get; private set; }
        /// <summary>
        /// Gets the combatants in this combat
        /// </summary>
        public ReadOnlyObservableCollection<ICombatant> Combatants { get { return Combat.Combatants; } }

        /// <summary>
        /// Gets whether or not this is a valid combat
        /// </summary>
        public override bool IsValid => true;
        /// <summary>
        /// Gets the category for this view model
        /// </summary>
        public override string ViewModelCategory { get { return Resources.Resources.CombatCategory; } }
        /// <summary>
        /// Gets the display name for this view model
        /// </summary>
        public override string ViewModelDisplayName { get { return Combat.Name; } }
        /// <summary>
        /// Gets whether or not information is being sent to a server
        /// </summary>
        #endregion
        #region Methods
        /// <summary>
        /// Backs up the combat and goes to the next combatant
        /// </summary>
        /// <returns>Task for asynchronous completion</returns>
        public async Task<GotoNextResult> GotoNext()
        {
            await Combat.Backup();
            return Combat.GotoNext();
        }

        #endregion
    }
}
