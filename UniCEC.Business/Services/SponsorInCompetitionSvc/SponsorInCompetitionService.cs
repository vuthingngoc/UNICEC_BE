using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.SponsorInCompetitionRepo;
using UniCEC.Data.Repository.ImplRepo.SponsorRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.SponsorInCompetition;

namespace UniCEC.Business.Services.SponsorInCompetitionSvc
{
    public class SponsorInCompetitionService : ISponsorInCompetitionService
    {
        private ISponsorInCompetitionRepo _sponsorInCompetitionRepo;
        //
        private ISponsorRepo _sponsorRepo;
        public SponsorInCompetitionService(ISponsorInCompetitionRepo sponsorInCompetitionRepo, ISponsorRepo sponsorRepo)
        {
            _sponsorInCompetitionRepo = sponsorInCompetitionRepo;
            _sponsorRepo = sponsorRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewSponsorInCompetition>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewSponsorInCompetition> GetBySponsorInCompetitionId(int id)
        {
            throw new NotImplementedException();
        }

      

        //Sponsor Create Competition - Event
        public async Task<ViewSponsorInCompetition> Insert(SponsorInCompetitionInsertModel model, string token)
        {
            
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewSponsorInCompetition sponsorInCompetition)
        {
            throw new NotImplementedException();
        }

        private ViewSponsorInCompetition TransferView(SponsorInCompetition sponsorInCompetition)
        {
            return new ViewSponsorInCompetition()
            {
                Id = sponsorInCompetition.Id,
                SponsorId = sponsorInCompetition.SponsorId,
                CompetitionId = sponsorInCompetition.CompetitionId,
            };
        }
    }
}
