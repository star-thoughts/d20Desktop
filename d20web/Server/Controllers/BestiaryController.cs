using d20Web.Models.Bestiary;
using d20Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace d20Web.Controllers
{
    /// <summary>
    /// Controller for handling bestiary management
    /// </summary>
    [ApiController]
    [Route("api/Campaign/{campaignID}/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public sealed class BestiaryController : ControllerBase
    {
        /// <summary>
        /// Constructs a new <see cref="CampaignController"/>
        /// </summary>
        /// <param name="campaignsService">Interface for managing campaigns</param>
        /// <param name="logger">Interface to use for logging</param>
        public BestiaryController(ICampaignsService campaignsService, ILogger<CampaignController> logger)
        {
            _logger = logger;
            _campaignsService = campaignsService;
        }

        private readonly ILogger _logger;
        private readonly ICampaignsService _campaignsService;

        /// <summary>
        /// Adds a monster to the campaign's bestiary
        /// </summary>
        /// <param name="campaignID">ID of the campaign to add the monster to</param>
        /// <param name="monster">Monster to add to the campaign's bestiary</param>
        /// <returns>ID of the monster added</returns>
        [HttpPost]
        public async Task<IActionResult> CreateMonster([Required] string campaignID, [Required] Monster monster)
        {
            string id = await _campaignsService.CreateMonster(campaignID, monster, HttpContext.RequestAborted);

            return Ok(new { id });
        }

        /// <summary>
        /// Updates a monster with the given monster information
        /// </summary>
        /// <param name="campaignID">ID of the campaign the monster is in</param>
        /// <param name="id">ID of the monster to update</param>
        /// <param name="monster">Monster information for the update</param>
        /// <returns>Result of the operation</returns>
        [HttpPost("{id}")]
        public async Task<IActionResult> EditMonster([Required] string campaignID, [Required] string id, [Required] Monster monster)
        {
            if (!string.Equals(id, monster.ID, StringComparison.OrdinalIgnoreCase))
                monster.ID = id;

            await _campaignsService.UpdateMonster(campaignID, monster, HttpContext.RequestAborted);

            return NoContent();
        }

        /// <summary>
        /// Deletes the monster given by the ID
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the monster</param>
        /// <param name="id">ID of the monster to delete</param>
        /// <returns>Result of the operation</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonster([Required] string campaignID, [Required] string id)
        {
            await _campaignsService.DeleteMonster(campaignID, id, HttpContext.RequestAborted);

            return NoContent();
        }

        /// <summary>
        /// Gets the information for the given monster
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the monster</param>
        /// <param name="id">ID of the monster to get</param>
        /// <returns>Result of the operation</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMonster([Required] string campaignID, [Required] string id)
        {
            return Ok(await _campaignsService.GetMonster(campaignID, id, HttpContext.RequestAborted));
        }
    }
}
