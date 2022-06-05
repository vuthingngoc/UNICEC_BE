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
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Member;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/member")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class MemberController : ControllerBase // not authorize role in club yet !!!
    {
        private IMemberService _memberService;

        public MemberController(IMemberService ImemberService)
        {
            _memberService = ImemberService;
        }

        [HttpGet("club/{id}")]
        [SwaggerOperation(Summary = "Get all members in a club")]
        public async Task<IActionResult> GetAllMembersByClub(int id, [FromQuery] PagingRequest request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                PagingResult<ViewMember> members = await _memberService.GetByClub(token, id, request);
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
        [SwaggerOperation(Summary = "Get detail member by id")]
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

        [HttpGet("leaders/club/{id}")]
        [SwaggerOperation(Summary = "Get top leaders in a club")]
        public async Task<IActionResult> GetLeaders(int id)
        {
            try
            {
                List<ViewIntroClubMember> members = await _memberService.GetLeadersByClub(id);
                return Ok(members);
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
        [SwaggerOperation(Summary = "Get new members in current month of a club")]
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

        // Add Member In Club
        [HttpPost]
        [SwaggerOperation(Summary = "Insert member")]
        public async Task<IActionResult> InsertMember([FromBody] MemberInsertModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                ViewMember result = await _memberService.Insert(token, model);
                return Ok(result);
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

        //[HttpPost("update-new-term")]
        //[SwaggerOperation(Summary = "Update new term of a club")]
        //public async Task<IActionResult> UpdateNewTerm([FromBody] MemberInsertModel model)
        //{
        //    try
        //    {
        //        string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
        //        ViewMember result = await _memberService.Insert(token, model);
        //        return Ok(result);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //    catch (NullReferenceException ex)
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

        // PUT api/<MemberController>/5
        [HttpPut]
        [SwaggerOperation(Summary = "Update member")]
        public async Task<IActionResult> UpdateMember([FromBody] MemberUpdateModel model)
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
        [SwaggerOperation(Summary = "Delete member")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _memberService.Delete(token, id);
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
    }
}
