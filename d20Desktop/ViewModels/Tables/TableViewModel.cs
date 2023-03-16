using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels.Tables
{
    /// <summary>
    /// View model for a table
    /// </summary>
    public class TableViewModel : INotifyPropertyChanged
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="TableViewModel"/>
        /// </summary>
        public TableViewModel()
        {
            Entries = new ObservableCollection<TableEntryViewModel>();
        }
        #endregion
        #region Properties
        private string _name;
        /// <summary>
        /// Gets or sets the name of the table
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.Equals(_name, value, StringComparison.Ordinal))
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }
        /// <summary>
        /// Gets a collection of entries for this table
        /// </summary>
        public ObservableCollection<TableEntryViewModel> Entries { get; }
        #endregion
        #region Events
        /// <summary>
        /// Event that is triggered when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Methods
        /// <summary>
        /// Gets the entry at the given position, using the size values of each entry
        /// </summary>
        /// <param name="position">The given position in the table, must be between 0 and the sum of all entry sizes</param>
        /// <returns>Entry found</returns>
        public TableEntryViewModel GetEntry(int position)
        {
            foreach (TableEntryViewModel entry in Entries)
            {
                position -= entry.EntrySize;
                if (position <= 0)
                    return entry;
            }
            throw new ArgumentException($"Entry must be between {{0}} and {Entries.Sum(p => p.EntrySize)}.", nameof(position));
        }
        #endregion
    }
}
