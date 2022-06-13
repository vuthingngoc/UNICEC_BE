using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.CompetitionManagerSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/competition-managers")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CompetitionManagerController : ControllerBase
    {
        ICompetitionManagerService _competitionManagerService;

        public CompetitionManagerController(ICompetitionManagerService competitionManagerService)
        {
            _competitionManagerService = competitionManagerService;
        }


        // GET api/<CompetitionManagerController>/5
        [Authorize(Roles = "Student")]
        [HttpGet("manager")]
        [SwaggerOperation(Summary = "Get all manager in competition")]
        public async Task<IActionResult> GetAllManagerInCompetition([FromQuery] CompetitionManagerRequestModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                PagingResult<ViewCompetitionManager> result = await _competitionManagerService.GetAllManagerCompOrEve(model, token);

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



        //---------------------------------------------------------------------------Competition Manager
        //POST api/<CompetitionManagerController>
        [Authorize(Roles = "Student")]
        [HttpPost("member")]
        [SwaggerOperation(Summary = "Add member to manage competition")]
        public async Task<IActionResult> AddMemberInCompetitionManager(CompetitionManagerInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewCompetitionManager result = await _competitionManagerService.AddMemberInCompetitionManager(model, token);
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
        [HttpPut("member-role")]
        [SwaggerOperation(Summary = "Update member role in Competition Manager")]
        public async Task<IActionResult> UpdateMemberInCompetitionManagerRole([FromBody] CompetitionManagerUpdateModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                Boolean check = false;
                check = await _competitionManagerService.UpdateMemberInCompetitionManager(model, token);
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
