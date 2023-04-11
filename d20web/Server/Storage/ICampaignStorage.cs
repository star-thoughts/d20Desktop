using d20Web.Models;

namespace d20Web.Storage
{
    /// <summary>
    /// Main interface for storing campaign information
    /// </summary>
    public interface ICampaignStorage
    {
        /// <summary>
        /// Creates a campaign
        /// </summary>
        /// <param name="name">Name of the campaign</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the campaign that was created</returns>
        Task<string> CreateCampaign(string name, CancellationToken cancellationToken);
        /// <summary>
        /// Gets a collection of campaigns in the system
        /// </summary>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of campaign information</returns>
        Task<IEnumerable<CampaignListData>> GetCampaigns(CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the information for a campaign
        /// </summary>
        /// <param name="campaignId">ID of the campaign to get</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Campaign information retrieved</returns>
        Task<Campaign> GetCampaign(string campaignId, CancellationToken cancellationToken);
    }
}
