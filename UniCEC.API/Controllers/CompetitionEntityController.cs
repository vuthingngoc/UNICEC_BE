using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.CompetitionEntitySvc;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniCEC.API.Controllers
{
    [Route("api/v1/competition-entities")]
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
        [HttpPost("images")]
        [SwaggerOperation(Summary = "Add images for competition")]
        public async Task<IActionResult> AddImages([FromBody] ImageInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];


                List<ViewCompetitionEntity> result = await _competitionEntityService.AddImage(model, token);

                if (result != null)
                {

                    return Ok(result);
                }
                else
                {
                    return Ok(result);
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


        [Authorize(Roles = "Student")]
        [HttpDelete]
        [SwaggerOperation(Summary = "Delete competition entity")]
        public async Task<IActionResult> DeleteCompetitionEntity([FromQuery(Name = "competitionId"), BindRequired] int competitionId, [FromQuery(Name = "clubId"), BindRequired] int clubId)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];
                bool result = await _competitionEntityService.DeleteCompetitionEntity(competitionId, clubId, token);

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

        //POST api/<CompetitionEntityController>
        [Authorize(Roles = "Student")]
        [HttpPost("sponsors")]
        [SwaggerOperation(Summary = "Add Sponsors for competition")]
        public async Task<IActionResult> AddSponsors([FromBody] SponsorInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];


                List<ViewCompetitionEntity> result = await _competitionEntityService.AddSponsor(model, token);

                if (result != null)
                {

                    return Ok(result);
                }
                else
                {
                    return Ok(result);
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


        //POST api/<CompetitionEntityController>
        [Authorize(Roles = "Student")]
        [HttpPost("influencers")]
        [SwaggerOperation(Summary = "Add Influencers for competition")]
        public async Task<IActionResult> AddInfludencers([FromBody] InfluencerInsertModel model)
        {
            try
            {
                var header = Request.Headers;
                if (!header.ContainsKey("Authorization")) return Unauthorized();
                string token = header["Authorization"].ToString().Split(" ")[1];


                List<ViewCompetitionEntity> result = await _competitionEntityService.AddInfluencer(model, token);

                if (result != null)
                {

                    return Ok(result);
                }
                else
                {
                    return Ok(result);
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
