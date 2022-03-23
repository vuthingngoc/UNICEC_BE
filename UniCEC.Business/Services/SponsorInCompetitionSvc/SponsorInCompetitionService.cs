using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.SponsorInCompetition;

namespace UniCEC.Business.Services.SponsorInCompetitionSvc
{
    public class SponsorInCompetitionService : ISponsorInCompetitionService
    {
        private ISponsorInCompetitionRepo _sponsorInCompetitionRepo;

        public SponsorInCompetitionService(ISponsorInCompetitionRepo sponsorInCompetitionRepo)
        {
            _sponsorInCompetitionRepo = sponsorInCompetitionRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewSponsorInCompetition>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewSponsorInCompetition> GetBySponsorInCompetitionId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewSponsorInCompetition> Insert(SponsorInCompetitionInsertModel sponsorInCompetition)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewSponsorInCompetition sponsorInCompetition)
        {
            throw new NotImplementedException();
        }
    }
}
