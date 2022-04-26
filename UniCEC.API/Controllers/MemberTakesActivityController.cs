using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.MemberTakesActivitySvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.MemberTakesActivity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MemberTakesActivityController : ControllerBase
    {
        private IMemberTakesActivityService _memberTakesActivityService;

        public MemberTakesActivityController(IMemberTakesActivityService memberTakesActivityService)
        {
            _memberTakesActivityService = memberTakesActivityService;
        }

        // GET: api/<MemberTakesActivityController>
        [HttpGet("get-task-by-conditions")]
        public async Task<IActionResult> GetTaskByConditions([FromQuery]MemberTakesActivityRequestModel request)
        {
            try
            {
                PagingResult<ViewMemberTakesActivity>  result = await _memberTakesActivityService.GetAllTaskesByConditions(request);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    //
                    return Ok(result);
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

        // GET api/<MemberTakesActivityController>/5
        [HttpGet("get-task-by-{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                ViewMemberTakesActivity result = await _memberTakesActivityService.GetByMemberTakesActivityId(id);
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    //
                    return Ok(result);
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

        // POST api/<MemberTakesActivityController>
        [HttpPost("insert-member-takes-activity")]
        public async Task<IActionResult> InsertMemberTakesActivity([FromBody] MemberTakesActivityInsertModel model)
        {
            try
            {
                ViewMemberTakesActivity result = await _memberTakesActivityService.Insert(model);
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


        // PUT api/<MemberTakesActivityController>/5
        [HttpPut("member-submit-task-by-{id}")]
        public async Task<IActionResult> MemberSubmitTask(int id)
        {
            try
            {
                Boolean check = false;
                check = await _memberTakesActivityService.Update(id);
                if (check)
                {
                    return Ok();
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

    }
}
