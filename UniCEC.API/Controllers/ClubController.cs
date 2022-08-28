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
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Club;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/clubs")]
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
        [SwaggerOperation(Summary = "Get club by id - Authenticated user in the university")]
        public async Task<IActionResult> GetClubById(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewClub club = await _clubService.GetById(token, id);
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

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Get club by conditions - Authenticated user in the university")]
        public async Task<IActionResult> GetClubByConditions([FromQuery] ClubRequestModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewClub> clubs = await _clubService.GetByConditions(token, request);
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

        [HttpGet("all-club-by-uni")]
        [SwaggerOperation(Summary = "Get club by uni - Authenticated user in the university")]
        public async Task<IActionResult> GetClubByUni()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                List<ViewClub> clubs = await _clubService.GetClubByUni(token);
                return Ok(clubs);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exeption");
            }
        }

        [HttpGet("club-manager/search")]
        [SwaggerOperation(Summary = "Get club by conditions - Authenticated user in the university")]
        public async Task<IActionResult> SearchClubsByManager([FromQuery] ClubRequestByManagerModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewClub> clubs = await _clubService.GetByManager(token, request);
                return Ok(clubs);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exeption");
            }
        }

        [HttpGet("user/{id}")]
        [SwaggerOperation(Summary = "Get club of this user - Student")]
        public async Task<IActionResult> GetClubByUser(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                List<ViewClub> clubs = await _clubService.GetByUser(token, id);
                return Ok(clubs);
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
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpGet("competition/{id}")]
        [SwaggerOperation(Summary = "Get club by competition - Authenticated user in scope")]
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
        [SwaggerOperation(Summary = "Insert club - Admin")]
        [Authorize(Roles = "University Admin")]
        public async Task<IActionResult> InsertClub([FromBody] ClubInsertModel club)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewClub viewClub = await _clubService.Insert(token, club);
                return Ok(viewClub);
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
        [SwaggerOperation(Summary = "Update club - leader and vice president")]
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

        [HttpPut("{id}/status")]
        [Authorize(Roles = "University Admin")]
        [SwaggerOperation(Summary = "Update status of a club - Admin")]
        public async Task<IActionResult> UpdateStatusClub(int id, [FromQuery, BindRequired] bool status)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _clubService.Update(token, id, status);
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
        [Authorize(Roles = "University Admin")]
        [SwaggerOperation(Summary = "Delete club - Admin")]
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


        [HttpGet("activity-of-club")]
        [SwaggerOperation(Summary = "Get activity of club by id - Authenticated user in the university")]
        public async Task<IActionResult> GetActivityOfClubById([FromQuery(Name = "clubId"), BindRequired]int clubId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewActivityOfClubModel clubActivity = await _clubService.GetActivityOfClubById(token, clubId);
                return Ok(clubActivity);
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
