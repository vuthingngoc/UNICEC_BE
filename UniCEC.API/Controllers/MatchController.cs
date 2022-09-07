using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Entities.Match;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/matches")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MatchController : ControllerBase
    {
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get match by id - All user")]
        public Task<IActionResult> GetMatchTypeById(int id)
        {

            throw new System.NotImplementedException();
        }

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Search match by conditions - All user")]
        public Task<IActionResult> GetMatchByConditions(MatchRequestModel request)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Insert match - Competition manager")]
        public Task<IActionResult> InsertMatch(MatchInsertModel model)
        {
            throw new System.NotImplementedException();
        }

        [HttpPut]
        [Authorize]
        [SwaggerOperation(Summary = "Update match - Competition manager")]
        public Task<IActionResult> UpdateMatch(MatchUpdateModel model)
        {
            throw new System.NotImplementedException();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Delete match - Competition manager")]
        public Task<IActionResult> DeleteMatch(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
