using Fiction.GameScreen.Combat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

            Name = _scenario.Name;
            Group = _scenario.Group;
            Details = _scenario.Details;
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
        private string _group;
        /// <summary>
        /// Gets or sets the group this scenario is in
        /// </summary>
        public string Group
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
        private string _details;
        /// <summary>
        /// Gets or sets any extra details for the scenario
        /// </summary>
        public string Details
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
        public CombatScenario Save()
        {
            _scenario.Name = Name;
            _scenario.Group = Group;
            _scenario.Details = Details;
            _scenario.SetCombatants(Combatants.Select(p => p.Save()));

            return _scenario;
        }

        private void _combatantsMonitor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(nameof(IsValid));
        }
        #endregion
    }
}
