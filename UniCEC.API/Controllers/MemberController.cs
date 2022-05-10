using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class MemberController : ControllerBase
    {
        //
        private IMemberService _ImemberService;
        public MemberController(IMemberService ImemberService)
        {
            _ImemberService = ImemberService;
        }

        [HttpGet("club/{id}")]
        [SwaggerOperation(Summary = "Get all members in a club")]
        public async Task<IActionResult> GetAllMembersByClub(int id, [FromQuery] PagingRequest request)
        {
            try
            {
                PagingResult<ViewMember> members = await _ImemberService.GetAllPaging(id, request);
                return Ok(members);
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
        [SwaggerOperation(Summary = "Get member by id")]
        public async Task<IActionResult> GetMemberById(int id)
        {
            try
            {
                ViewMember member = await _ImemberService.GetByMemberId(id);
                return Ok(member);
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
        [SwaggerOperation(Summary = "Get top leader in a club")]
        public async Task<IActionResult> GetLeaders(int id)
        {
            try
            {
                List<ViewMember> members = await _ImemberService.GetLeadersByClub(id);
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
        [SwaggerOperation(Summary = "Get new members in a club")]
        public async Task<IActionResult> GetQuantityNewMembers(int id)
        {
            try
            {
                int quantity = await _ImemberService.GetQuantityNewMembersByClub(id);
                return Ok(quantity);
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
                ViewMember result = await _ImemberService.Insert(model);
                return Ok(result);
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
        [SwaggerOperation(Summary = "Update member")]
        public async Task<IActionResult> UpdateMember([FromBody] MemberUpdateModel model)
        {
            try
            {
                await _ImemberService.Update(model);
                return Ok();
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
                await _ImemberService.Delete(id);
                return Ok();
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
