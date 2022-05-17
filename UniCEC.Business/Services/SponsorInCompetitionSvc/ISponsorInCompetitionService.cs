using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.SponsorInCompetition;

namespace UniCEC.Business.Services.SponsorInCompetitionSvc
{
    public interface ISponsorInCompetitionService
    {
        public Task<PagingResult<ViewSponsorInCompetition>> GetAllPaging(PagingRequest request);
        public Task<ViewSponsorInCompetition> GetBySponsorInCompetitionId(int id);
        public Task<ViewSponsorInCompetition> Insert(SponsorInCompetitionInsertModel sponsorInCompetition, string token);
        public Task<bool> Update(ViewSponsorInCompetition sponsorInCompetition);
        public Task<bool> Delete(int id);
    }
}
