using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.ClubActivityRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubActivity;

namespace UniCEC.Business.Services.ClubActivitySvc
{
    public class ClubActivityService : IClubActivityService
    {
        private IClubActivityRepo _clubActivityRepo;

        public ClubActivityService(IClubActivityRepo clubActivityRepo)
        {
            _clubActivityRepo = clubActivityRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewClubActivity>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewClubActivity> GetByClubActivityId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewClubActivity> Insert(ClubActivityInsertModel clubActivity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ClubActivityUpdateModel clubActivity)
        {
            throw new NotImplementedException();
        }
    }
}
