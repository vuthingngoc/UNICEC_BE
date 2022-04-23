using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.MemberSvc;
using UniCEC.Data.ViewModels.Entities.Member;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MemberController : ControllerBase
    {
        //
        private IMemberService _ImemberService;
        public MemberController(IMemberService ImemberService)
        {
            _ImemberService = ImemberService; 
        }



        // GET api/<MemberController>/5
        [HttpGet("get-member-by-{id}")]
        public async Task<IActionResult> GetMemberById(int id)
        {
            try
            {

                ViewMember member = await _ImemberService.GetByMemberId(id);
                if (member == null)
                {
                    return NotFound();
                }
                else
                {
                    //
                    return Ok(member);
                }
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        // Add Member In Club
        [HttpPost("insert-member")]
        public async Task<IActionResult> InsertMember([FromBody] MemberInsertModel model)
        {
            try
            {
                ViewMember result = await _ImemberService.Insert(model);
                if (result != null)
                {

                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
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

        // PUT api/<MemberController>/5
        [HttpPut("update-member-by-id")]
        public async Task<IActionResult> UpdateMember ([FromBody] MemberUpdateModel member)
        {
            try
            {
                Boolean check = false;
                //
                check = await _ImemberService.Update(member);
                if (check)
                {

                    return Ok();
                }
                else
                {
                    return NotFound("Not found this City");
                }
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
        [HttpDelete("delete-member-by-{id}")]
        public async Task<IActionResult> DeleteMember (int id)
        {
            try
            {
                bool result = false;
                result = await _ImemberService.Delete(id);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }
    }
}
