using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.ClubPreviousRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.ClubPrevious;

namespace UNICS.Business.Services.ClubPreviousSvc
{
    public class ClubPreviousService : IClubPreviousService
    {
        private IClubPreviousRepo _clubPreviousRepo;

        public ClubPreviousService(IClubPreviousRepo clubPreviousRepo)
        {
            _clubPreviousRepo = clubPreviousRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewClubPrevious>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewClubPrevious> GetByClubPreviousId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewClubPrevious> Insert(ClubPreviousInsertModel clubPrevious)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ClubPreviousUpdateModel clubPrevious)
        {
            throw new NotImplementedException();
        }
    }
}
