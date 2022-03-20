using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.SeedsWalletRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.SeedsWallet;

namespace UNICS.Business.Services.SeedsWalletSvc
{
    public class SeedsWalletService : ISeedsWalletService
    {
        private ISeedsWalletRepo _seedsWalletRepo;

        public SeedsWalletService(ISeedsWalletRepo seedsWalletRepo)
        {
            _seedsWalletRepo = seedsWalletRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewSeedsWallet>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewSeedsWallet> GetBySeedsWalletId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewSeedsWallet> Insert(SeedsWalletInsertModel seedsWallet)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewSeedsWallet seedsWallet)
        {
            throw new NotImplementedException();
        }
    }
}
