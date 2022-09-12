using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.CompetitionSvc;
using UniCEC.Data.Enum;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;
using UniCEC.Data.ViewModels.Entities.CompetitionInMajor;
using UniCEC.Data.ViewModels.Entities.MemberInCompetition;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/competitions")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CompetitionController : ControllerBase
    {
        private ICompetitionService _competitionService;

        public CompetitionController(ICompetitionService competitionService)
        {
            _competitionService = competitionService;
        }


        //UNAUTHORIZE
        [HttpGet("guest")]
        [SwaggerOperation(Summary = "Get EVENT or COMPETITION by Condition - UnAuthorize")]
        public async Task<IActionResult> GetCompOrEveUnAuthorize([FromQuery] CompetitionUnAuthorizeRequestModel model)
        {
            try
            {
                PagingResult<ViewCompetition> result = await _competitionService.GetCompOrEveUnAuthorize(model);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }


        // GET: api/<CompetitionController>
        [Authorize(Roles = "Student")]
        [HttpGet]
        [SwaggerOperation(Summary = "Get EVENT or COMPETITION by Condition")]
        public async Task<IActionResult> GetCompOrEve([FromQuery] CompetitionRequestModel request)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                PagingResult<ViewCompetition> result = await _competitionService.GetCompOrEve(request, token);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }


        // GET: api/<CompetitionController>
        [Authorize(Roles = "Student")]
        [HttpGet("student-join-competitions")]
        [SwaggerOperation(Summary = "Get EVENTS or COMPETITIONS that Student Join")]
        public async Task<IActionResult> GetCompsOrEvesStudentJoin([FromQuery] GetStudentJoinCompOrEve request)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                PagingResult<ViewCompetition> result = await _competitionService.GetCompsOrEvesStudentJoin(request, token);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }


        // GET: api/<CompetitionController>
        [Authorize(Roles = "Student")]
        [HttpGet("student-join-competition")]
        [SwaggerOperation(Summary = "")]
        public async Task<IActionResult> GetCompsOrEvesStudentIsAssignedTask([FromQuery(Name = "competitionId"), BindRequired] int competitionId)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewCompetition result = await _competitionService.GetCompOrEveStudentJoin(competitionId, token);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        // GET: api/<CompetitionController>
        [Authorize(Roles = "Student")]
        [HttpGet("student-is-assigned")]
        [SwaggerOperation(Summary = "Get EVENTS or COMPETITIONS that Student is assigned in Task (Status != Canceling)")]
        public async Task<IActionResult> GetCompOrEveStudentIsAssignedTask([FromQuery] PagingRequest request, [FromQuery(Name = "clubId"), BindRequired] int clubId, [FromQuery(Name = "name")] string searchName, [FromQuery(Name = "event")] bool? isEvent)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                PagingResult<ViewCompetition> result = await _competitionService.GetCompOrEveStudentIsAssignedTask(request, clubId, searchName, isEvent, token);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }


        // GET: api/<CompetitionController>
        [Authorize(Roles = "Student")]
        [HttpGet("top")]
        [SwaggerOperation(Summary = "Get top X EVENT or COMPETITION by club, status")]
        public async Task<IActionResult> GetTopCompOrEve([FromQuery(Name = "clubId"), BindRequired] int ClubId, [FromQuery(Name = "event")] bool? Event/*, [FromQuery(Name = "status")] CompetitionStatus? Status*/, [FromQuery(Name = "scope")] CompetitionScopeStatus? Scope, [FromQuery, BindRequired] int top)
        {
            try
            {
                List<ViewTopCompetition> result = await _competitionService.GetTopCompOrEve(ClubId, Event/*, Status*/, Scope, top);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }


        // GET api/<CompetitionController>/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get detail of EVENT or COMPETITON by id")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                ViewDetailCompetition result = await _competitionService.GetById(id);

                return Ok(result);

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



        // GET api/<CompetitionController>/5
        [Authorize(Roles = "Student")]
        [HttpGet("process-by-club")]
        [SwaggerOperation(Summary = "Get Process of EVENT or COMPETITON by club id")]
        public async Task<IActionResult> GetProcessByClub([FromQuery(Name = "clubId"), BindRequired] int ClubId)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewProcessCompetitionOrEventOfClub result = await _competitionService.GetNumberOfCompetitionOrEventInClubWithStatus(ClubId, token);

                return Ok(result);

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



        //ClubLeader
        // POST api/<CompetitionController>
        [Authorize(Roles = "Student")]
        [HttpPost()]
        [SwaggerOperation(Summary = "Insert EVENT or COMPETITON, if Event please put value at number-of-group = 0 ")]
        //phải có author student
        public async Task<IActionResult> Insert([FromBody] LeaderInsertCompOrEventModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];


                ViewDetailCompetition viewCompetition = await _competitionService.InsertCompetitionOrEvent(model, token);
                if (viewCompetition != null)
                {

                    return Ok(viewCompetition);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        // PUT api/<CompetitionController>/5
        [Authorize(Roles = "Student")]
        [HttpPut()]
        [SwaggerOperation(Summary = "Update detail EVENT or COMPETITON By State")]
        public async Task<IActionResult> Update([FromBody] LeaderUpdateCompOrEventModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _competitionService.UpdateCompetitionByState(model, token);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        // PUT api/<CompetitionController>/5
        [Authorize(Roles = "University Admin")]
        [HttpPut("university-admin")]
        [SwaggerOperation(Summary = "Admin University Approve or Reject Competition")]
        public async Task<IActionResult> ChangeStateByAdminUni([FromBody] AdminUniUpdateCompetitionStatusModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                bool check = await _competitionService.ChangeStateByAdminUni(model, token);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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

        [Authorize(Roles = "University Admin")]
        [HttpGet("university-admin")]
        [SwaggerOperation(Summary = "Get EVENT or COMPETITION State Pending Review by Admin University")]
        public async Task<IActionResult> GetCompOrEveByStatePendingReview([FromQuery] AdminUniGetCompetitionRequestModel request)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                PagingResult<ViewCompetition> result = await _competitionService.GetCompetitionByAdminUni(request, token);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }


        // PUT api/<CompetitionController>/5
        [Authorize(Roles = "Student")]
        [HttpPut("status")]
        [SwaggerOperation(Summary = "Change Competition Status")]
        public async Task<IActionResult> CompetitionUpdateStatus([FromBody] CompetitionStatusUpdateModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                bool check = await _competitionService.CompetitionStatusUpdate(model, token);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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

        // PUT api/<CompetitionController>/5
        [Authorize(Roles = "Student")]
        [HttpPut("pending")]
        [SwaggerOperation(Summary = "Change Competition Status from Pending to Another Status")]
        public async Task<IActionResult> CompetitionUpdateStatusAfterPending(UpdateCompetitionWithStatePendingModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                bool check = await _competitionService.CompetitionUpdateStatusAfterPending(model, token);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        // PUT api/<CompetitionController>/5
        [Authorize(Roles = "Student")]
        [HttpPut("test-be")]
        [SwaggerOperation(Summary = "Update detail EVENT or COMPETITON")]
        public async Task<IActionResult> Update_BE([FromBody] LeaderUpdateCompOrEventModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _competitionService.UpdateBE(model, token);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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



        //---------------------------------------------------------------------------Competition In Major
        //POST api/<CompetitionController>

        [Authorize(Roles = "Student")]
        [HttpPost("major")]
        [SwaggerOperation(Summary = "Add major for competition")]
        public async Task<IActionResult> AddCompetitionInDepartment([FromBody] CompetitionInMajorInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                List<ViewCompetitionInMajor> result = await _competitionService.AddCompetitionInMajor(model, token);

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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

        [Authorize(Roles = "Student")]
        [HttpDelete("major")]
        [SwaggerOperation(Summary = "Delete major for competition")]
        public async Task<IActionResult> DeleteMajorInCompetition([FromBody] CompetitionInMajorDeleteModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                bool result = await _competitionService.DeleteMajorInCompetition(model, token);


                return Ok(result);

            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        //---------------------------------------------------------------------------Competition In Club
        //POST api/<CompetitionController>
        [Authorize(Roles = "Student")]
        [HttpPost("club")]
        [SwaggerOperation(Summary = "Add another club in competition")]
        public async Task<IActionResult> AddClubCollaborate([FromBody] CompetitionInClubInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewCompetitionInClub result = await _competitionService.AddClubCollaborate(model, token);
                if (result != null)
                {

                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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

        [Authorize(Roles = "Student")]
        [HttpDelete("club")]
        [SwaggerOperation(Summary = "Delete another club collaborate in competition")]
        public async Task<IActionResult> DeleteClubCollaborate([FromQuery(Name = "competitionInClubId"), BindRequired] int CompetitionInClubId, [FromQuery(Name = "clubId"), BindRequired] int clubId)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                bool result = await _competitionService.DeleteClubCollaborate(CompetitionInClubId, clubId, token);

                return Ok(result);

            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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


        //---------------------------------------------------------------------------Member In Competition
        // GET api/<CompetitionManagerController>/5
        [Authorize(Roles = "Student")]
        [HttpGet("manager")]
        [SwaggerOperation(Summary = "Get all manager in competition")]
        public async Task<IActionResult> GetAllManagerInCompetition([FromQuery] MemberInCompetitionRequestModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                PagingResult<ViewMemberInCompetition> result = await _competitionService.GetAllManagerCompOrEve(model, token);


                return Ok(result);

            }
            catch (NullReferenceException)
            {
                return Ok(new object());
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }


        //POST api/<CompetitionManagerController>
        [Authorize(Roles = "Student")]
        [HttpPost("member")]
        [SwaggerOperation(Summary = "Add member to manage competition")]
        public async Task<IActionResult> AddMemberInCompetition([FromBody] MemberInCompetitionInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewMemberInCompetition result = await _competitionService.AddMemberInCompetition(model, token);

                if (result != null)
                {

                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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



        // PUT api/<CompetitionManagerController>/5
        [Authorize(Roles = "Student")]
        [HttpPut("member")]
        [SwaggerOperation(Summary = "Update member role in Member In Competition")]
        public async Task<IActionResult> UpdateMembeInCompetition([FromBody] MemberInCompetitionUpdateModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _competitionService.UpdateMemberInCompetition(model, token);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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
