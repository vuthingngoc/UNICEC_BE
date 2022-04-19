using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.ClubRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Club;

namespace UniCEC.Business.Services.ClubSvc
{
    public class ClubService : IClubService
    {
        private IClubRepo _clubRepo;

        public ClubService(IClubRepo clubRepo)
        {
            _clubRepo = clubRepo;
        }

        public Task<PagingResult<ViewClub>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewClub> GetByClub(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ViewClub>> GetByCompetition(int competitionId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ViewClub>> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ViewClub> Insert(ClubInsertModel club)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ClubUpdateModel club)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
