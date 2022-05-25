using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.InfluencerSvc;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/influencer")]
    [ApiController]
    [ApiVersion("1.0")]
    public class InfluencerController : ControllerBase
    {
        private readonly IInfluencerService _influencerService;
        
        public InfluencerController(IInfluencerService influencerService)
        {
            _influencerService = influencerService;
        }

        [HttpGet("competition/{id}")]
        [SwaggerOperation(Summary = "Get all influencers in the competition")]
        public Task<IActionResult> GetAllInfluencerByCompetition(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Insert a new influencer to the competition")]
        public Task<IActionResult> InsertInfluencer(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update an influencer in the competition")]
        public Task<IActionResult> UpdateInfluencer(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an influencer to the competition")]
        public Task<IActionResult> DeleteInfluencer(int id)
        {
            throw new NotImplementedException();
        }

    }
}
