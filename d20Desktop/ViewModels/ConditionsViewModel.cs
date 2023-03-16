using Fiction.GameScreen.Combat;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for managing conditions
    /// </summary>
    public sealed class ConditionsViewModel : CampaignViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ConditionsViewModel"/>
        /// </summary>
        public ConditionsViewModel(ViewModelFactory factory)
            : base(factory)
        {
            Conditions = factory.Campaign.Conditions.Conditions;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a collection of conditions in the campaign
        /// </summary>
        public ObservableCollection<Condition> Conditions { get; private set; }
        /// <summary>
        /// Gets whether or not all of the conditions are valid
        /// </summary>
        public override bool IsValid
        {
            get { return Conditions.All(p => !string.IsNullOrWhiteSpace(p.Name)); }
        }

        /// <summary>
        /// Gets the category of this view model
        /// </summary>
        public override string ViewModelCategory => Resources.Resources.CombatCategory;

        /// <summary>
        /// Gets the display name of this view model
        /// </summary>
        public override string ViewModelDisplayName => Resources.Resources.ConditionsViewModelName;
        #endregion
    }
}
