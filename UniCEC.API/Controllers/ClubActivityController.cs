using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.ClubActivitySvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubActivity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ClubActivityController : ControllerBase
    {
        private IClubActivityService _clubActivityService;

        public ClubActivityController(IClubActivityService clubActivityService)
        {
            _clubActivityService = clubActivityService;
        }


        [HttpGet("find-list-club-activities-by-conditions")]
        public async Task<IActionResult> GetListClubActivitiesByConditions([FromQuery] ClubActivityRequestModel conditions )
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
        [HttpGet("find-club-activity-by-{id}")]
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
        [HttpPost("insert-club-activity")]
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
        [HttpPut("update-club-activity-by-id")]
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
        [HttpDelete("delete-club-activity-by-{id}")]
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
