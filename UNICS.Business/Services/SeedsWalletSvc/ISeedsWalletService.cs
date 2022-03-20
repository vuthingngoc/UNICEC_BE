using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.SeedsWallet;

namespace UNICS.Business.Services.SeedsWalletSvc
{
    public interface ISeedsWalletService
    {
        public Task<PagingResult<ViewSeedsWallet>> GetAll(PagingRequest request);
        public Task<ViewSeedsWallet> GetBySeedsWalletId(int id);
        public Task<ViewSeedsWallet> Insert(SeedsWalletInsertModel seedsWallet);
        public Task<bool> Update(ViewSeedsWallet seedsWallet);
        public Task<bool> Delete(int id);
    }
}
