using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.CompetitionEntitySvc;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/competition-entities")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CompetitionEntityController : ControllerBase
    {

        private ICompetitionEntityService _competitionEntityService;
        public CompetitionEntityController(ICompetitionEntityService competitionEntityService)
        {
            _competitionEntityService = competitionEntityService;
        }

        //---------------------------------------------------------------------------Competition Entity
        //POST api/<CompetitionEntityController>
        [Authorize(Roles = "Student")]
        [HttpPost("image")]
        [SwaggerOperation(Summary = "Add image for competition")]
        public async Task<IActionResult> AddCompetitionEntity([FromBody] CompetitionEntityInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];


                ViewCompetitionEntity result = await _competitionEntityService.AddCompetitionEntity(model, token);

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


    }
}
