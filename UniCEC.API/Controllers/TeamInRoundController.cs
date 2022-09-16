using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.TeamInRoundSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Team;
using UniCEC.Data.ViewModels.Entities.TeamInRound;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/teams-in-round")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]

    public class TeamInRoundController : ControllerBase
    {
        private ITeamInRoundService _teamInRoundService;

        public TeamInRoundController(ITeamInRoundService teamInRoundService)
        {
            _teamInRoundService = teamInRoundService;
        }

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Search teams in round by conditions - Authenticated user")]
        public async Task<IActionResult> GetRoundByConditions([FromQuery] TeamInRoundRequestModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult< ViewTeamInRound> teamInRounds = await _teamInRoundService.GetByConditions(token, request);
                return Ok(teamInRounds);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exeption");
            }
        }

        [HttpGet("total-result")]
        [SwaggerOperation(Summary = "Get total result of teams in a competition - Authenticated user")]
        public async Task<IActionResult> GetResultTeamsInCompetition([FromQuery, BindRequired] int competitionId, int top)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                List<ViewResultTeam> teams = await _teamInRoundService.GetResultTeamsInCompetition(token, competitionId, top);
                return Ok(teams);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exeption");
            }
        }

        [HttpGet("next-round")]
        [SwaggerOperation(Summary = "Get teams that continue to participate next round - Competition manager")]
        public async Task<IActionResult> GetTopTeamsToNextRound([FromQuery, BindRequired] int roundId, [FromQuery, BindRequired]  int top)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                List<ViewTeamInRound> teams = await _teamInRoundService.GetTopTeamsToNextRound(token, roundId, top);
                return Ok(teams);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exeption");
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Insert teams in round - Competition manager")]
        public async Task<IActionResult> InsertTeamInClub([FromBody] List<TeamInRoundInsertModel> models)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                List<ViewTeamInRound> teamInRounds = await _teamInRoundService.Insert(token, models);
                return Ok(teamInRounds);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update team in round - Competition manager")]
        public async Task<IActionResult> UpdateTeamInRound([FromBody] TeamInRoundUpdateModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _teamInRoundService.Update(token, model);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return Ok(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete team in round - Competition manager")]
        public async Task<IActionResult> DeleteTeamInRound(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _teamInRoundService.Delete(token, id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return Ok(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
