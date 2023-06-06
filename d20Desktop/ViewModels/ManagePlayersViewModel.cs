using Fiction.GameScreen.Players;
using Fiction.GameScreen.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for managing players in the camapign
    /// </summary>
    public sealed class ManagePlayersViewModel : CampaignViewModelCore
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="ManagePlayersViewModel"/>
        /// </summary>
        public ManagePlayersViewModel(IViewModelFactory factory)
            : base(factory)
        {
            Campaign = factory.Campaign;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the campaign
        /// </summary>
        public CampaignSettings Campaign { get; private set; }
        /// <summary>
        /// Gets a collection of characters
        /// </summary>
        public ObservableCollection<PlayerCharacter> Characters { get { return Campaign.Players.PlayerCharacters; } }
        /// <summary>
        /// Gets whether or not this view model's data is valid
        /// </summary>
        public override bool IsValid => true;
        /// <summary>
        /// Gets the category for this view model
        /// </summary>
        public override string ViewModelCategory { get { return Resources.Resources.CampaignSettingsCategory; } }
        /// <summary>
        /// Gets the display name for this view model
        /// </summary>
        public override string ViewModelDisplayName { get { return Resources.Resources.PlayersDisplayName; } }
        #endregion
        #region Methods
        public async Task Remove(PlayerCharacter character)
        {
            Characters.Remove(character);

            ICampaignManagement? campaignManagement = Factory.GetCampaignManager();
            if (campaignManagement != null && !string.IsNullOrEmpty(character.ServerID))
                await campaignManagement.DeletePlayerCharacter(character.ServerID);
        }
        #endregion
    }
}
