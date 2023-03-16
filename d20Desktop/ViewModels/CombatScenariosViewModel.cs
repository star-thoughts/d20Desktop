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
    /// View model for managing combat scenarios
    /// </summary>
    public sealed class CombatScenariosViewModel : CampaignViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CombatScenariosViewModel"/>
        /// </summary>
        /// <param name="factory">Factory for view models</param>
        /// <param name="campaign">Campaign to edit scenarios for</param>
        public CombatScenariosViewModel(IViewModelFactory factory, CampaignSettings campaign)
            : base(factory)
        {
            Campaign = campaign;
        }
        #endregion
        #region Member Variables
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the campaign associated with this view model
        /// </summary>
        public CampaignSettings Campaign { get; private set; }
        /// <summary>
        /// Gets a collectio of combat scenarios
        /// </summary>
        public ObservableCollection<CombatScenario> Scenarios { get { return Campaign.Combat.Scenarios; } }
        /// <summary>
        /// Gets the category for this view model
        /// </summary>
        public override string ViewModelCategory { get { return Resources.Resources.CombatCategory; } }
        /// <summary>
        /// Gets the display name for this view model
        /// </summary>
        public override string ViewModelDisplayName { get { return Resources.Resources.CombatScenariosViewModelDisplayName; } }
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid { get { return true; } }
        #endregion
        #region Methods
        #endregion
    }
}
