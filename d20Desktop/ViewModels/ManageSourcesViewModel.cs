using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for managing sources in the campaign
    /// </summary>
    public sealed class ManageSourcesViewModel : CampaignViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ManageSourcesViewModel"/>
        /// </summary>
        /// <param name="factory"></param>
        public ManageSourcesViewModel(IViewModelFactory factory)
            : base(factory)
        {
            Sources = new ReadOnlyObservableCollection<string>(factory.Campaign.Sources);
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the sources associated with the campaign
        /// </summary>
        public ReadOnlyObservableCollection<string> Sources { get; private set; }
        private IEnumerable<ISourcedItem> _items;
        /// <summary>
        /// Gets a set of items based on a call to
        /// </summary>
        public IEnumerable<ISourcedItem> Items
        {
            get { return _items; }
            private set
            {
                if (!ReferenceEquals(this, value))
                {
                    _items = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets the category of the view model
        /// </summary>
        public override string ViewModelCategory => Resources.Resources.CampaignSettingsCategory;
        /// <summary>
        /// Gets the display name for the view model
        /// </summary>
        public override string ViewModelDisplayName => Resources.Resources.ManageSourcesDisplayName;
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid => true;
        #endregion
        #region Methods
        /// <summary>
        /// Sets the <see cref="Items"/> collection based on the source
        /// </summary>
        /// <param name="source">Source to populate into <see cref="Items"/></param>
        public void ShowItemsFromSource(string source)
        {
            Exceptions.ThrowIfArgumentNull(source, nameof(source));

            Items = Factory.Campaign.GetAllSourcedItems(source);
        }
        #endregion
    }
}
