using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.ClubSvc;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Club;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ClubController : ControllerBase
    {
        private IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClub([FromQuery] PagingRequest request)
        {
            try
            {
                PagingResult<ViewClub> clubs = await _clubService.GetAllPaging(request);
                return Ok(clubs);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exeption");
            }            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClubById(int id)
        {
            try
            {
                ViewClub club = await _clubService.GetByClub(id);
                return Ok(club);
            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exeption");
            }
        }

        [HttpGet("name")]
        public async Task<IActionResult> GetClubByName(string name, [FromQuery] PagingRequest request)
        {
            try
            {
                PagingResult<ViewClub> clubs = await _clubService.GetByName(name, request);
                return Ok(clubs);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exeption");
            }
        }

        [HttpGet("user/{id}")]
        //[Authorize(Roles = "System Admin")]
        public async Task<IActionResult> GetClubByUser(int id)
        {
            try
            {
                List<ViewClub> clubs = await _clubService.GetByUser(id);
                return Ok(clubs);
            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }


        [HttpGet("competition/{id}")]
        public async Task<IActionResult> GetClubByCompetition(int id, [FromQuery] PagingRequest request)
        {
            try
            {
                PagingResult<ViewClub> clubs = await _clubService.GetByCompetition(id, request);
                return Ok(clubs);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exeption");
            }
        }


        [HttpPost]
        public async Task<IActionResult> InsertClub([FromBody] ClubInsertModel club)
        {
            try
            {
                ViewClub viewClub = await _clubService.Insert(club);
                return Created("api/v1/[controller]", club); 
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
        public async Task<IActionResult> UpdateClub([FromBody] ClubUpdateModel club)
        {
            try
            {
                await _clubService.Update(club);
                return Ok();
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
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
        public async Task<IActionResult> DeleteClub(int id)
        {
            try
            {
                await _clubService.Delete(id);
                return NoContent();
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
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
