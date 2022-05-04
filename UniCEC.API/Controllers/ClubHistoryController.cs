using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.ClubHistorySvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubHistory;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/club-history")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ClubHistoryController : ControllerBase
    {
        private IClubHistoryService _clubHistoryService;
        public ClubHistoryController(IClubHistoryService clubHistoryService)
        {
            _clubHistoryService = clubHistoryService;
        }

        [HttpGet("club/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get all history member of a club")]
        public async Task<IActionResult> GetAllHistoryByClub(int id, [FromQuery] PagingRequest request)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewClubHistory> clubHistories = await _clubHistoryService.GetAllPaging(id, token, request);
                return (clubHistories != null) ? Ok(clubHistories) : BadRequest("You don't have permission to see history of the club");
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
        public async Task<IActionResult> GetClubHistoryById(int id)
        {
            try
            {
                ViewClubHistory clubHistory = await _clubHistoryService.GetByClubHistory(id);
                return Ok(clubHistory);
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
        public async Task<IActionResult> GetClubHistoryByConditions([FromQuery] ClubHistoryRequestModel request)
        {
            try
            {
                PagingResult<ViewClubHistory> clubHistories = await _clubHistoryService.GetByContitions(request);
                return Ok(clubHistories);
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

        [HttpGet("club/{id}/term/{term-id}")]
        public async Task<IActionResult> GetMembersByClub(int id, [FromRoute(Name = "term-id")] int termId, [FromQuery] PagingRequest request)
        {
            try
            {
                PagingResult<ViewClubMember> clubMembers = await _clubHistoryService.GetMembersByClub(id, termId, request);
                return Ok(clubMembers);
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

        [HttpPost]
        public async Task<IActionResult> InsertClubHistory([FromBody] ClubHistoryInsertModel clubHistory)
        {
            try
            {
                ViewClubHistory viewClubHistory = await _clubHistoryService.Insert(clubHistory);
                return Created($"api/v1/[controller]/{viewClubHistory.Id}", viewClubHistory);
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

        [HttpPut]
        public async Task<IActionResult> UpdateClubHistory([FromBody] ClubHistoryUpdateModel clubHistory)
        {
            try
            {
                await _clubHistoryService.Update(clubHistory);
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
        public async Task<IActionResult> DeleteClubHistory(int id)
        {
            try
            {
                await _clubHistoryService.Delete(id);
                return NoContent();
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

        //
        [HttpGet("member-in-club")]
        [SwaggerOperation(Summary = "Get info member in club")]
        public async Task<IActionResult> GetMemberInClub([FromQuery] GetMemberInClubModel model)
        {
            try
            {
                ViewClubMember viewClubMember = await _clubHistoryService.GetMemberInCLub(model);
                if (viewClubMember != null)
                {
                    return Ok(viewClubMember);
                }
                else
                {
                    return BadRequest();
                }
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

    }
}
