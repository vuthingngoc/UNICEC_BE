using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.SponsorSvc;
using UniCEC.Data.ViewModels.Entities.Sponsor;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/sponsors")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SponsorController : ControllerBase
    {

        private ISponsorService _sponsorService;

        public SponsorController(ISponsorService sponsorService)
        {
            _sponsorService = sponsorService;
        }

        // GET api/<SponsorController>/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get sponsor by id - Admin")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                ViewSponsor result = await _sponsorService.GetBySponsorId(id);
                return Ok(result);

            }
            catch (ArgumentException ex)
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



        // PUT api/<SponsorController>/5
        [HttpPut]
        [SwaggerOperation(Summary = "Update infomations sponsor - Admin")]
        public async Task<IActionResult> Update([FromBody] SponsorUpdateModel sponsorUpdateModel)
        {
            try
            {
                bool result = await _sponsorService.Update(sponsorUpdateModel);
                if (result)
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
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal server exception");
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal server exception");
            }
        }

        // DELETE api/<SponsorController>/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Ban sponsor by Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Boolean check = false;
                check = await _sponsorService.Delete(id);
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
