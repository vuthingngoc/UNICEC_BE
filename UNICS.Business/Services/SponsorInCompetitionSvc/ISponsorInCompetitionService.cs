using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.SponsorInCompetition;

namespace UNICS.Business.Services.SponsorInCompetitionSvc
{
    public interface ISponsorInCompetitionService
    {
        public Task<PagingResult<ViewSponsorInCompetition>> GetAll(PagingRequest request);
        public Task<ViewSponsorInCompetition> GetBySponsorInCompetitionId(int id);
        public Task<ViewSponsorInCompetition> Insert(SponsorInCompetitionInsertModel sponsorInCompetition);
        public Task<bool> Update(ViewSponsorInCompetition sponsorInCompetition);
        public Task<bool> Delete(int id);
    }
}
