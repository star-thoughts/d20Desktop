using Fiction.GameScreen.Monsters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for managing monsters in a campaign
    /// </summary>
    public sealed class ManageMonstersViewModel : CampaignViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ManageMonstersViewModel"/>
        /// </summary>
        /// <param name="factory">Factory for creating view models</param>
        public ManageMonstersViewModel(IViewModelFactory factory)
            : base(factory)
        {
            Campaign = factory.Campaign;
            Manager = factory.Campaign.MonsterManager;
            Filter = new MonsterFilterViewModel(factory.Campaign);
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the campaign for this view model
        /// </summary>
        public CampaignSettings Campaign { get; private set; }
        /// <summary>
        /// Gets the monster manager for the campaign
        /// </summary>
        public MonsterManager Manager { get; private set; }
        /// <summary>
        /// Gets a collection of monsters in the campaign
        /// </summary>
        public ObservableCollection<Monster> Monsters { get { return Manager.Monsters; } }
        /// <summary>
        /// Gets or sets the filter listing specific monsters
        /// </summary>
        public MonsterFilterViewModel Filter { get; set; }
        /// <summary>
        /// Gets the category for this view model
        /// </summary>
        public override string ViewModelCategory => Resources.Resources.MonstersCategory;
        /// <summary>
        /// Gets the display name of this view model
        /// </summary>
        public override string ViewModelDisplayName => Resources.Resources.ManageMonstersDisplayName;
        /// <summary>
        /// Gets whether or not this view model is valid
        /// </summary>
        public override bool IsValid => true;
        #endregion
        #region Methods

        #endregion
    }
}
