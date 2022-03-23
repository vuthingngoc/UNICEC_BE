using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.SeedsWalletRepo
{
    public class SeedsWalletRepo : Repository<SeedsWallet>, ISeedsWalletRepo
    {
        public SeedsWalletRepo(UniCECContext context) : base(context)
        {

        }
    }
}
