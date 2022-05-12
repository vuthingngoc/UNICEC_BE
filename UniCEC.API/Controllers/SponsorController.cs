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
    [Route("api/[controller]")]
    [ApiController]
    public class SponsorController : ControllerBase
    {

        private ISponsorService _sponsorService;

        public SponsorController(ISponsorService sponsorService)
        {
            _sponsorService = sponsorService;
        }

        // GET api/<SponsorController>/5
        //[Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get sponsor by id")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                ViewSponsor result = await _sponsorService.GetBySponsorId(id);
                if (result == null)
                {
                    //Not has data
                    return Ok(new object());
                }
                else
                {
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



        // PUT api/<SponsorController>/5
        //[Authorize(Roles = "Sponsor")]
        [HttpPut]
        [SwaggerOperation(Summary = "Update infomations sponsor")]
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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE api/<SponsorController>/5
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Ban sponsor by Admin")]
        public async Task<IActionResult> Delete([FromQuery] int id)
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
