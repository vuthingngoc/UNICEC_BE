using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.SeedsWallet;

namespace UNICS.Business.Services.SeedsWalletSvc
{
    public interface ISeedsWalletSvc
    {
        Task<PagingResult<ViewSeedsWallet>> GetAll(PagingRequest request);
        Task<ViewSeedsWallet> GetById(int id);
        Task<bool> Insert(SeedsWalletInsertModel seedsWallet);
        Task<bool> Update(ViewSeedsWallet seedsWallet);
        Task<bool> Delete(int id);
    }
}
