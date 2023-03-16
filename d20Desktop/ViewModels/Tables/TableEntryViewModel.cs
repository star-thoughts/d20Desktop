using System.ComponentModel;

namespace Fiction.GameScreen.ViewModels.Tables
{
    /// <summary>
    /// View model for a single entry in a table
    /// </summary>
    public class TableEntryViewModel : INotifyPropertyChanged
    {
        private int _entrySize;
        /// <summary>
        /// Gets or sets the size of the range for this entry
        /// </summary>
        public int EntrySize
        {
            get { return _entrySize; }
            set
            {
                if (_entrySize != value)
                {
                    _entrySize = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EntrySize)));
                }
            }
        }
        private string _text;

        /// <summary>
        /// Gets or sets the text to display for this entry
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                if (!string.Equals(_text, value, System.StringComparison.Ordinal))
                {
                    _text = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
                }
            }
        }

        public OtherTableViewModel _otherTable;

        /// <summary>
        /// Gets or sets another table to roll on when this entry is rolled
        /// </summary>
        public OtherTableViewModel OtherTable
        {
            get { return _otherTable; }
            set
            {
                if (!ReferenceEquals(_otherTable, value))
                {
                    _otherTable = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OtherTable)));
                }
            }
        }
        #region Events
        /// <summary>
        /// Event that is triggered when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}