using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.SeedsWallet;

namespace UniCEC.Data.Repository.ImplRepo.SeedsWalletRepo
{
    public interface ISeedsWalletRepo : IRepository<SeedsWallet>
    {
        public Task<ViewSeedsWallet> GetById(int id);
        public Task<SeedsWallet> GetByStudentId(int studentId);
        public Task<PagingResult<ViewSeedsWallet>> GetByConditions(SeedsWalletRequestModel request);
    }
}
