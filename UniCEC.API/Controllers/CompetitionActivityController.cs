using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.CompetitionActivitySvc;
using UniCEC.Data.Enum;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.CompetitionActivity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/club-activity")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CompetitionActivityController : ControllerBase
    {
        private ICompetitionActivityService _clubActivityService;

        public CompetitionActivityController(ICompetitionActivityService clubActivityService)
        {
            _clubActivityService = clubActivityService;
        }


        [Authorize(Roles = "Student")]
        [HttpGet("top4-process")]
        [SwaggerOperation(Summary = "Get top 4 club activities by now and process")]
        //Lưu ý University Id dựa vào JWT
        public async Task<IActionResult> GetTop4_Process([FromQuery(Name = "clubId")] int ClubId)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                List<ViewProcessCompetitionActivity> result = await _clubActivityService.GetTop4_Process(ClubId, token);
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



        [HttpGet]
        [SwaggerOperation(Summary = "Get ClubActivities by conditions")]
        public async Task<IActionResult> GetListClubActivitiesByConditions([FromQuery] CompetitionActivityRequestModel conditions)
        {
            try
            {
                PagingResult<ViewCompetitionActivity> result = await _clubActivityService.GetListClubActivitiesByConditions(conditions);

                    return Ok(result);
                                        
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

        // GET api/<ClubActivityController>/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get ClubActivity by id")]
        public async Task<IActionResult> GetClubActivityById(int id)
        {
            try
            {
                ViewCompetitionActivity result = await _clubActivityService.GetByClubActivityId(id);                  
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

        // POST api/<ClubActivityController>
        [Authorize(Roles = "Student")]
        [HttpPost]
        [SwaggerOperation(Summary = "Insert ClubActivity - Student(Leader of club)")]
        public async Task<IActionResult> InsertClubActivity([FromBody] CompetitionActivityInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewCompetitionActivity result = await _clubActivityService.Insert(model, token);
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
        [SwaggerOperation(Summary = "Update ClubActivity by Id - Student(Leader of club)")]
        public async Task<IActionResult> UpdateClubActivity([FromBody] CompetitionActivityUpdateModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                Boolean check = false;
                check = await _clubActivityService.Update(model, token);
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
        [SwaggerOperation(Summary = "Delete ClubActivity by id - Student(Leader of club) ")]
        public async Task<IActionResult> DeleteClubAcitvityById([FromBody] CompetitionActivityDeleteModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                bool result = false;
                result = await _clubActivityService.Delete(model, token);
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
