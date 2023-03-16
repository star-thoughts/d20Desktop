using Fiction.GameScreen.Equipment;
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
    /// View model for managing magic items
    /// </summary>
    public sealed class MagicItemsViewModel : CampaignViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="MagicItemsViewModel"/>
        /// </summary>
        /// <param name="factory">Factory for managing view models</param>
        public MagicItemsViewModel(IViewModelFactory factory)
            : base(factory)
        {
            Filter = new MagicItemFilterViewModel(factory.Campaign);
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a collection of magic items in the campaign
        /// </summary>
        public ObservableCollection<MagicItem> MagicItems { get { return Factory.Campaign.EquipmentManager.MagicItems; } }
        private MagicItemFilterViewModel _filter;
        /// <summary>
        /// Gets or sets the filter to use
        /// </summary>
        public MagicItemFilterViewModel Filter
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
        /// Gets the category for this view model
        /// </summary>
        public override string ViewModelCategory => Resources.Resources.EquipmentCategory;
        /// <summary>
        /// Gets the display name of this view model
        /// </summary>
        public override string ViewModelDisplayName => Resources.Resources.MagicItemsDisplayName;
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid => true;
        #endregion
    }
}
