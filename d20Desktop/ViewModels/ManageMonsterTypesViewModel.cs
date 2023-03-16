using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    public sealed class ManageMonsterTypesViewModel : CampaignViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a nw <see cref="ManageMonsterTypesViewModel"/>
        /// </summary>
        /// <param name="factory">Factory used for campaign view models</param>
        public ManageMonsterTypesViewModel(IViewModelFactory factory)
            : base(factory)
        {
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets a collection of monster types
        /// </summary>
        public ObservableCollection<string> MonsterTypes { get { return Factory.Campaign.MonsterManager.Types; } }
        /// <summary>
        /// Gets a collection of subtypes
        /// </summary>
        public ObservableCollection<string> SubTypes { get { return Factory.Campaign.MonsterManager.SubTypes; } }
        /// <summary>
        /// Gets the category of this view model
        /// </summary>
        public override string ViewModelCategory => Resources.Resources.MonstersCategory;
        /// <summary>
        /// Gets the display name for this view model
        /// </summary>
        public override string ViewModelDisplayName => Resources.Resources.ManageMonsterTypesDisplayName;
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid => true;
        #endregion
    }
}
