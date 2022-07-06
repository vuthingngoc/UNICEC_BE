using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.SeedsWallet;

namespace UniCEC.Business.Services.SeedsWalletSvc
{
    public interface ISeedsWalletService
    {
        public Task<ViewSeedsWallet> GetById(string token, int id);
        public Task<PagingResult<ViewSeedsWallet>> GetByConditions(string token, SeedsWalletRequestModel request);
        public Task<ViewSeedsWallet> Insert(string token, int studentId);
        public Task Update(string token, SeedsWalletUpdateModel model);
        public Task Delete(string token, int id);
    }
}
