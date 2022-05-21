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
using UniCEC.Data.ViewModels.Entities.Term;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/club-history")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class ClubHistoryController : ControllerBase
    {
        private IClubHistoryService _clubHistoryService;

        public ClubHistoryController(IClubHistoryService clubHistoryService)
        {
            _clubHistoryService = clubHistoryService;
        }

        //[HttpGet("club/{id}")]
        //[SwaggerOperation(Summary = "Get all history member of a club")]
        //public async Task<IActionResult> GetAllHistoryByClub(int id, [FromQuery] PagingRequest request)
        //{
        //    try
        //    {
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];
        //        PagingResult<ViewClubHistory> clubHistories = await _clubHistoryService.GetAllPaging(id, token, request);
        //        return (clubHistories != null) ? Ok(clubHistories) : BadRequest("You don't have permission to see history of the club");
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal Server Exception");
        //    }
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetClubHistoryById(int id)
        //{
        //    try
        //    {
        //        ViewClubHistory clubHistory = await _clubHistoryService.GetByClubHistory(id);
        //        return Ok(clubHistory);
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal Server Exception");
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //}

        [HttpGet("club/{id}/search")]
        [SwaggerOperation(Summary = "Search history member of a club")]
        public async Task<IActionResult> GetClubHistoryByConditions(int id, [FromQuery] ClubHistoryRequestModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewClubHistory> clubHistories = await _clubHistoryService.GetByContitions(token, id, request);
                return Ok(clubHistories);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (NullReferenceException ex)
            {
                return Ok(ex.Message);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        //[HttpGet("club/{id}/term/{term-id}")]
        //[SwaggerOperation(Summary = "Get history member in the term of a club")]
        //public async Task<IActionResult> GetMembersInTermByClub(int id, [FromRoute(Name = "term-id")] int termId, [FromQuery] PagingRequest request)
        //{
        //    try
        //    {
        //        PagingResult<ViewClubMember> clubMembers = await _clubHistoryService.GetMembersByClub(id, termId, request);
        //        return Ok(clubMembers);
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal Server Exception");
        //    }
        //}

        [HttpPost("{id}")]
        [SwaggerOperation(Summary = "Insert all members of a club with new term")]
        public async Task<IActionResult> InsertNewTermClubHistory(int id, [FromBody] TermInsertModel termModel) // Testing ...
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _clubHistoryService.InsertForNewTerm(token, id, termModel);
                return Ok();
            }
            catch(UnauthorizedAccessException ex)
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
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        //[HttpPut]
        //public async Task<IActionResult> UpdateClubHistory([FromBody] ClubHistoryUpdateModel clubHistory)
        //{
        //    try
        //    {
        //        await _clubHistoryService.Update(clubHistory);
        //        return Ok();
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Internal Server Exception");
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal Server Exception");
        //    }
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteClubHistory(int id)
        //{
        //    try
        //    {
        //        await _clubHistoryService.Delete(id);
        //        return NoContent();
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Internal Server Exception");
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal Server Exception");
        //    }
        //}

        // Tien Anh
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
