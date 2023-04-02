﻿using d20Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace d20Web.Controllers
{
    /// <summary>
    /// Controller for handling campaign management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class CampaignsController : ControllerBase
    {
        /// <summary>
        /// Constructs a new <see cref="CampaignsController"/>
        /// </summary>
        /// <param name="campaignsService">Interface for managing campaigns</param>
        /// <param name="logger">Interface to use for logging</param>
        public CampaignsController(ICampaignsService campaignsService, ILogger<CampaignsController> logger)
        {
            _logger = logger;
            _campaignsService = campaignsService;
        }

        private readonly ILogger _logger;
        private readonly ICampaignsService _campaignsService;

        /// <summary>
        /// Creates a campaign
        /// </summary>
        /// <param name="name">Name to give the new campaign</param>
        /// <returns>ID of the campaign that was created</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCampaign([Required] string name)
        {
            string id = await _campaignsService.CreateCampaign(name, HttpContext.RequestAborted);

            return CreatedAtAction(nameof(GetCampaign), new { campaignID = id });
        }

        /// <summary>
        /// Gets the basic information for a campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign</param>
        /// <returns>Basic campaign information</returns>
        [HttpGet("{campaignID}")]
        public async Task<IActionResult> GetCampaign([Required] string campaignID)
        {
            return Ok(await _campaignsService.GetCampaign(campaignID, HttpContext.RequestAborted));
        }
    }
}