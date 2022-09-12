using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.MemberSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Member;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/members")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class MemberController : ControllerBase 
    {
        private IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Get members by conditions - club member")]
        public async Task<IActionResult> GetMembersByConditions([FromQuery] MemberRequestModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewMember> members = await _memberService.GetByConditions(token, request);
                return Ok(members);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        // GET api/<MemberController>/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get detail member by id - club member")]
        public async Task<IActionResult> GetMemberById(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewDetailMember member = await _memberService.GetByMemberId(token, id);
                return Ok(member);
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
                return StatusCode(500, "Internal server exception");
            }
        }

        [HttpGet("info")]
        [SwaggerOperation(Summary = "Get detail member info by club - user")]
        public async Task<IActionResult> GetMemberInfoByClub(int? clubId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                List<ViewDetailMember> members = await _memberService.GetMemberInfoByClub(token, clubId);
                return Ok(members);
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
                return StatusCode(500, "Internal server exception");
            }
        }

        [HttpGet("leaders/club/{id}")]
        [SwaggerOperation(Summary = "Get top leaders in a club - authenticated user")]
        public async Task<IActionResult> GetLeaders(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                List<ViewIntroClubMember> members = await _memberService.GetLeadersByClub(token, id);
                return Ok(members);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException e)
            {
                return Ok(e.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        [HttpGet("new/quantity/club/{id}")]
        [SwaggerOperation(Summary = "Get new members in current month of a club - club member")]
        public async Task<IActionResult> GetQuantityNewMembers(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                int quantity = await _memberService.GetQuantityNewMembersByClub(token, id);
                return Ok(quantity);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException e)
            {
                return Ok(e.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        // Student apply to club
        [HttpPost("apply/club/{id}")]
        [SwaggerOperation(Summary = "Student apply to a club - student")]
        public async Task<IActionResult> ApplyToBecomeMember(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewMember result = await _memberService.Insert(token, id);
                return Ok(result);
            }
            catch(UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
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

        // Confirm student to become member
        [HttpPut("confirm-member")]
        [SwaggerOperation(Summary = "Confirm member to join club - leader or vice president")]
        public async Task<IActionResult> ConfirmMember([FromBody, BindRequired] ConfirmMemberModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _memberService.ConfirmMember(token, model);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (NullReferenceException ex)
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

        // PUT api/<MemberController>/5
        [HttpPut]
        [SwaggerOperation(Summary = "Update member - Admin, leader or vice president")]
        public async Task<IActionResult> UpdateMember([FromBody, BindRequired] MemberUpdateModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _memberService.Update(token, model);
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

        // DELETE api/<MemberController>/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete member - leader or vice president")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _memberService.Delete(token, id);
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

        [HttpGet("by-club")]
        [SwaggerOperation(Summary = "Get members by clubId - club member")]
        public async Task<IActionResult> GetMembersByClubId([FromQuery(Name = "clubId"), BindRequired] int clubId, [FromQuery(Name = "searchName")] string searchName,
            [FromQuery(Name = "roleId")] int? roleId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                List<ViewMember> members = await _memberService.GetMembersByClub(token, clubId, searchName, roleId);
                if(members == null)
                {
                    Ok(new List<Object>());
                }
                return Ok(members);
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
                return StatusCode(500, "Internal server exception");
            }
        }
    }
}
