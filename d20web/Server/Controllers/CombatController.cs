using d20Web.Models;
using d20Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace d20Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CombatController : ControllerBase
    {
        /// <summary>
        /// Constructs a new <see cref="CombatController"/>
        /// </summary>
        /// <param name="logger">Interface to use for logging</param>
        /// <param name="service">Interface to user for managing combat</param>
        public CombatController(ILogger<CombatController> logger, ICombatService service)
        {
            _combatService = service;
            _logger = logger;
        }

        private ICombatService _combatService;
        private ILogger _logger;

        /// <summary>
        /// Creates a new combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign to create a combat in</param>
        /// <param name="name">Name of the combat</param>
        /// <returns>ID of the newly created combat</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCombat([Required] string campaignID, [Required] string name)
        {
            string id = await _combatService.CreateCombat(campaignID, name, HttpContext.RequestAborted);

            return CreatedAtAction(nameof(GetCombat), new { combatID = id });
        }

        /// <summary>
        /// Removes the given combat
        /// </summary>
        /// <param name="combatID">ID of the combat to remove</param>
        /// <returns>Result of the operation</returns>
        [HttpDelete("{combatID}")]
        public async Task<IActionResult> EndCombat([Required] string combatID)
        {
            await _combatService.EndCombat(combatID, HttpContext.RequestAborted);
            return NoContent();
        }

        /// <summary>
        /// Updates a combat's information
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is in</param>
        /// <param name="combat">Information about the combat to update</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCombat([Required] string campaignID, [Required] Combat combat)
        {
            await _combatService.UpdateCombat(campaignID, combat, HttpContext.RequestAborted);

            return NoContent();
        }

        /// <summary>
        /// Gets the given combat information
        /// </summary>
        /// <param name="combatID">ID of the combat to get</param>
        /// <returns>Information about the combat</returns>
        [HttpGet]
        public async Task<IActionResult> GetCombat([Required] string combatID)
        {
            return Ok(await _combatService.GetCombat(combatID, HttpContext.RequestAborted));
        }

        /// <summary>
        /// Creates a combatant with the given statistics
        /// </summary>
        /// <param name="combatID">ID of the combat to create the combatant in</param>
        /// <param name="combatants">Combatant information to create</param>
        /// <returns>ID of the created combatant</returns>
        [HttpPost("{combatID}/combatant")]
        public async Task<IActionResult> CreateCombatants([Required] string combatID, [Required] Combatant[] combatants)
        {
            IEnumerable<string> ids = await _combatService.CreateCombatant(combatID, combatants, HttpContext.RequestAborted);

            return Ok(new { combatantIDs = ids });
        }

        [HttpGet("{combatID}/combatant")]
        public async Task<IActionResult> GetCombatants([Required] string combatID)
        {
            return Ok(await _combatService.GetCombatants(combatID, HttpContext.RequestAborted));
        }

        /// <summary>
        /// Gets combatant information
        /// </summary>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatantID">ID of the combatant</param>
        /// <returns>Combatant information</returns>
        [HttpGet("{combatID}/combatant/{combatantID}")]
        public async Task<IActionResult> GetCombatant([Required] string combatID, [Required] string combatantID)
        {
            return Ok(await _combatService.GetCombatant(combatID, combatantID, HttpContext.RequestAborted));
        }

        public async Task<IActionResult> UpdateCombatant([Required] string campaignID, [Required] string combatID, [Required] Combatant combatant)
        {
            await _combatService.UpdateCombatant(campaignID, combatID, combatant, HttpContext.RequestAborted);

            return NoContent();
        }
    }
}
