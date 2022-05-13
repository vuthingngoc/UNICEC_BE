using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using UniCEC.Business.Services.SponsorInCompetitionSvc;
using UniCEC.Data.ViewModels.Entities.SponsorInCompetition;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/sponsor-in-competition")]
    [ApiController]
    public class SponsorInCompetitionController : ControllerBase
    {

        private ISponsorInCompetitionService _sponsorInCompetitionService;

        public SponsorInCompetitionController(ISponsorInCompetitionService sponsorInCompetitionService)
        {
            _sponsorInCompetitionService = sponsorInCompetitionService;
        }



        //// GET api/<SponsorInCompetitionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SponsorInCompetitionController>
        //[Authorize(Roles = "Sponsor")]
        //[HttpPost]
        //[SwaggerOperation(Summary = "sponsor creates competition")]
        //public async Task<IActionResult> Insert([FromBody] SponsorInCompetitionInsertModel model)
        //{
        //    try
        //    {
        //        ViewSponsorInCompetition result = await _sponsorInCompetitionService.Insert(model);
        //        if (result != null)
        //        {

        //            return Ok(result);
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
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

        //// PUT api/<SponsorInCompetitionController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<SponsorInCompetitionController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
