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
        public Task<ViewSeedsWallet> Insert(string token, SeedsWalletInsertModel seedsWallet);
        public Task<bool> Update(string token, ViewSeedsWallet seedsWallet);
        public Task<bool> Delete(string token, int id);
    }
}
