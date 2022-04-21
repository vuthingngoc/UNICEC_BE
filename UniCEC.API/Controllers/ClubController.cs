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
        public async Task<IActionResult> GetAllClub(PagingRequest request)
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
        public async Task<IActionResult> GetClubByName(string name)
        {
            try
            {
                List<ViewClub> clubs = await _clubService.GetByName(name);
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


        [HttpGet("competition/{id}")]
        public async Task<IActionResult> GetClubByCompetition(int id)
        {
            try
            {
                List<ViewClub> clubs = await _clubService.GetByCompetition(id);
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
        public async Task<IActionResult> InsertClub(ClubInsertModel club)
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
        public async Task<IActionResult> UpdateClub(ClubUpdateModel club)
        {
            try
            {
                bool result = await _clubService.Update(club);
                return (result) ? Ok() : StatusCode(500, "Internal Server Exception");
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
                bool result = await _clubService.Delete(id);
                return (result) ? NoContent() : StatusCode(500, "Internal Server Exception");
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
