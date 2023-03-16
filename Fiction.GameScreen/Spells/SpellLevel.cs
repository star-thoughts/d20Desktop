using System.ComponentModel;

namespace Fiction.GameScreen.Spells
{
    /// <summary>
    /// Contains information about a spells level
    /// </summary>
    public class SpellLevel : INotifyPropertyChanged
    {
        #region Constructors
        #endregion
        #region Properties
        private string _class;
        /// <summary>
        /// Gets or sets the class for this spell
        /// </summary>
        public string Class
        {
            get { return _class; }
            set
            {
                if (!string.Equals(_class, value, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    _class = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private int _level;
        /// <summary>
        /// Gets or sets the level of this spell
        /// </summary>
        public int Level
        {
            get { return _level; }
            set
            {
                if (_level != value)
                {
                    _level = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion
        #region Events
#pragma warning disable 67
        /// <summary>
        /// Event that is triggered when a property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67
        #endregion
    }
}