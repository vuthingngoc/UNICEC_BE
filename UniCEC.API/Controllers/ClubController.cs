using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Business.Services.ClubSvc;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Club;

namespace UniCEC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ClubController : ControllerBase
    {
        private IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService; 
        }

        //[HttpGet("{id}")]
        //public async Task<ViewClub> GetClubById(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpGet]
        //public async Task<PagingResult<ViewClub>> GetAllClub()
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpGet]
        //public async Task<List<ViewClub>> GetClubByName(string name)
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpGet]
        //public async Task<List<ViewClub>> GetClubByCompetition(int competitionId)
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpPost]
        //public async Task<ViewClub> InsertClub(ClubInsertModel club)
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpPut]
        //public async Task<ViewClub> UpdateClub(ClubUpdateModel club)
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpDelete("{id}")]
        //public async Task<ViewClub> DeleteClub(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
