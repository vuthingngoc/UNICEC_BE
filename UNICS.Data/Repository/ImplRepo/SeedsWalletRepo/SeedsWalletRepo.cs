using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.SeedsWalletRepo
{
    public class SeedsWalletRepo : Repository<SeedsWallet>, ISeedsWalletRepo
    {
        public SeedsWalletRepo(UNICSContext context) : base(context)
        {

        }
    }
}
