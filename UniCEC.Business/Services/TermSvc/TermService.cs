using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.TermRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Term;

namespace UniCEC.Business.Services.TermSvc
{
    public class TermService : ITermService
    {
        private ITermRepo _termRepo;
        public TermService(ITermRepo termRepo)
        {
            _termRepo = termRepo;
        }

        public Task<PagingResult<ViewTerm>> GetByClub(int clubId, PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewTerm>> GetByConditions(int clubId, TermRequestModel request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewTerm> GetById(int clubId, int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewTerm>> GetByName(int clubId, string name)
        {
            throw new NotImplementedException();
        }

        public Task<ViewTerm> Insert(TermInsertModel term)
        {
            throw new NotImplementedException();
        }

        public Task Update(ViewTerm term)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
