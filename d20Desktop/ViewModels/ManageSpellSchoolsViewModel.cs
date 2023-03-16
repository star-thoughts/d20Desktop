using Fiction.GameScreen.Spells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for managing spell school
    /// </summary>
    public sealed class ManageSpellSchoolsViewModel : CampaignViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ManageSpellSchoolsViewModel"/>
        /// </summary>
        public ManageSpellSchoolsViewModel(IViewModelFactory factory)
            : base(factory)
        {
            SpellManager = factory.Campaign.Spells;
            Schools = SpellManager.Schools;
            SubSchools = SpellManager.SubSchools;
            EffectTypes = SpellManager.SpellEffectTypes;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the spell manager
        /// </summary>
        public SpellManager SpellManager { get; private set; }
        /// <summary>
        /// Gets the schools in the campaign
        /// </summary>
        public ReadOnlyObservableCollection<string> Schools { get; private set; }
        /// <summary>
        /// Gets the subschools in the campaign
        /// </summary>
        public ReadOnlyObservableCollection<string> SubSchools { get; private set; }
        /// <summary>
        /// Gets the types of effects
        /// </summary>
        public ReadOnlyObservableCollection<string> EffectTypes { get; private set; }
        /// <summary>
        /// Gets whether or not this view model's data is valid
        /// </summary>
        public override bool IsValid => true;
        /// <summary>
        /// Gets the category for this view model
        /// </summary>
        public override string ViewModelCategory => Resources.Resources.SpellsCategory;
        /// <summary>
        /// Gets the display name for this view model
        /// </summary>
        public override string ViewModelDisplayName => Resources.Resources.ManageSpellSchoolsDisplayName;
        #endregion
    }
}
