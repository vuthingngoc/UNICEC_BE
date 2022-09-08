using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.MatchSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Match;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/matches")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MatchController : ControllerBase
    {
        private IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get match by id - All user")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"];
                if (!string.IsNullOrEmpty(token)) token = token.ToString().Split(" ")[1];
                ViewMatch match = await _matchService.GetById(id, token);
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
        [SwaggerOperation(Summary = "Search match by conditions - All user")]
        public async Task<IActionResult> GetMatchByConditions([FromQuery] MatchRequestModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"];
                if (!string.IsNullOrEmpty(token)) token = token.ToString().Split(" ")[1];
                PagingResult<ViewMatch> match = await _matchService.GetByConditions(request, token);
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
        [SwaggerOperation(Summary = "Insert match - Competition manager")]
        public async Task<IActionResult> InsertMatch(MatchInsertModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewMatch match = await _matchService.Insert(model, token);
                return Ok(match);
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
        [SwaggerOperation(Summary = "Update match - Competition manager")]
        public async Task<IActionResult> UpdateMatch(MatchUpdateModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _matchService.Update(model, token);
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
        [SwaggerOperation(Summary = "Delete match - Competition manager")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _matchService.Delete(id, token);
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
