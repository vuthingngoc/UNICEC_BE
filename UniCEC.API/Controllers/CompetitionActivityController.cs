using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.CompetitionActivitySvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionActivity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/competition-activities")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CompetitionActivityController : ControllerBase
    {
        private ICompetitionActivityService _competitionActivityService;

        public CompetitionActivityController(ICompetitionActivityService competitionActivityService)
        {
            _competitionActivityService = competitionActivityService;
        }


        [Authorize(Roles = "Student")]
        [HttpGet("top3-process")]
        [SwaggerOperation(Summary = "Get top 3 competition activities of Competition by club with now and process")]      
        public async Task<IActionResult> GetTop4_Process([FromQuery(Name = "clubId")] int ClubId)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                List<ViewProcessCompetitionActivity> result = await _competitionActivityService.GetTop3TasksOfCompetition(ClubId, token);
                return Ok(result);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
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

        [Authorize(Roles = "Student")]
        [HttpGet]
        [SwaggerOperation(Summary = "Get Competition Activity by Conditions")]
        public async Task<IActionResult> GetListClubActivitiesByConditions([FromQuery] CompetitionActivityRequestModel conditions)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                PagingResult<ViewCompetitionActivity> result = await _competitionActivityService.GetListActivitiesByConditions(conditions, token);

                return Ok(result);

            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
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

        // GET api/<ClubActivityController>/5
        [Authorize(Roles = "Student")]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Competition Activity by Id")]
        public async Task<IActionResult> GetCompetitionActivityById(int id, [FromQuery(Name = "clubId")] int clubId)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewDetailCompetitionActivity result = await _competitionActivityService.GetCompetitionActivityById(id, clubId, token);
                return Ok(result);

            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
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

        // POST api/<ClubActivityController>
        [Authorize(Roles = "Student")]
        [HttpPost]
        [SwaggerOperation(Summary = "Insert competition activity")]
        public async Task<IActionResult> InsertClubActivity([FromBody] CompetitionActivityInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewDetailCompetitionActivity result = await _competitionActivityService.Insert(model, token);
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

        // PUT api/<ClubActivityController>/5
        [Authorize(Roles = "Student")]
        [HttpPut]
        [SwaggerOperation(Summary = "Update Competition Activity by Id ")]
        public async Task<IActionResult> UpdateClubActivity([FromBody] CompetitionActivityUpdateModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                Boolean check = false;
                check = await _competitionActivityService.Update(model, token);
                if (check)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
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

        // DELETE api/<ClubActivityController>/5
        [Authorize(Roles = "Student")]
        [HttpDelete]
        [SwaggerOperation(Summary = "Delete Competition Activity by Id  ")]
        public async Task<IActionResult> DeleteClubAcitvityById([FromBody] CompetitionActivityDeleteModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                bool result = false;
                result = await _competitionActivityService.Delete(model, token);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
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
