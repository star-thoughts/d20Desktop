using System.ComponentModel;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for selecting a type of damage
    /// </summary>
    public class DamageTypeSelectionViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructs a new <see cref="DamageTypeSelectionViewModel"/>
        /// </summary>
        /// <param name="type">Type of damage being produced</param>
        public DamageTypeSelectionViewModel(string type)
        {
            Type = type;
        }
        private bool _selected;
        /// <summary>
        /// Gets or sets whether or not this damage type is selected
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        /// <summary>
        /// Gets the type of damage
        /// </summary>
        public string Type { get; }
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