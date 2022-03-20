using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.ClubRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Club;

namespace UNICS.Business.Services.ClubSvc
{
    public class ClubService : IClubService
    {
        private IClubRepo _clubRepo;

        public ClubService(IClubRepo clubRepo)
        {
            _clubRepo = clubRepo;
        }


        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewClub>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewClub> GetByClubId(int id)
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
    }
}
