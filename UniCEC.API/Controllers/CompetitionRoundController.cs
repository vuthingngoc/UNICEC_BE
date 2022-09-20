using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.CompetitionRoundSvc;
using UniCEC.Data.Enum;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionRound;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/competition-rounds")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]

    public class CompetitionRoundController : ControllerBase
    {
        private ICompetitionRoundService _competitionRoundService;

        public CompetitionRoundController(ICompetitionRoundService competitionRoundService)
        {
            _competitionRoundService = competitionRoundService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get competition round by id - Authenticated user")]
        public async Task<IActionResult> GetRoundById(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewCompetitionRound competitionRound = await _competitionRoundService.GetById(token, id);
                return Ok(competitionRound);
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

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Get competition round by conditions - Authenticated user")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoundByConditions([FromQuery] CompetitionRoundRequestModel request)
        {
            try
            {
                PagingResult<ViewCompetitionRound> competitionRounds = await _competitionRoundService.GetByConditions(request);
                return Ok(competitionRounds);
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

        [HttpPost("")]
        [SwaggerOperation(Summary = "Insert competition round - Competition manager")]
        public async Task<IActionResult> InsertRound([FromBody] List<CompetitionRoundInsertModel> models)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                List< ViewCompetitionRound> competitionRound = await _competitionRoundService.Insert(token, models);
                return Ok(competitionRound);
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

        [HttpPut("")]
        [SwaggerOperation(Summary = "Update competition round - Competition manager")]
        public async Task<IActionResult> UpdateRound([FromBody] CompetitionRoundUpdateModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _competitionRoundService.Update(token, model);
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
        [SwaggerOperation(Summary = "Delete competition round - Competition manager")]
        public async Task<IActionResult> DeleteRound(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _competitionRoundService.Delete(token, id);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(NullReferenceException ex)
            {
                return BadRequest(ex.Message);
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
    }
}
