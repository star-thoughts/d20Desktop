using Fiction.GameScreen.Combat;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    public sealed class CombatScenarioEditViewModel : CampaignViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CombatScenarioEditViewModel"/>
        /// </summary>
        /// <param name="factory">Factory for view model generation</param>
        /// <param name="scenario">Scenario to edit</param>
        public CombatScenarioEditViewModel(IViewModelFactory factory, CombatScenario scenario)
            : base(factory)
        {
            _scenario = scenario;

            _name = _scenario.Name;
            _group = _scenario.Group;
            _details = _scenario.Details;
            Combatants = _scenario.Combatants.Select(p => new CombatantTemplateEditViewModel(p))
                .ToObservableCollection();
            _combatantsMonitor = new CollectionMonitor(Combatants);
            _combatantsMonitor.PropertyChanged += _combatantsMonitor_PropertyChanged;
        }
        #endregion
        #region Member Variables
        private CombatScenario _scenario;
        private CollectionMonitor _combatantsMonitor;
        #endregion
        #region Properties
        /// <summary>
        /// Gets the campaign associated with the scenario
        /// </summary>
        public CampaignSettings Campaign { get { return _scenario.Campaign; } }
        private string _name;
        /// <summary>
        /// Gets or sets the name of the scenario
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.Equals(_name, value))
                {
                    _name = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _group;
        /// <summary>
        /// Gets or sets the group this scenario is in
        /// </summary>
        public string? Group
        {
            get { return _group; }
            set
            {
                if (!string.Equals(_group, value))
                {
                    _group = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private string? _details;
        /// <summary>
        /// Gets or sets any extra details for the scenario
        /// </summary>
        public string? Details
        {
            get { return _details; }
            set
            {
                if (!string.Equals(_details, value))
                {
                    _details = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets a collection of combatants
        /// </summary>
        public ObservableCollection<CombatantTemplateEditViewModel> Combatants { get; private set; }
        /// <summary>
        /// Gets the category for this view model
        /// </summary>
        public override string ViewModelCategory
        {
            get { return Resources.Resources.CombatCategory; }
        }
        /// <summary>
        /// Gets the display name for this view model
        /// </summary>
        public override string ViewModelDisplayName
        {
            get { return Name; }
        }
        /// <summary>
        /// Gets whether or not this view model's data is valid
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Name)
                    && Combatants.Any()
                    && Combatants.All(p => p.IsValid);
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Updates the combat scenario with this data
        /// </summary>
        /// <returns>Combat scenario saved</returns>
        public async Task<CombatScenario> Save()
        {
            ICombatantTemplate[] combatants = Combatants.Select(p => p.Save()).ToArray();
            _scenario.Name = Name;
            _scenario.Group = Group;
            _scenario.Details = Details;

            GetCombatantChanges(_scenario.Combatants.ToArray(), combatants, out ICombatantTemplate[] toAdd, out ICombatantTemplate[] updates, out string[] toRemove);

            _scenario.SetCombatants(combatants);

            if (!string.IsNullOrWhiteSpace(Campaign.CampaignID))
            {
                await Exceptions.FailSafeMethodCall(async () =>
                {
                    Server.ICombatManagement? server = Factory.GetCombatManagement();

                    if (server != null)
                    {
                        if (string.IsNullOrWhiteSpace(_scenario.ServerID))
                            await server.CreateCombatScenario(Campaign.CampaignID, _scenario);
                        else
                            await server.UpdateCombatScenario(Campaign.CampaignID, _scenario);

                        if (!string.IsNullOrEmpty(_scenario.ServerID))
                        {
                            foreach (CombatantTemplate template in toAdd)
                                await server.AddScenarioCombatant(Campaign.CampaignID, _scenario.ServerID, template);
                            foreach (CombatantTemplate template in updates)
                                await server.UpdateScenarioCombatant(Campaign.CampaignID, _scenario.ServerID, template);
                            foreach (string id in toRemove)
                                await server.DeleteScenarioCombatant(Campaign.CampaignID, _scenario.ServerID, id);
                        }
                    }
                });
            }

            return _scenario;
        }

        private void GetCombatantChanges(ICombatantTemplate[] before, ICombatantTemplate[] after, out ICombatantTemplate[] add, out ICombatantTemplate[] updates, out string[] remove)
        {
            string[] beforeIDs = before.Select(p => p?.ServerID).Where(p => !string.IsNullOrEmpty(p)).OfType<string>().ToArray();
            string[] afterIDs = after.Select(p => p?.ServerID).Where(p => !string.IsNullOrEmpty(p)).OfType<string>().ToArray();

            ICombatantTemplate[] toAdd = after.Where(p => !beforeIDs.Contains(p.ServerID)).ToArray();
            string[] toRemoveIDs = beforeIDs.Except(afterIDs).ToArray();
            string[] toUpdateIDs = beforeIDs.Intersect(afterIDs).ToArray();

            add = toAdd.ToArray();
            updates = after.Where(p => toUpdateIDs.Contains(p.ServerID)).ToArray();
            remove = toRemoveIDs;
        }

        private void _combatantsMonitor_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(nameof(IsValid));
        }
        #endregion
    }
}
