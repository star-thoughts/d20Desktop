using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// Interface for filtering items
    /// </summary>
    public interface IFilterViewModel
    {
        /// <summary>
        /// Gets whether or not the filter is set up, otherwise the filter is empty
        /// </summary>
        bool HasFilter { get; }
        /// <summary>
        /// Detremines whether the object given in <paramref name="item"/> matches the filter
        /// </summary>
        /// <param name="item">Item to test against</param>
        /// <returns>Whether or not there is a match</returns>
        bool Matches(object item);
        /// <summary>
        /// Resets the filter back to default
        /// </summary>
        void Reset();
    }
}
