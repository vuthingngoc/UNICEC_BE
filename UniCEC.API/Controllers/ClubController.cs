using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.ClubSvc;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Club;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/club")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class ClubController : ControllerBase
    {
        private IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get club by id")]
        public async Task<IActionResult> GetClubById(int id, [FromQuery, BindRequired] int universityId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewClub club = await _clubService.GetByClub(token, id, universityId);
                return Ok(club);
            }
            catch(UnauthorizedAccessException ex)
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

        [HttpGet("university/{id}/search")]
        [SwaggerOperation(Summary = "Get club by name")]
        public async Task<IActionResult> GetClubByName(int id, [FromQuery] string name, [FromQuery] PagingRequest request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewClub> clubs = await _clubService.GetByName(token, id, name, request);
                return Ok(clubs);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exeption");
            }
        }

        [HttpGet("user")]
        [SwaggerOperation(Summary = "Get club by user")]
        public async Task<IActionResult> GetClubByUser()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                List<ViewClub> clubs = await _clubService.GetByUser(token);
                return Ok(clubs);
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

        [HttpGet("university/{id}")]
        [SwaggerOperation(Summary = "Get club by university of user")]
        public async Task<IActionResult> GetClubByUniversity(int id, [FromQuery] PagingRequest request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewClub> clubs = await _clubService.GetByUniversity(token, id, request);
                return Ok(clubs);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return Ok(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpGet("competition/{id}")]
        [SwaggerOperation(Summary = "Get club by competition")]
        public async Task<IActionResult> GetClubByCompetition(int id, [FromQuery] PagingRequest request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewClub> clubs = await _clubService.GetByCompetition(token, id, request);
                return Ok(clubs);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return Ok(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exeption");
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Insert club")]
        public async Task<IActionResult> InsertClub([FromBody] ClubInsertModel club)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewClub viewClub = await _clubService.Insert(token, club);
                return Ok(club);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentNullException ex)
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

        [HttpPut]
        [SwaggerOperation(Summary = "Update club")]
        public async Task<IActionResult> UpdateClub([FromBody] ClubUpdateModel club)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _clubService.Update(token, club);
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
            catch (ArgumentNullException ex)
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

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete club")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _clubService.Delete(token, id);
                return NoContent();
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
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
