using d20Web.Models.Combat;
using d20Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace d20Web.Controllers
{
    [ApiController]
    [Route("api/Campaign/{campaignID}/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
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

        #region Combat Prep
        [HttpPost("prep")]
        public async Task<IActionResult> BeginPrep([Required] string campaignID)
        {
            string id = await _combatService.CreateCombatPrep(campaignID, HttpContext.RequestAborted);

            return CreatedAtAction(nameof(GetCombatPrep), new { campaignID = campaignID, combatPrepID = id }, new { combatID = id });
        }

        [HttpGet("prep")]
        public async Task<IActionResult> GetCombatPreps([Required] string campaignID)
        {
            return Ok(await _combatService.GetCombatPreps(campaignID, HttpContext.RequestAborted));
        }

        [HttpGet("prep/{combatPrepID}")]
        public async Task<IActionResult> GetCombatPrep([Required] string campaignID, [Required] string combatPrepID)
        {
            return Ok(await _combatService.GetCombatPrep(campaignID, combatPrepID, HttpContext.RequestAborted));
        }

        [HttpDelete("prep/{combatPrepID}")]
        public async Task<IActionResult> DeleteCombatPrep([Required] string campaignID, [Required] string combatPrepID)
        {
            await _combatService.EndCombatPrep(campaignID, combatPrepID, HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpPost("prep/{combatPrepID}/combatant")]
        public async Task<IActionResult> AddCombatantPreparers([Required] string campaignID, [Required] string combatPrepID, [Required] IEnumerable<CombatantPreparer> preparers)
        {
            IEnumerable<string> ids = await _combatService.AddCombatantPreparers(campaignID, combatPrepID, preparers, HttpContext.RequestAborted);

            return Ok(new { combatantIDs = ids.ToArray() });
        }

        [HttpDelete("prep/{combatPrepID}/combatant")]
        public async Task<IActionResult> RemoveCombatantPreparer([Required] string campaignID, [Required] string combatPrepID, [Required] string[] ids)
        {
            await _combatService.DeleteCombatantPreparers(campaignID, combatPrepID, ids, HttpContext.RequestAborted);

            return NoContent();
        }


        [HttpPut("prep/{combatID}/combatant/{combatantID}")]
        public async Task<IActionResult> UpdateCombatantPreparer([Required] string campaignID, [Required] string combatID, [Required] CombatantPreparer combatant)
        {
            await _combatService.UpdateCombatantPreparer(campaignID, combatID, combatant, HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpGet("prep/{combatID}/combatant")]
        public async Task<IActionResult> GetCombatantPreparers([Required] string campaignID, [Required] string combatID)
        {
            var result = await _combatService.GetCombatantPreparers(campaignID, combatID, Array.Empty<string>(), HttpContext.RequestAborted);

            return Ok(result);
        }
        #endregion
        #region Combat
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

            return CreatedAtAction(nameof(GetCombat), new { campaignID = campaignID, combatID = id }, new { combatID = id });
        }

        /// <summary>
        /// Removes the given combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is in</param>
        /// <param name="combatID">ID of the combat to remove</param>
        /// <returns>Result of the operation</returns>
        [HttpDelete("{combatID}")]
        public async Task<IActionResult> EndCombat([Required] string campaignID, [Required] string combatID)
        {
            await _combatService.EndCombat(campaignID, combatID, HttpContext.RequestAborted);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetCombats([Required] string campaignID)
        {
            return Ok(await _combatService.GetCombats(campaignID, HttpContext.RequestAborted));
        }

        /// <summary>
        /// Updates a combat's information
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is in</param>
        /// <param name="combat">Information about the combat to update</param>
        /// <returns></returns>
        [HttpPut("{combatID}")]
        public async Task<IActionResult> UpdateCombat([Required] string campaignID, [Required] Combat combat)
        {
            await _combatService.UpdateCombat(campaignID, combat, HttpContext.RequestAborted);

            return NoContent();
        }

        /// <summary>
        /// Gets the given combat information
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is in</param>
        /// <param name="combatID">ID of the combat to get</param>
        /// <returns>Information about the combat</returns>
        [HttpGet("{combatID}")]
        public async Task<IActionResult> GetCombat([Required] string campaignID, [Required] string combatID)
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
        public async Task<IActionResult> CreateCombatants([Required] string campaignID, [Required] string combatID, [Required] Combatant[] combatants)
        {
            IEnumerable<string> ids = await _combatService.CreateCombatant(campaignID, combatID, combatants, HttpContext.RequestAborted);

            return Ok(new { combatantIDs = ids });
        }

        [HttpGet("{combatID}/combatant")]
        public async Task<IActionResult> GetCombatants([Required] string campaignID, [Required] string combatID)
        {
            IEnumerable<Combatant> result = await _combatService.GetCombatants(combatID, HttpContext.RequestAborted);
            return Ok(result);
        }

        /// <summary>
        /// Gets combatant information
        /// </summary>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatantID">ID of the combatant</param>
        /// <returns>Combatant information</returns>
        [HttpGet("{combatID}/combatant/{combatantID}")]
        public async Task<IActionResult> GetCombatant([Required] string campaignID, [Required] string combatID, [Required] string combatantID)
        {
            return Ok(await _combatService.GetCombatant(combatID, combatantID, HttpContext.RequestAborted));
        }

        [HttpPut("{combatID}/combatant/{combatantID}")]
        public async Task<IActionResult> UpdateCombatant([Required] string campaignID, [Required] string combatID, [Required] Combatant combatant)
        {
            await _combatService.UpdateCombatant(campaignID, combatID, combatant, HttpContext.RequestAborted);

            return NoContent();
        }

        /// <summary>
        /// Deletes the combatant given
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat the combatant is in</param>
        /// <param name="combatID">ID of the combat</param>
        /// <param name="combatantIDs">ID of the combatants to delete</param>
        /// <returns></returns>
        [HttpDelete("{combatID}/combatant")]
        public async Task<IActionResult> DeleteCombatants([Required] string campaignID, [Required] string combatID, [Required] string[] combatantIDs)
        {
            await _combatService.DeleteCombatant(campaignID, combatID, combatantIDs, HttpContext.RequestAborted);

            return NoContent();
        }
        #endregion
    }
}
