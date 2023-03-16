using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// Core view model for filtering objects
    /// </summary>
    /// <typeparam name="T">Type of item to filter</typeparam>
    public abstract class FilterViewModelCore<T> : ViewModelCore, IFilterViewModel where T : class
    {
        #region Properties
        /// <summary>
        /// Gets whether or not the filter is ready
        /// </summary>
        public abstract bool HasFilter { get; }
        #endregion
        #region Methods
        /// <summary>
        /// Determines whether or not the item given by <paramref name="item"/> matches
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>Whether or not there is a match</returns>
        public abstract bool Matches(T item);
        /// <summary>
        /// Determines whether or not the item given by <paramref name="item"/> matches
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>Whether or not there is a match</returns>
        bool IFilterViewModel.Matches(object item)
        {
            if (item is T t)
                return this.Matches(t);
            return false;
        }
        /// <summary>
        /// Resets this filter back to default
        /// </summary>
        public abstract void Reset();
        #endregion
    }
}
