using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.CompetitionInClubSvc;
using UniCEC.Data.ViewModels.Entities.CompetitionInClub;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/competition-in-club")]
    [ApiController]
    public class CompetitionInClubController : ControllerBase
    {
        private ICompetitionInClubService _competitionInClubService;

        public CompetitionInClubController(ICompetitionInClubService competitionInClubService)
        {
            _competitionInClubService = competitionInClubService;
        }

        // GET api/<CompetitionInClubController>/5
        //[HttpGet("{id}")]
        //[SwaggerOperation(Summary = "club leader creates competition")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //POST api/<CompetitionInClubController>
        [Authorize(Roles = "Student")]
        [HttpPost("add-club-collaborate")]
        [SwaggerOperation(Summary = "add another club in competition")]
        public async Task<IActionResult> Insert([FromBody] CompetitionInClubInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];

                ViewCompetitionInClub result = await _competitionInClubService.Insert(model, token);
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

        ////PUT api/<CompetitionInClubController>/5
        //[Authorize(Roles = "Student")]
        //[HttpPut]
        //[SwaggerOperation(Summary = "Update competition in club ")]
        //public async Task<IActionResult> Update([FromBody] ViewCompetitionInClub model)
        //{
        //    try
        //    {
        //        Boolean check = false;
        //        check = await _competitionInClubService.Update(model);
        //        if (check)
        //        {
        //            return Ok();
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

        //// DELETE api/<CompetitionInClubController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
