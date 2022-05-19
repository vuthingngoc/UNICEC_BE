using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.CompetitionSvc;
using UniCEC.Data.Enum;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;
using UniCEC.Data.ViewModels.Entities.SponsorInCompetition;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/competition")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CompetitionController : ControllerBase
    {
        private ICompetitionService _competitionService;

        public CompetitionController(ICompetitionService competitionService)
        {
            _competitionService = competitionService;
        }

        // GET: api/<MemberTakesActivityController>
        [HttpGet]
        [SwaggerOperation(Summary = "Get EVENT or COMPETITION by conditions, 0.Launching, 1.HappenningSoon, 2.Registering, 3.Happening, 4.Ending, 5.Canceling, 6.NotAssigned")]
        public async Task<IActionResult> GetCompOrEve([FromQuery] CompetitionRequestModel request)
        {
            try
            {
                PagingResult<ViewCompetition> result = await _competitionService.GetCompOrEve(request);

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    //Not has data
                    return Ok(new List<object>());
                }
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        // GET: api/<MemberTakesActivityController>
        [HttpGet("top3")]
        [SwaggerOperation(Summary = "Get top 3 EVENT or COMPETITION by club, status, public")]
        public async Task<IActionResult> GetTop3CompOrEve([FromQuery(Name = "clubId")] int? ClubId, [FromQuery(Name = "event")] bool? Event, [FromQuery(Name = "status")] CompetitionStatus? Status, [FromQuery(Name = "public")] bool? Public)
        {
            try
            {
                List<ViewCompetition> result = await _competitionService.GetTop3CompOrEve(ClubId, Event, Status, Public);

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    //Not has data
                    return Ok(new List<object>());
                }
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        // GET api/<CompetitionController>/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get detail of EVENT or COMPETITON by id")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                ViewCompetition result = await _competitionService.GetById(id);
                if (result == null)
                {
                    //Not has data
                    return Ok(new object());
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        //ClubLeader
        // POST api/<CompetitionController>
        [Authorize(Roles = "Student")]
        [HttpPost("leader")]
        [SwaggerOperation(Summary = "Leader insert EVENT or COMPETITON, if Event please put value at number-of-group = 0 ")]
        //phải có author student
        public async Task<IActionResult> Insert([FromBody] LeaderInsertCompOrEventModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewCompetition viewCompetition = await _competitionService.LeaderInsert(model, token);
                if (viewCompetition != null)
                {

                    return Ok(viewCompetition);
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
                return StatusCode(500, "Internal Server Exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        //Sponsor
        // POST api/<CompetitionController>
        [Authorize(Roles = "Sponsor")]
        [HttpPost("sponsor")]
        [SwaggerOperation(Summary = "Sponsor insert EVENT or COMPETITON, if Event please put value at number-of-group = 0 ")]
        //phải có author student
        public async Task<IActionResult> Insert([FromBody] SponsorInsertCompOrEventModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewCompetition viewCompetition = await _competitionService.SponsorInsert(model, token);
                if (viewCompetition != null)
                {

                    return Ok(viewCompetition);
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
                return StatusCode(500, "Internal Server Exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }


        // PUT api/<CompetitionController>/5
        [Authorize(Roles = "Student")]
        [HttpPut("leader")]
        [SwaggerOperation(Summary = "Leader update detail EVENT or COMPETITON")]
        public async Task<IActionResult> Update([FromBody] LeaderUpdateCompOrEventModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _competitionService.LeaderUpdate(model, token);
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
        [Authorize(Roles = "Sponsor")]
        [HttpPut("sponsor")]
        [SwaggerOperation(Summary = "Sponsor update detail EVENT or COMPETITON")]
        public async Task<IActionResult> Update([FromBody] SponsorUpdateCompOrEvent model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _competitionService.SponsorUpdate(model, token);
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

        // DELETE api/<CompetitionController>/5
        [Authorize(Roles = "Student")]
        [HttpDelete("leader")]
        [SwaggerOperation(Summary = "Leader canceling EVENT or COMPETITION")]
        public async Task<IActionResult> Delete([FromBody] LeaderDeleteCompOrEventModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _competitionService.LeaderDelete(model, token);
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

        [Authorize(Roles = "Sponsor")]
        [HttpDelete("sponsor")]
        [SwaggerOperation(Summary = "Sponsor canceling EVENT or COMPETITION")]
        public async Task<IActionResult> Delete([FromBody] SponsorDeleteCompOrEventModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _competitionService.SponsorDelete(model, token);
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

        //---------------------------------------------------------------------------Competition In Club
        //POST api/<CompetitionInClubController>
        [Authorize(Roles = "Student")]
        [HttpPost("add-club-collaborate")]
        [SwaggerOperation(Summary = "add another club in competition")]
        public async Task<IActionResult> AddClubCollaborate([FromBody] CompetitionInClubInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewCompetitionInClub result = await _competitionService.AddClubCollaborate(model,token);
                if (result != null)
                {

                    return Ok(result);
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

        //---------------------------------------------------------------------------Sponsor in Competition
        //POST api/<SponsorInCompetitionController>
        [Authorize(Roles = "Sponsor")]
        [HttpPost("add-sponsor-collaborate")]
        [SwaggerOperation(Summary = "Add another sponsor in competition")]
        public async Task<IActionResult> AddSponsorCollaborate([FromBody] SponsorInCompetitionInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewSponsorInCompetition result = await _competitionService.AddSponsorCollaborate(model,token);
                if (result != null)
                {

                    return Ok(result);
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
