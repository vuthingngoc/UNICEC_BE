using Microsoft.AspNetCore.Mvc;
using UniCEC.Business.Services.MemberTakesActivitySvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/member-takes-activities")]
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
        //[Authorize(Roles = "Student")]
        //[HttpGet("member")]
        //[SwaggerOperation(Summary = "Get All Task Member Take In Competition Activity by conditions, 0.Doing , 1.LateTime, 2.Finished, 3.FinishedLate, 4.Approved, 5.Rejected - CompetitionManager Role")]
        //public async Task<IActionResult> GetTaskByConditionsOfMember([FromQuery] MemberTakesActivityRequestModel request)
        //{
        //    try
        //    {
        //        //JWT
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];


        //        PagingResult<ViewMemberTakesActivity> result = await _memberTakesActivityService.GetAllTasksMemberByConditions(request, token);
        //        //
        //        return Ok(result);
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (NullReferenceException)
        //    {
        //        return Ok(new List<object>());
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}



        //// GET: api/<MemberTakesActivityController>
        //[Authorize(Roles = "Student")]
        //[HttpGet("manager")]
        //[SwaggerOperation(Summary = "Get All Member Take Task In Competition Activity by conditions, 0.Doing , 1.LateTime, 2.Finished, 3.FinishedLate, 4.Approved, 5.Rejected - CompetitionManager Role" )]
        //public async Task<IActionResult> GetTaskByConditionsOfManager([FromQuery] MemberTakesActivityRequestModel request)
        //{
        //    try
        //    {
        //        //JWT
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];


        //        PagingResult<ViewMemberTakesActivity> result = await _memberTakesActivityService.GetAllTasksByConditions(request, token);
        //        //
        //        return Ok(result);
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (NullReferenceException)
        //    {
        //        return Ok(new List<object>());
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}

        //// GET api/<MemberTakesActivityController>/5
        //[Authorize(Roles = "Student")]
        //[HttpGet()]
        //[SwaggerOperation(Summary = "Get Task by Id")]
        //public async Task<IActionResult> GetTaskById([FromQuery(Name = "memberTakesActivityId")] int memberTakesActivityId, [FromQuery(Name = "clubId")] int clubId)
        //{
        //    try
        //    {
        //        //JWT
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];

        //        ViewDetailMemberTakesActivity result = await _memberTakesActivityService.GetByMemberTakesActivityId(memberTakesActivityId, clubId, token);
        //        return Ok(result);

        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (NullReferenceException)
        //    {
        //        return Ok(new object());
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}

        //// POST api/<MemberTakesActivityController>
        //[Authorize(Roles = "Student")]
        //[HttpPost]
        //[SwaggerOperation(Summary = "Insert member in task - Student")]
        //public async Task<IActionResult> InsertMemberTakesActivity([FromBody] MemberTakesActivityInsertModel model)
        //{
        //    try
        //    {
        //        //JWT
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];

        //        ViewDetailMemberTakesActivity result = await _memberTakesActivityService.Insert(model, token);
        //        if (result != null)
        //        {

        //            return Ok(result);
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}

        //// PUT api/<MemberTakesActivityController>/5
        //[Authorize(Roles = "Student")]
        //[HttpDelete("remove-task-of-member")]
        //[SwaggerOperation(Summary = "Remove member out of task - Student")]
        //public async Task<IActionResult> MemberSubmitTask([FromBody] RemoveMemberTakeActivityModel model)
        //{
        //    try
        //    {
        //        //JWT
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];

        //        Boolean check = false;
        //        check = await _memberTakesActivityService.RemoveMemberTakeActivity(model, token);
        //        if (check)
        //        {
        //            return Ok();
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
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
        //        return StatusCode(500, "Internal server exception");
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}


        //// PUT api/<MemberTakesActivityController>/5
        //[Authorize(Roles = "Student")]
        //[HttpPut("submit-activity")]
        //[SwaggerOperation(Summary = "Member submit task by Id - Student")]
        //public async Task<IActionResult> MemberSubmitTask([FromBody] SubmitMemberTakesActivity model)
        //{
        //    try
        //    {
        //        //JWT
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];

        //        Boolean check = false;
        //        check = await _memberTakesActivityService.SubmitTask(model, token);
        //        if (check)
        //        {
        //            return Ok();
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
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
        //        return StatusCode(500, "Internal server exception");
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}


        //// PUT api/<MemberTakesActivityController>/5
        //[Authorize(Roles = "Student")]
        //[HttpPut("confirm-activity")]
        //[SwaggerOperation(Summary = "Competition Manager is Approved task of member - Student")]
        //public async Task<IActionResult> ApprovedOrRejectedTask([FromBody] ConfirmMemberTakesActivity model)
        //{
        //    try
        //    {
        //        //JWT
        //        var header = Request.Headers;
        //        if (!header.ContainsKey("Authorization")) return Unauthorized();
        //        string token = header["Authorization"].ToString().Split(" ")[1];

        //        Boolean check = false;
        //        check = await _memberTakesActivityService.ApprovedOrRejectedTask(model, token);
        //        if (check)
        //        {
        //            return Ok();
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal server exception");
        //    }
        //}

    }
}
