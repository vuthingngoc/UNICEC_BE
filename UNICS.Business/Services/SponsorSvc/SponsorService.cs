using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.SponsorRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Sponsor;

namespace UNICS.Business.Services.SponsorSvc
{
    public class SponsorService : ISponsorService
    {
        private ISponsorRepo _sponsorRepo;

        public SponsorService(ISponsorRepo sponsorRepo)
        {
            _sponsorRepo = sponsorRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewSponsor>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewSponsor> GetBySponsorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewSponsor> Insert(SponsorInsertModel sponsor)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(SponsorUpdateModel sponsor)
        {
            throw new NotImplementedException();
        }
    }
}
