using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.SeedsWallet;

namespace UniCEC.Business.Services.SeedsWalletSvc
{
    public interface ISeedsWalletService
    {
        public Task<PagingResult<ViewSeedsWallet>> GetAllPaging(PagingRequest request);
        public Task<ViewSeedsWallet> GetBySeedsWalletId(int id);
        public Task<ViewSeedsWallet> Insert(SeedsWalletInsertModel seedsWallet);
        public Task<bool> Update(ViewSeedsWallet seedsWallet);
        public Task<bool> Delete(int id);
    }
}
