using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Entities.MatchType;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/match-types")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MatchTypeController : ControllerBase
    {
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get match type by id - All user")]
        public Task<IActionResult> GetMatchTypeById(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Search match type by name - All user")]
        public Task<IActionResult> GetMatchTypeByName(string name)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all match type - All user")]
        public Task<IActionResult> GetAllMatchTypes(string name)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        [Authorize(Roles = "System Admin")]
        [SwaggerOperation(Summary = "Insert match type - System admin")]
        public Task<IActionResult> InsertMatchType(MatchTypeInsertModel model)
        {
            throw new System.NotImplementedException();
        }

        [HttpPut]
        [Authorize(Roles = "System Admin")]
        [SwaggerOperation(Summary = "Update match type - System admin")]
        public Task<IActionResult> UpdateMatchType(MatchTypeUpdateModel model)
        {
            throw new System.NotImplementedException();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "System Admin")]
        [SwaggerOperation(Summary = "Delete match type - System admin")]
        public Task<IActionResult> DeleteMatchType(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
