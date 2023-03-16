using System.ComponentModel;

namespace Fiction.GameScreen.ViewModels.Tables
{
    /// <summary>
    /// View model for referencing another table
    /// </summary>
    public class OtherTableViewModel : INotifyPropertyChanged
    {
        private TableViewModel _table;
        /// <summary>
        /// Gets or sets the table to roll on
        /// </summary>
        public TableViewModel Table
        {
            get { return _table; }
            set
            {
                if (!ReferenceEquals(_table, value))
                {
                    _table = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Table)));
                }
            }
        }
        public int _numberOfRolls;
        /// <summary>
        /// Gets or sets the number of times to roll on the target table
        /// </summary>
        public int NumberOfRolls
        {
            get { return _numberOfRolls; }
            set
            {
                if (_numberOfRolls != value)
                {
                    _numberOfRolls = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumberOfRolls)));
                }
            }
        }
        private bool _allowRepeats;
        /// <summary>
        /// Gets or sets whether or not the rolls on the target table can be repeated.
        /// </summary>
        /// <remarks>
        /// When true, all rolls on the target table are accepted.  When false, any rolls on the target table
        /// cannot match this roll and cannot match each other.
        /// </remarks>
        public bool AllowRepeats
        {
            get { return _allowRepeats; }
            set
            {
                if (_allowRepeats != value)
                {
                    _allowRepeats = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllowRepeats)));
                }
            }
        }
        #region Events
        /// <summary>
        /// Event that is triggered when a property changse
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}