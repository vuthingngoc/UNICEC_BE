using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Entities.TeamInMatch;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/teams-in-match")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TeamInMatchController : ControllerBase
    {
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get match type by id - all user")]
        public Task<IActionResult> GetMatchTypeById(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Search match type by name - all user")]
        public Task<IActionResult> GetMatchTypeByConditions(TeamInMatchRequestModel request)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all match type - all user")]
        public Task<IActionResult> GetAllMatchTypes(string name)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Insert match type - Competition manager")]
        public Task<IActionResult> InsertMatchType(TeamInMatchInsertModel model)
        {
            throw new System.NotImplementedException();
        }

        [HttpPut]
        [Authorize]
        [SwaggerOperation(Summary = "Update match type - Competition manager")]
        public Task<IActionResult> UpdateMatchType(TeamInMatchUpdateModel model)
        {
            throw new System.NotImplementedException();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Update match - Competition manager")]
        public Task<IActionResult> DeleteMatchType(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
