using d20Web.Models;

namespace d20Web.Clients
{
    /// <summary>
    /// Interface for communicating with a campaign server
    /// </summary>
    public interface ICampaignServer
    {
        /// <summary>
        /// Gets a list of combats in a campaign
        /// </summary>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of combats in the campaign</returns>
        Task<IEnumerable<CampaignListData>> GetCampaigns(CancellationToken cancellationToken = default);
    }
}