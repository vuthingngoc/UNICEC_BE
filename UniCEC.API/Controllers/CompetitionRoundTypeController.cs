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
        private ICompetitionRoundTypeService _competitionRoundTypeService;

        public CompetitionRoundTypeController(ICompetitionRoundTypeService competitionRoundTypeService)
        {
            _competitionRoundTypeService = competitionRoundTypeService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get round type by id - All user")]
        public async Task<IActionResult> GetCompetitionRoundTypeById(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"];
                if (!string.IsNullOrEmpty(token)) token = token.ToString().Split(" ")[1];
                ViewCompetitionRoundType competitionRoundType = await _competitionRoundTypeService.GetById(id, token);
                return Ok(competitionRoundType);
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
        [SwaggerOperation(Summary = "Get round types by conditions - All user")]
        public async Task<IActionResult> GetCompetitionRoundTypesByConditions([FromQuery] CompetitionRoundTypeRequestModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"];
                if (!string.IsNullOrEmpty(token)) token = token.ToString().Split(" ")[1];
                List<ViewCompetitionRoundType> competitionRoundTypes = await _competitionRoundTypeService.GetByConditions(request, token);
                return Ok(competitionRoundTypes);
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
        [SwaggerOperation(Summary = "Insert round type - System admin")]
        public async Task<IActionResult> InsertCompetitionRoundType(CompetitionRoundTypeInsertModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewCompetitionRoundType competitionRoundType = await _competitionRoundTypeService.Insert(model, token);
                return Ok(competitionRoundType);
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
        [SwaggerOperation(Summary = "Update round type - System admin")]
        public async Task<IActionResult> UpdateCompetitionRoundType(CompetitionRoundTypeUpdateModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _competitionRoundTypeService.Update(model, token);
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
        [SwaggerOperation(Summary = "Delete round type - System admin")]
        public async Task<IActionResult> DeleteCompetitionRoundType(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _competitionRoundTypeService.Delete(id, token);
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
