﻿using d20Web.Hubs;
using d20Web.Models;
using d20Web.Storage;
using Microsoft.AspNetCore.SignalR;

namespace d20Web.Services
{
    /// <summary>
    /// Manages combat within the system
    /// </summary>
    public class CombatService : ICombatService
    {
        public CombatService(ILogger<CombatService> logger, ICombatStorage storage, IHubContext<CampaignHub> hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        private readonly IHubContext<CampaignHub> _hub;
        private readonly ILogger<CombatService> _logger;
        private readonly ICombatStorage _storage;

        /// <summary>
        /// Creates a new combat in a campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign to create the combat in</param>
        /// <param name="name">Name of the combat to create</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combat that was created</returns>
        /// <exception cref="ArgumentNullException">One or more parameters was null or empty</exception>
        public async Task<string> CreateCombat(string campaignID, string name, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            string id = await _storage.BeginCombat(campaignID, name, cancellationToken);

            _ = _hub.CombatCreated(campaignID, id, name);

            return id;
        }

        /// <summary>
        /// Gets information for a combat
        /// </summary>
        /// <param name="combatID">ID of the combat to get information for</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combat information</returns>
        public async Task<Combat> GetCombat(string combatID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));

            return await _storage.GetCombat(combatID, cancellationToken);
        }
        /// <summary>
        /// Creates a combatant with the given statistics
        /// </summary>
        /// <param name="combatID">ID of the combat to create the combatant in</param>
        /// <param name="combatant">Combatant information to create</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combatant</returns>
        public async Task<IEnumerable<string>> CreateCombatant(string combatID, IEnumerable<Combatant> combatants, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (combatants == null)
                throw new ArgumentNullException(nameof(combatants));
            if (!combatants.Any())
                throw new ArgumentException(nameof(combatants));

            foreach (Combatant combatant in combatants)
            {
                if (string.IsNullOrWhiteSpace(combatant.Name))
                    throw new ArgumentNullException(nameof(combatant.Name));
                if (combatant.Health == null)
                    throw new ArgumentNullException(nameof(combatant.Health));
            }

            (string campaignID, IEnumerable<string> ids) = await _storage.CreateCombatants(combatID, combatants, cancellationToken);

            _ = _hub.CombatantCreated(campaignID, ids);

            return ids;
        }
        /// <summary>
        /// Updates the stats for a combatant
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatant">Combatant details to use for the update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateCombatant(string campaignID, string combatID, Combatant combatant, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (combatant == null)
                throw new ArgumentNullException(nameof(combatant));

            await _storage.UpdateCombatant(campaignID, combatID, combatant, cancellationToken);

            _ = _hub.CombatantUpdated(campaignID, combatant.ToPlayerView());
        }
        /// <summary>
        /// Gets combatant information
        /// </summary>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatantID">ID of the combatant</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combatant information</returns>
        public async Task<Combatant> GetCombatant(string combatID, string combatantID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (string.IsNullOrWhiteSpace(combatantID))
                throw new ArgumentNullException(nameof(combatantID));

            return await _storage.GetCombatant(combatID, combatantID, cancellationToken);
        }
        /// <summary>
        /// Gets combatant information
        /// </summary>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combatant information</returns>
        public async Task<IEnumerable<Combatant>> GetCombatants(string combatID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));

            IEnumerable<Combatant> result = await _storage.GetCombatants(combatID, cancellationToken);

            //  For now, we assume all clients are players and return only what they have access to
            result = result.Where(p => p.CanDisplayToPlayers())
                .Select(p => p.ToPlayerView());

            return result;
        }
    }
}