using System.ComponentModel;

namespace Fiction.GameScreen.Monsters
{
    /// <summary>
    /// Contains information about a monster type
    /// </summary>
    public sealed class MonsterType : INotifyPropertyChanged
    {
        #region Properties
        private string? _name;
        /// <summary>
        /// Gets or sets the name of the monster type
        /// </summary>
        public string? Name
        {
            get { return _name; }
            set
            {
                if (!string.Equals(_name, value, System.StringComparison.CurrentCulture))
                {
                    _name = value;
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
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 67
        #endregion
    }
}