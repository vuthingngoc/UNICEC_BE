using System;
using System.Threading.Tasks;
using UNICS.Data.Models.DB;
using UNICS.Data.Repository.ImplRepo.CompetitionRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Competition;

namespace UNICS.Business.Services.CompetitionSvc
{
    public class CompetitionSvc : ICompetitionSvc
    {
        private ICompetitionRepo _competitionRepo;

        public CompetitionSvc(ICompetitionRepo competitionRepo)
        {
            _competitionRepo = competitionRepo;
        }

        public Task<PagingResult<Competition>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<Competition>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Competition> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<Competition>> Insert(CompetitionInsertModel competition)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<Competition>> Update(ViewCompetition competition)
        {
            throw new NotImplementedException();
        }
    }
}
