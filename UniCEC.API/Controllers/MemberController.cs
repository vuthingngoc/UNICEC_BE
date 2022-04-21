using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // Add Member In Club
        [HttpPost]
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
