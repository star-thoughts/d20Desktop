using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// Base view model for campaign information
    /// </summary>
    public abstract class CampaignViewModelCore : ViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="CampaignViewModelCore"/>
        /// </summary>
        /// <param name="factory">Factory used to create this view model</param>
        protected CampaignViewModelCore(IViewModelFactory factory)
        {
            Exceptions.ThrowIfArgumentNull(factory, nameof(factory));

            Factory = factory;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the category this view model falls into
        /// </summary>
        public abstract string ViewModelCategory { get; }
        /// <summary>
        /// Gets the display name for this view model
        /// </summary>
        public abstract string ViewModelDisplayName { get; }
        /// <summary>
        /// Gets the campaign's view model factory used to generate this view model
        /// </summary>
        public IViewModelFactory Factory { get; private set; }
        #endregion
    }
}
