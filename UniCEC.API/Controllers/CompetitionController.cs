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
        [SwaggerOperation(Summary = "Get EVENT or COMPETITION by conditions, 0.HappenningSoon, 1.Registering , 2.Happening , 3.Ending , 4.Canceling , 5.NotAssigned")]
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
        //[Authorize(Roles = "Student")]
        [HttpPost("leader")]
        [SwaggerOperation(Summary = "Leader insert EVENT or COMPETITON, if Event please put value at number-of-group = 0 ")]
        //phải có author student
        public async Task<IActionResult> InsertByLeader([FromBody] LeaderInsertCompOrEventModel model)
        {
            try
            {
                ViewCompetition viewCompetition = await _competitionService.LeaderInsert(model);
                if (viewCompetition != null)
                {

                    return Ok(viewCompetition);
                }
                else
                {
                    return BadRequest();
                }
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
        //[Authorize(Roles = "Sponsor")]
        [HttpPost("sponsor")]
        [SwaggerOperation(Summary = "Sponsor insert EVENT or COMPETITON, if Event please put value at number-of-group = 0 ")]
        //phải có author student
        public async Task<IActionResult> InsertBySposor([FromBody] SponsorInsertCompOrEventModel model)
        {
            try
            {
                ViewCompetition viewCompetition = await _competitionService.SponsorInsert(model);
                if (viewCompetition != null)
                {

                    return Ok(viewCompetition);
                }
                else
                {
                    return BadRequest();
                }
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
        //[Authorize(Roles = "Student")]
        [HttpPut("leader")]
        [SwaggerOperation(Summary = "Leader update detail EVENT or COMPETITON")]
        public async Task<IActionResult> Update([FromBody] CompetitionUpdateModel model)
        {
            try
            {
                Boolean check = false;
                check = await _competitionService.LeaderUpdate(model);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
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
        //[Authorize(Roles = "Student")]
        [HttpDelete("leader")]
        [SwaggerOperation(Summary = "Leader canceling EVENT or COMPETITION")]
        public async Task<IActionResult> Delete([FromBody] CompetitionDeleteModel model)
        {
            try
            {
                Boolean check = false;
                check = await _competitionService.LeaderDelete(model);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
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
