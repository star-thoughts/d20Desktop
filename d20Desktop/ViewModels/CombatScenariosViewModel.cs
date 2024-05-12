using Fiction.GameScreen.Combat;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

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
            _scenarios = Campaign.Combat.Scenarios;
            ((INotifyCollectionChanged)_scenarios).CollectionChanged += CombatScenariosViewModel_CollectionChanged;
        }
        #endregion
        #region Member Variables
        private ObservableCollection<CombatScenario> _scenarios;
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the campaign associated with this view model
        /// </summary>
        public CampaignSettings Campaign { get; private set; }
        /// <summary>
        /// Gets a collectio of combat scenarios
        /// </summary>
        public ObservableCollection<CombatScenario> Scenarios { get { return _scenarios; } }
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
        private async void CombatScenariosViewModel_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            await Exceptions.FailSafeMethodCall(async () =>
            {
                Server.ICombatManagement? server = Factory.GetCombatManagement();
                if (e.OldItems != null
                    && server != null
                    && !string.IsNullOrEmpty(Campaign.CampaignID))
                {
                    foreach (CombatScenario scenario in e.OldItems)
                    {
                        if (!string.IsNullOrEmpty(scenario.ServerID))
                            await server.DeleteCombatScenario(Campaign.CampaignID, scenario.ServerID);
                    }
                }
            });
        }
        #endregion
    }
}
