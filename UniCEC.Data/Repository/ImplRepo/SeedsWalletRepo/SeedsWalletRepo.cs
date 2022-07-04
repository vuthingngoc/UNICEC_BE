using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.SeedsWallet;

namespace UniCEC.Data.Repository.ImplRepo.SeedsWalletRepo
{
    public class SeedsWalletRepo : Repository<SeedsWallet>, ISeedsWalletRepo
    {
        public SeedsWalletRepo(UniCECContext context) : base(context)
        {

        }

        public Task<PagingResult<ViewSeedsWallet>> GetByConditions(SeedsWalletRequestModel request)
        {
            throw new System.NotImplementedException();
        }

        public Task<ViewSeedsWallet> GetById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
