using d20Web.Models.Players;
using d20Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace d20Web.Controllers
{
    /// <summary>
    /// Controller for managing players
    /// </summary>
    [ApiController]
    [Route("api/Campaign/{campaignID}/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PlayersController : ControllerBase
    {
        /// <summary>
        /// Constructs a new <see cref="PlayersController"/>
        /// </summary>
        /// <param name="campaignsService">Interface for managing campaigns</param>
        /// <param name="logger">Interface to use for logging</param>
        public PlayersController(ICampaignsService campaignsService, ILogger<CampaignController> logger)
        {
            _logger = logger;
            _campaignsService = campaignsService;
        }

        private readonly ILogger _logger;
        private readonly ICampaignsService _campaignsService;

        [HttpPost("character")]
        public async Task<IActionResult> CreatePlayerCharacter(string campaignID, [Required] PlayerCharacter character)
        {
            string characterID = await _campaignsService.CreatePlayerCharacter(campaignID, character, HttpContext.RequestAborted);

            return CreatedAtAction(nameof(GetPlayerCharacter), new { campaignID, characterID }, new { id = characterID });
        }

        [HttpGet("character")]
        public async Task<IActionResult> GetPlayerCharacters(string campaignID)
        {
            return Ok(await _campaignsService.GetPlayerCharacters(campaignID, HttpContext.RequestAborted));
        }

        [HttpGet("character/{characterID}")]
        public async Task<IActionResult> GetPlayerCharacter(string campaignID, [Required] string characterID)
        {
            return Ok(await _campaignsService.GetPlayerCharacter(campaignID, characterID, HttpContext.RequestAborted));
        }

        [HttpDelete("character/{characterID}")]
        public async Task<IActionResult> DeletePlayerCharacter(string campaignID, [Required] string characterID)
        {
            await _campaignsService.DeletePlayerCharacter(campaignID, characterID, HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpPut("character")]
        public async Task<IActionResult> UpdatePlayerCharacter(string campaignID, [Required] PlayerCharacter character)
        {
            await _campaignsService.UpdatePlayerCharacter(campaignID, character, HttpContext.RequestAborted);

            return NoContent();
        }
    }
}
