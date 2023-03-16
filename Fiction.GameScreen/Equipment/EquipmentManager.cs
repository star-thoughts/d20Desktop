using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Fiction.GameScreen.Equipment
{
    /// <summary>
    /// Manages equipment and magic items in a campaign
    /// </summary>
    public sealed class EquipmentManager : ISourcedItemManager
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="EquipmentManager"/>
        /// </summary>
        public EquipmentManager()
        {
            MagicItems = new ObservableCollection<MagicItem>();
            Groups = new ObservableCollection<string>();
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a collection of magic items in the campaign
        /// </summary>
        public ObservableCollection<MagicItem> MagicItems { get; private set; }
        /// <summary>
        /// Gets a collection of magic item groups
        /// </summary>
        public ObservableCollection<string> Groups { get; private set; }
        #endregion
        #region Methods
        /// <summary>
        /// Gets all items of the given source
        /// </summary>
        /// <param name="source">Source to get items from</param>
        /// <returns>Collection of items from that source</returns>
        public ISourcedItem[] GetItemsFromSource(string source)
        {
            return MagicItems.Where(p => string.Equals(p.Source, source, StringComparison.CurrentCultureIgnoreCase)).ToArray();
        }

        /// <summary>
        /// Removes items from the given source
        /// </summary>
        /// <param name="source">Source to remove</param>
        public void RemoveSource(string source)
        {
            ISourcedItem[] items = GetItemsFromSource(source);

            foreach (MagicItem item in items)
                MagicItems.Remove(item);
        }
        /// <summary>
        /// Reconciles groups
        /// </summary>
        public void Reconcile()
        {
            Groups.Clear();
            foreach (string group in MagicItems.Select(p => p.Group).Distinct(StringComparer.CurrentCultureIgnoreCase))
                Groups.Add(group);
        }
        #endregion
    }
}
