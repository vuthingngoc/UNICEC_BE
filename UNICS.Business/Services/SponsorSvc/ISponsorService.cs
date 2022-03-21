using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Sponsor;

namespace UNICS.Business.Services.SponsorSvc
{
    public interface ISponsorService
    {
        public Task<PagingResult<ViewSponsor>> GetAll(PagingRequest request);
        public Task<ViewSponsor> GetBySponsorId(int id);
        public Task<ViewSponsor> Insert(SponsorInsertModel sponsor);
        public Task<bool> Update(SponsorUpdateModel sponsor);
        public Task<bool> Delete(int id);
    }
}
