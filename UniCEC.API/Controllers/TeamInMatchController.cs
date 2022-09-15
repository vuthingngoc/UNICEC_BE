using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.TeamInMatchSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.TeamInMatch;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/teams-in-match")]
    [ApiController]
    [ApiVersion("1.0")]
    
    public class TeamInMatchController : ControllerBase
    {
        private ITeamInMatchService _teamInMatchService;

        public TeamInMatchController(ITeamInMatchService teamInMatchService)
        {
            _teamInMatchService = teamInMatchService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get detail team in match by id - all user")]
        public async Task<IActionResult> GetTeamInMatchById(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"];
                if (!string.IsNullOrEmpty(token)) token = token.ToString().Split(" ")[1];
                ViewTeamInMatch match = await _teamInMatchService.GetById(id, token);
                return Ok(match);
            }
            catch (NullReferenceException)
            {
                return Ok(new object());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Search teams in match by conditions - all user")]
        public async Task<IActionResult> GetTeamsInMatchByConditions([FromQuery] TeamInMatchRequestModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"];
                if (!string.IsNullOrEmpty(token)) token = token.ToString().Split(" ")[1];
                PagingResult<ViewTeamInMatch> match = await _teamInMatchService.GetByConditions(request, token);
                return Ok(match);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }        

        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Insert result teams in match - Competition manager")]
        public async Task<IActionResult> InsertTeamsInMatch(List<TeamInMatchInsertModel> models)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                List<ViewTeamInMatch> teamsInMatch = await _teamInMatchService.Insert(models, token);
                return Ok(teamsInMatch);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        [HttpPut]
        [Authorize]
        [SwaggerOperation(Summary = "Update result teams in match - Competition manager")]
        public async Task<IActionResult> UpdateTeamInMatch(List<TeamInMatchUpdateModel> models)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _teamInMatchService.Update(models, token);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Delete result team in match - Competition manager")]
        public async Task<IActionResult> DeleteTeamInMatch(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _teamInMatchService.Delete(id, token);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }
    }
}
