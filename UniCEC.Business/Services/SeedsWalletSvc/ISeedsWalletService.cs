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
        public Task InsertSeedsWallet(int studentId);
        public Task UpdateAmount(int studentId, double amount);
        public Task<ViewSeedsWallet> Insert(string token, int studentId); // future
        public Task Update(string token, SeedsWalletUpdateModel model); // future
        public Task Delete(string token, int id); // future
    }
}
