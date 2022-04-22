using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.ClubPreviousSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubPrevious;

namespace UniCEC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubPreviousController : ControllerBase
    {
        private IClubPreviousService _clubPreviousService;
        public ClubPreviousController(IClubPreviousService clubPreviousService)
        {
            _clubPreviousService = clubPreviousService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClubPrevious([FromQuery] PagingRequest request)
        {
            try
            {
                PagingResult<ViewClubPrevious> previousClubs = await _clubPreviousService.GetAllPaging(request);
                return Ok(previousClubs);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClubPreviousById(int id)
        {
            try
            {
                ViewClubPrevious clubPrevious = await _clubPreviousService.GetByClubPrevious(id);
                return Ok(clubPrevious);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetClubPreviousByConditions([FromQuery] ClubPreviousRequestModel request)
        {
            try
            {
                PagingResult<ViewClubPrevious> previousClubs = await _clubPreviousService.GetByContitions(request);
                return Ok(previousClubs);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertClubPrevious(ClubPreviousInsertModel clubPrevious)
        {
            try
            {
                ViewClubPrevious viewClubPrevious = await _clubPreviousService.Insert(clubPrevious);
                return Created($"api/v1/[controller]/{viewClubPrevious.Id}", viewClubPrevious);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(ArgumentException ex)
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

        [HttpPut]
        public async Task<IActionResult> UpdateClubPrevious(ClubPreviousUpdateModel clubPrevious)
        {
            try
            {
                bool result = await _clubPreviousService.Update(clubPrevious);
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
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClubPrevious(int id)
        {
            try
            {
                bool result = await _clubPreviousService.Delete(id);
                return (result) ? NoContent() : StatusCode(500, "Internal Server Exception");
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
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

    }
}
