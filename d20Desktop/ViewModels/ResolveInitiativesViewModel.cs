using Fiction.GameScreen.Combat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for resolving initiative ties
    /// </summary>
    public sealed class ResolveInitiativesViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ResolveInitiativesViewModel"/>
        /// </summary>
        /// <param name="preparer">Combat preparer with combat information</param>
        public ResolveInitiativesViewModel(PrepareCombatViewModel preparer)
        {
            Preparer = preparer;
            //  Go ahead and let the machine do anything it can up front
            preparer.Preparer.ResolveInitiatives();
        }
        #endregion
        #region Properties
        /// <summary>
        /// The combat preparer containing combatant information
        /// </summary>
        public PrepareCombatViewModel Preparer { get; private set; }
        /// <summary>
        /// Gets a collection of combatants to resolve initiatives for
        /// </summary>
        public ObservableCollection<CombatantPreparer> Combatants { get { return Preparer.Combatants; } }
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid => true;
        #endregion
    }
}
