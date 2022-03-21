using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.SponsorInCompetitionRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.SponsorInCompetition;

namespace UNICS.Business.Services.SponsorInCompetitionSvc
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
