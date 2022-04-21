using System;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubPreviousRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubPrevious;

namespace UniCEC.Business.Services.ClubPreviousSvc
{
    public class ClubPreviousService : IClubPreviousService
    {
        private IClubPreviousRepo _clubPreviousRepo;

        public ClubPreviousService(IClubPreviousRepo clubPreviousRepo)
        {
            _clubPreviousRepo = clubPreviousRepo;
        }

        public async Task<PagingResult<ViewClubPrevious>> GetAllPaging(PagingRequest request)
        {
            PagingResult<ClubPreviou> listClubPrevious = await _clubPreviousRepo.GetAllPaging(request);
            if (listClubPrevious.Items == null) throw new NullReferenceException("Not found any previous clubs");
            throw new NotImplementedException();
        }

        public Task<ViewClubPrevious> GetByClubPrevious(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewClubPrevious>> GetByContitions(ClubPreviousRequestModel request)
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

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
