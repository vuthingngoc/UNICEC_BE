using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.CompetitionRoundTypeSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Entities.CompetitionRoundType;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/competition-round-types")]
    [ApiController]
    [ApiVersion("1.0")]

    public class CompetitionRoundTypeController : ControllerBase
    {
        private ICompetitionRoundTypeService _matchTypeService;

        public CompetitionRoundTypeController(ICompetitionRoundTypeService matchTypeService)
        {
            _matchTypeService = matchTypeService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get match type by id - All user")]
        public async Task<IActionResult> GetMatchTypeById(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"];
                if (!string.IsNullOrEmpty(token)) token = token.ToString().Split(" ")[1];
                ViewCompetitionRoundType matchType = await _matchTypeService.GetById(id, token);
                return Ok(matchType);
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
        [SwaggerOperation(Summary = "Get match types by conditions - All user")]
        public async Task<IActionResult> GetMatchTypesByConditions([FromQuery] CompetitionRoundTypeRequestModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"];
                if (!string.IsNullOrEmpty(token)) token = token.ToString().Split(" ")[1];
                List<ViewCompetitionRoundType> matchTypes = await _matchTypeService.GetByConditions(request, token);
                return Ok(matchTypes);
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

        [HttpPost]
        [Authorize(Roles = "System Admin")]
        [SwaggerOperation(Summary = "Insert match type - System admin")]
        public async Task<IActionResult> InsertMatchType(CompetitionRoundTypeInsertModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewCompetitionRoundType matchType = await _matchTypeService.Insert(model, token);
                return Ok(matchType);
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
        [Authorize(Roles = "System Admin")]
        [SwaggerOperation(Summary = "Update match type - System admin")]
        public async Task<IActionResult> UpdateMatchType(CompetitionRoundTypeUpdateModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _matchTypeService.Update(model, token);
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
        [Authorize(Roles = "System Admin")]
        [SwaggerOperation(Summary = "Delete match type - System admin")]
        public async Task<IActionResult> DeleteMatchType(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _matchTypeService.Delete(id, token);
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
