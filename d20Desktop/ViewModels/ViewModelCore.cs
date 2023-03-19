using System.ComponentModel;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// Base class for view models
    /// </summary>
    public abstract class ViewModelCore : IViewModelCore
    {
        #region Properties
        /// <summary>
        /// Gets whether or not this view model's information is valid
        /// </summary>
        /// <remarks>
        /// Override in derived classes to return whether or not the current information is valid.
        /// </remarks>
        public abstract bool IsValid { get; }
        private bool _isDirty;
        /// <summary>
        /// Gets whether or not this view model's information has changed
        /// </summary>
        public bool IsDirty
        {
            get { return _isDirty; }
            protected set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion
        #region Events
        /// <summary>
        /// Event that is triggered when a property changes
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion
        #region Methods
        /// <summary>
        /// Updates the view model to show all of its data has been saved
        /// </summary>
        public void SetClean()
        {
            IsDirty = false;
        }
        /// <summary>
        /// Causes views to update with the current state of the <see cref="IsValid"/> property
        /// </summary>
        /// <param name="setDirty">Whether or not to set the <see cref="IsDirty"/> property to true.</param>
        public void CheckValid(bool setDirty = true)
        {
            if (setDirty)
                IsDirty = true;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
        }
        #endregion
    }
}
