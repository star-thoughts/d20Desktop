using Fiction.GameScreen.Spells;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for managing spells
    /// </summary>
    public sealed class ManageSpellsViewModel : CampaignViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ManageSpellsViewModel"/>
        /// </summary>
        /// <param name="factory">View model factory</param>
        public ManageSpellsViewModel(IViewModelFactory factory)
            : base(factory)
        {
            _filter = new SpellFilterViewModel(factory.Campaign);
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the spells in the campaign
        /// </summary>
        public ObservableCollection<Spell> Spells { get { return Factory.Campaign.Spells.Spells; } }
        private SpellFilterViewModel _filter;
        /// <summary>
        /// Gets or sets the filter to use
        /// </summary>
        public SpellFilterViewModel Filter
        {
            get { return _filter; }
            set
            {
                if (!ReferenceEquals(value, _filter))
                {
                    _filter = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets the category of this view model
        /// </summary>
        public override string ViewModelCategory => Resources.Resources.SpellsCategory;
        /// <summary>
        /// Gets the display name of this view model
        /// </summary>
        public override string ViewModelDisplayName => Resources.Resources.ManageSpellsDisplayName;
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid => true;
        #endregion
    }
}