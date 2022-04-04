using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Sponsor;

namespace UniCEC.Business.Services.SponsorSvc
{
    public interface ISponsorService
    {
        public Task<PagingResult<ViewSponsor>> GetAllPaging(PagingRequest request);
        public Task<ViewSponsor> GetBySponsorId(int id);
        public Task<ViewSponsor> Insert(SponsorInsertModel sponsor);
        public Task<bool> Update(SponsorUpdateModel sponsor);
        public Task<bool> Delete(int id);
    }
}
