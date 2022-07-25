using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Competition;
using UniCEC.Data.ViewModels.Entities.CompetitionEntity;

namespace UniCEC.Business.Services.CompetitionEntitySvc
{
    public interface ICompetitionEntityService
    {
        public Task<List<ViewCompetitionEntity>> AddImage(ImageInsertModel model, string token);

        public Task<List<ViewCompetitionEntity>> AddSponsor(SponsorInsertModel model, string token);

        public Task<List<ViewCompetitionEntity>> AddInfluencer (InfluencerInsertModel model, string token); 

        public Task<bool> DeleteCompetitionEntity(int competitionId, int clubId, string token);
    }
}
