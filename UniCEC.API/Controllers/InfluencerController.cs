using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.InfluencerSvc;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Influencer;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/influencers")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]

    public class InfluencerController : ControllerBase
    {
        private readonly IInfluencerService _influencerService;
        
        public InfluencerController(IInfluencerService influencerService)
        {
            _influencerService = influencerService;
        }

        [HttpGet("competition/{id}")]
        [SwaggerOperation(Summary = "Get all influencers in the competition")]
        public async Task<IActionResult> GetAllInfluencerByCompetition(int id, PagingRequest request)
        {
            try
            {
                PagingResult<ViewInfluencer> influencers = await _influencerService.GetByCompetition(id, request);
                return Ok(influencers);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        //[HttpPost]
        //[SwaggerOperation(Summary = "Insert a new influencer to the competition")]
        //public async Task<IActionResult> InsertInfluencer([FromBody] InfluencerInsertModel model)
        //{
        //    try
        //    {
        //        string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
        //        ViewInfluencer influencer = await _influencerService.Insert(model, token);
        //        return Ok(influencer);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (NullReferenceException)
        //    {
        //        return Ok(new List<object>());
        //    }
        //    catch (SqlException)
        //    {
        //        return StatusCode(500, "Internal Server Exception");
        //    }
        //}

        [HttpPut("{id}/upload-image")]
        [SwaggerOperation(Summary = "Update image for influencer in the competition")]
        public async Task<IActionResult> UpdateImageForInfluencer(int id)
        {
            try
            {
                var imageFile = Request.Form.Files[0];
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _influencerService.Update(id, imageFile, token);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return Ok(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update info influencer in the competition")]
        public async Task<IActionResult> UpdateInfluencer(InfluencerUpdateModel model)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _influencerService.Update(model, token);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return Ok(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an influencer to the competition")]
        public async Task<IActionResult> DeleteInfluencer(int id)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                await _influencerService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException)
            {
                return Ok(new List<object>());
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

    }
}
