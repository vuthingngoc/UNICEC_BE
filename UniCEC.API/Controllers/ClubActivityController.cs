using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.ClubActivitySvc;
using UniCEC.Data.Enum;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubActivity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/club-activity")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ClubActivityController : ControllerBase
    {
        private IClubActivityService _clubActivityService;

        public ClubActivityController(IClubActivityService clubActivityService)
        {
            _clubActivityService = clubActivityService;
        }


        [HttpGet("process-club-activity")]
        [SwaggerOperation(Summary = "Get process of club activity by Id")]
        public async Task<IActionResult> GetListClubActivitiesByConditions([FromQuery] int ClubActivityId , [FromQuery] MemberTakesActivityStatus status)
        {
            try
            {
                ViewProcessClubActivity result = await _clubActivityService.GetProcessClubActivity(ClubActivityId,status);

                if (result != null)
                {

                    return Ok(result);
                }
                else
                {
                    return NotFound();
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


        [HttpGet("top4")]
        [SwaggerOperation(Summary = "Get top 4 club activities by create date")]
        //Lưu ý University Id dựa vào JWT
        public async Task<IActionResult> GetListClubActivitiesByConditions([FromQuery] int UniversityId, [FromQuery] int ClubId, [FromQuery] DateTime CreateDate)
        {
            try
            {
                List<ViewClubActivity> result = await _clubActivityService.GetClubActivitiesByCreateTime(UniversityId, ClubId, CreateDate);

                if (result != null)
                {

                    return Ok(result);
                }
                else
                {
                    return NotFound();
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


        [HttpGet]
        [SwaggerOperation(Summary = "Get ClubActivities by conditions")]
        public async Task<IActionResult> GetListClubActivitiesByConditions([FromQuery] ClubActivityRequestModel conditions)
        {
            try
            {
                PagingResult<ViewClubActivity> result = await _clubActivityService.GetListClubActivitiesByConditions(conditions);

                if (result != null)
                {

                    return Ok(result);
                }
                else
                {
                    return NotFound();
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

        // GET api/<ClubActivityController>/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get ClubActivity by id")]
        public async Task<IActionResult> GetClubActivityById(int id)
        {
            try
            {
                ViewClubActivity result = await _clubActivityService.GetByClubActivityId(id);
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

        // POST api/<ClubActivityController>
        [HttpPost]
        [SwaggerOperation(Summary = "Insert ClubActivity")]
        public async Task<IActionResult> InsertClubActivity([FromBody] ClubActivityInsertModel model)
        {
            try
            {

                ViewClubActivity result = await _clubActivityService.Insert(model);
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

        // PUT api/<ClubActivityController>/5
        [HttpPut]
        [SwaggerOperation(Summary = "Update ClubActivity by Id")]
        public async Task<IActionResult> UpdateClubActivity([FromBody] ClubActivityUpdateModel model)
        {
            try
            {
                Boolean check = false;
                check = await _clubActivityService.Update(model);
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

        // DELETE api/<ClubActivityController>/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete ClubActivity by id")]
        public async Task<IActionResult> DeleteClubAcitvityById(int id)
        {
            try
            {
                bool result = false;
                result = await _clubActivityService.Delete(id);
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
