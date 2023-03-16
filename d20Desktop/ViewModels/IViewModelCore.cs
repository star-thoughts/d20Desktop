using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// Base interface for view models
    /// </summary>
    public interface IViewModelCore : INotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// Gets whether or not this view model's information is valid
        /// </summary>
        bool IsValid { get; }
        /// <summary>
        /// Gets whether or not this view model's information has changed
        /// </summary>
        bool IsDirty { get; }
        #endregion
        #region Methods
        /// <summary>
        /// Updates the view model to show all of its data has been saved
        /// </summary>
        void SetClean();
        /// <summary>
        /// Causes views to update with the current state of the <see cref="IsValid"/> property
        /// </summary>
        /// <param name="setDirty">Whether or not to set the <see cref="IsDirty"/> property to true.</param>
        void CheckValid(bool setDirty = true);
        #endregion
    }
}
