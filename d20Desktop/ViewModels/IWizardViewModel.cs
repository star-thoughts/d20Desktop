using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// Interface for displaying a wizard to the user
    /// </summary>
    public interface IWizardViewModel
    {
        /// <summary>
        /// Gets whether or not the user should be able to navigate to the next page
        /// </summary>
        bool CanNext { get; }
        /// <summary>
        /// Gets whether or not the user should be able to complete the wizard now
        /// </summary>
        bool CanFinish { get; }
        /// <summary>
        /// Gets the next page to navigate to
        /// </summary>
        /// <returns>The view model for the next page</returns>
        Task<IWizardViewModel> GetNextPage();
        /// <summary>
        /// Completes the wizard
        /// </summary>
        /// <returns>Whether or not to close the wizard dialog</returns>
        Task<bool> Finish();
        /// <summary>
        /// Cancels the wizard
        /// </summary>
        /// <returns>Whether or not to close the wizard dialog</returns>
        Task<bool> Cancel();
    }
}
