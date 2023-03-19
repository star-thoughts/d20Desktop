using Fiction.GameScreen.Combat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for handling the prepartion of combat
    /// </summary>
    public sealed class PrepareCombatViewModel : CampaignViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="PrepareCombatViewModel"/>
        /// </summary>
        /// <param name="factory">View model factory</param>
        public PrepareCombatViewModel(IViewModelFactory factory)
            : base(factory)
        {
            Preparer = new CombatPreparer(factory.Campaign);
            Combatants.CollectionChanged += Combatants_CollectionChanged;
        }/// <summary>
         /// Constructs a new <see cref="PrepareCombatViewModel"/> to add to an existing combat
         /// </summary>
         /// <param name="factory">View model factory</param>
         /// <param name="activeCombat">Combat to add to</param>
        public PrepareCombatViewModel(IViewModelFactory factory, ActiveCombatViewModel activeCombat)
            : base(factory)
        {
            Preparer = new CombatPreparer(factory.Campaign, activeCombat.Combat);
            Combatants.CollectionChanged += Combatants_CollectionChanged;
        }
        #endregion
        #region Member Variables
        #endregion
        #region Properties
        /// <summary>
        /// Gets the underlying combat preparer
        /// </summary>
        public CombatPreparer Preparer { get; private set; }
        /// <summary>
        /// Gets a collection of combat scenarios available for including in the combat
        /// </summary>
        public IEnumerable<CombatScenario> Scenarios { get { return Factory.Campaign.Combat.Scenarios; } }
        /// <summary>
        /// Gets a collection of combatants to begin combat with
        /// </summary>
        public ObservableCollection<CombatantPreparer> Combatants { get { return Preparer.Combatants; } }
        /// <summary>
        /// Gets the category for this view model
        /// </summary>
        public override string ViewModelCategory { get { return Resources.Resources.CombatCategory; } }

        /// <summary>
        /// Gets the display name for this view model
        /// </summary>
        public override string ViewModelDisplayName { get { return Preparer.SourceCombat == null ? Resources.Resources.BeginCombatDisplayName : Resources.Resources.AddToCombatDisplayName; } }
        /// <summary>
        /// Gets whether or not the values of this view model are valid
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return Preparer.SourceCombat == null
                    ? Combatants.Count > 1
                    : Combatants.Any();
            }
        }

        private void Combatants_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.RaisePropertyChanged(nameof(IsValid));
        }

        /// <summary>
        /// Resets the combatant list, adding players if necessary
        /// </summary>
        public void Reset()
        {
            Preparer.Reset();
        }

        #endregion
    }
}
