using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.TeamInRoundSvc;
using UniCEC.Data.ViewModels.Entities.TeamInRound;

namespace UniCEC.API.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get team in round by id - Authenticated user")]
        public async Task<IActionResult> GetRoundById(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewTeamInRound teamInRound = await _teamInRoundService.GetById(token, id);
                return Ok(teamInRound);
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
    }
}
