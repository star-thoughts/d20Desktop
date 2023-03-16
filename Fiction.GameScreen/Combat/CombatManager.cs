using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Fiction.GameScreen.Combat
{
    /// <summary>
    /// Manages combat scenarios in a cmapaign
    /// </summary>
    public sealed class CombatManager : INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CombatManager"/>
        /// </summary>
        internal CombatManager()
        {
            Scenarios = new ObservableCollection<CombatScenario>();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a collection of scenarios for combat
        /// </summary>
        public ObservableCollection<CombatScenario> Scenarios { get; private set; }
        private ActiveCombat _active;

        /// <summary>
        /// Gets or sets the currently active combat
        /// </summary>
        public ActiveCombat Active
        {
            get { return _active; }
            set
            {
                if (!ReferenceEquals(_active, value))
                {
                    _active = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Determines whether or not the given combat scenario can be deleted
        /// </summary>
        /// <param name="scenario">Combat scenario to delete</param>
        /// <returns>Whether or not the combat scenario can be deleted</returns>
        public bool CanDeleteScenario(CombatScenario scenario)
        {
            return scenario.Combatants.All(p => CanDeleteCombatantTemplate(p));
        }
        /// <summary>
        /// Determines whether or not the given combatant template can be deleted
        /// </summary>
        /// <param name="template">Template to delete</param>
        /// <returns>Whether or not the template can be deleted</returns>
        public bool CanDeleteCombatantTemplate(ICombatantTemplate template)
        {
            return true;
        }
        #endregion
        #region Events
#pragma warning disable 67
        /// <summary>
        /// Event that is triggered when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67
        #endregion
    }
}
