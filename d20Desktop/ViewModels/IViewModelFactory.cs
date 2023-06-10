using Fiction.GameScreen.Server;
using System.Net.Http;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// Base interface for view model factory
    /// </summary>
    public interface IViewModelFactory
    {
        #region Properties
        /// <summary>
        /// Gets the campaign this view model factory is for
        /// </summary>
        CampaignSettings Campaign { get; }
        /// <summary>
        /// Gets the combat scenarios for this campaign
        /// </summary>
        CombatScenariosViewModel CombatScenarios { get; }
        /// <summary>
        /// Gets the view model to use to begin combat
        /// </summary>
        PrepareCombatViewModel BeginCombat { get; }
        /// <summary>
        /// Gets the currently active combat
        /// </summary>
        ActiveCombatViewModel? ActiveCombat { get; }
        /// <summary>
        /// Gets the view model for managing monsters
        /// </summary>
        ManageMonstersViewModel MonsterManager { get; }
        /// <summary>
        /// Gets the view model for managing players
        /// </summary>
        ManagePlayersViewModel Players { get; }
        /// <summary>
        /// Gets the view model for managing monster types and subtypes
        /// </summary>
        ManageMonsterTypesViewModel ManageTypes { get; }
        /// <summary>
        /// Gets the view model for managing spells
        /// </summary>
        ManageSpellsViewModel Spells { get; }
        /// <summary>
        /// Gets the view model for managing spell schools
        /// </summary>
        ManageSpellSchoolsViewModel ManageSpellSchools { get; }
        /// <summary>
        /// Gets the view model for managing sources
        /// </summary>
        ManageSourcesViewModel ManageSources { get; }
        /// <summary>
        /// Gets the view model for managing magic items
        /// </summary>
        MagicItemsViewModel MagicItems { get; }
        /// <summary>
        /// Gets the server to use for campaign management
        /// </summary>
        ICampaignManagement? Server { get; }
        #endregion
        #region Methods

        /// <summary>
        /// Gets the campaign manager for the configured server
        /// </summary>
        /// <returns>Interface for managing campaigns on the server</returns>
        ICampaignManagement? GetCampaignManager();
        ICombatManagement? GetCombatManagement();
        #endregion
    }
}