using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for picking a campaign object
    /// </summary>
    /// <remarks>
    /// This shouldn't be used for picking combatants, as they require special sorting.  For those use a
    /// <see cref="SelectCombatantsViewModel"/>.
    /// </remarks>
    public sealed class SelectCampaignObjectViewModel : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="SelectCampaignObjectViewModel"/>
        /// </summary>
        /// <param name="items">Items to choose from</param>
        /// <param name="multiSelect">Whether or not multiple items can be selected</param>
        /// <param name="initial">Initial selection of items</param>
        public SelectCampaignObjectViewModel(IEnumerable<IFilterable> items, bool multiSelect, params IFilterable[] initial)
        {
            Exceptions.ThrowIfArgumentNull(items, nameof(items));
            Exceptions.ThrowIfArgumentNull(initial, nameof(initial));

            Items = items.ToArray();
            MultiSelect = multiSelect;
            SelectedItems = initial.ToObservableCollection();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a collectionm of items to choose from
        /// </summary>
        public IEnumerable<IFilterable> Items { get; private set; }
        /// <summary>
        /// Gets a collection of items chosen
        /// </summary>
        public ObservableCollection<IFilterable> SelectedItems { get; private set; }
        /// <summary>
        /// Gets whether or not the user can select multiple items
        /// </summary>
        public bool MultiSelect { get; private set; }
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid => SelectedItems.Any();
        #endregion
    }
}
