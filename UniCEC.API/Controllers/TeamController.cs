using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.TeamSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ParticipantInTeam;
using UniCEC.Data.ViewModels.Entities.Team;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/teams")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TeamController : ControllerBase
    {
        //
        private ITeamService _teamService;


        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }


        // GET api/<TeamController>
        [Authorize(Roles = "Student")]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all list team in competition")]
        public async Task<IActionResult> GetAllTeamInCompetition([FromQuery] TeamRequestModel request)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                PagingResult<ViewTeam> result = await _teamService.GetAllTeamInCompetition(request, token);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }


        // GET api/<TeamController>
        [Authorize(Roles = "Student")]
        [HttpGet("detail")]
        [SwaggerOperation(Summary = "Get detail a team in competition")]
        public async Task<IActionResult> GetDetailTeamInCompetition([FromQuery(Name = "teamId"), BindRequired] int teamId, [FromQuery(Name = "competitionId"), BindRequired] int competititonId)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewDetailTeam result = await _teamService.GetDetailTeamInCompetition(teamId, competititonId, token);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new object());
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        [HttpGet("{id}/competition/{competition-id}")]
        [SwaggerOperation(Summary = "Get total results of a team in competition - Authenticated user")]
        [Authorize]
        public async Task<IActionResult> GetTotalResultTeamInCompetition(int id, [FromRoute(Name = "competition-id")] int competitionId)
        {
            try
            {
                ViewTeamInCompetition totalResultTeamInCompetition = await _teamService.GetTotalResultTeamInCompetition(competitionId, id);
                return Ok(totalResultTeamInCompetition);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException)
            {
                return Ok(new object());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exeption");
            }
        }

        [HttpGet("final-result-competition")]
        [SwaggerOperation(Summary = "Get final result all teams in a competition - Authenticated user")]
        [Authorize]
        public async Task<IActionResult> GetResultTeamsInCompetition([FromQuery, BindRequired] int competitionId, int? top)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                List<ViewResultTeam> teams = await _teamService.GetFinalResultTeamsInCompetition(token, competitionId, top);
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

        // POST api/<TeamController>
        [Authorize(Roles = "Student")]
        [HttpPost()]
        [SwaggerOperation(Summary = "Insert team in competition - Student")]
        public async Task<IActionResult> InsertTeam([FromBody] TeamInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewTeam result = await _teamService.InsertTeam(model, token);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }


        // POST api/<TeamController>
        [Authorize(Roles = "Student")]
        [HttpPost("add-participant")]
        [SwaggerOperation(Summary = "Invited member join in team by Invited Code - Student")]
        public async Task<IActionResult> InsertMemberInTeam([FromBody] ParticipantInTeamInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewParticipantInTeam result = await _teamService.InsertMemberInTeam(model, token);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }


        // PUT api/<CompetitionController>/5
        [Authorize(Roles = "Student")]
        [HttpPut("team-role")]
        [SwaggerOperation(Summary = "Update team role by Leader - Student")]
        public async Task<IActionResult> UpdateTeamRole([FromBody] ParticipantInTeamUpdateModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _teamService.UpdateTeamRole(model, token);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        // PUT api/<CompetitionController>/5
        [Authorize(Roles = "Student")]
        [HttpPut()]
        [SwaggerOperation(Summary = "Update team by Leader - Student")]
        public async Task<IActionResult> UpdateTeam([FromBody] TeamUpdateModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _teamService.UpdateTeam(model, token);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        // PUT api/<CompetitionController>/5
        [Authorize(Roles = "Student")]
        [HttpPut("manager-lock-team")]
        [SwaggerOperation(Summary = "Competition Manager lock team")]
        public async Task<IActionResult> CompetitionManagerLockTeam(LockTeamModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _teamService.CompetitionManagerLockTeam(model, token);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        [Authorize(Roles = "Student")]
        [HttpDelete("team")]
        [SwaggerOperation(Summary = "Delete team by leader")]
        public async Task<IActionResult> DeleteByLeader([FromQuery(Name = "teamId"), BindRequired] int teamId)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _teamService.DeleteByLeader(teamId, token);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        [Authorize(Roles = "Student")]
        [HttpDelete("team/member")]
        [SwaggerOperation(Summary = "Delete member in team by leader")]
        public async Task<IActionResult> DeleteMemberByLeader([FromQuery(Name = "teamId"), BindRequired] int teamId, [FromQuery(Name = "participantId"), BindRequired] int participantId)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _teamService.DeleteMemberByLeader(teamId, participantId, token);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
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


        [Authorize(Roles = "Student")]
        [HttpDelete("member-out-team")]
        [SwaggerOperation(Summary = "member out team")]
        public async Task<IActionResult> OutTeam([FromQuery(Name = "teamId"), BindRequired] int teamId)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _teamService.OutTeam(teamId, token);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
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
