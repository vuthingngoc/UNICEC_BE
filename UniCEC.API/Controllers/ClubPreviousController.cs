using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using UniCEC.Business.Services.ClubPreviousSvc;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubPrevious;

namespace UniCEC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubPreviousController : ControllerBase
    {
        private IClubPreviousService _clubPreviousService;
        public ClubPreviousController(IClubPreviousService clubPreviousService)
        {
            _clubPreviousService = clubPreviousService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClubPrevious(PagingRequest request)
        {
            try
            {
                PagingResult<ViewClubPrevious> previousClubs = await _clubPreviousService.GetAllPaging(request);
                return Ok(previousClubs);
            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClubPreviousById(int id)
        {
            try
            {
                ViewClubPrevious clubPrevious = await _clubPreviousService.GetByClubPrevious(id);
                return Ok(clubPrevious);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch(NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetClubPreviousByConditions(ClubPreviousRequestModel request)
        {
            try
            {
                PagingResult<ViewClubPrevious> previousClubs = await _clubPreviousService.GetByContitions(request);
                return Ok(previousClubs);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public Task<IActionResult> InsertClubPrevious(ClubPreviousInsertModel clubPrevious)
        {

            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<IActionResult> UpdateClubPrevious(ClubPreviousUpdateModel clubPrevious)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteClubPrevious(int id)
        {
            throw new NotImplementedException();
        }

    }
}
