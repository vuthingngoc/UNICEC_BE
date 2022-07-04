using System;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Repository.ImplRepo.SeedsWalletRepo;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.SeedsWallet;

namespace UniCEC.Business.Services.SeedsWalletSvc
{
    public class SeedsWalletService : ISeedsWalletService
    {
        private ISeedsWalletRepo _seedsWalletRepo;
        private IUserRepo _userRepo;
        private DecodeToken _decodeToken;

        public SeedsWalletService(ISeedsWalletRepo seedsWalletRepo, IUserRepo userRepo)
        {
            _seedsWalletRepo = seedsWalletRepo;
            _userRepo = userRepo;
            _decodeToken = new DecodeToken();
        }

        private async Task<bool> CheckAuthorized(string token, int studentId)
        {
            int userId = _decodeToken.Decode(token, "Id");
            int roleId = _decodeToken.Decode(token, "RoleId");
            int universityId = _decodeToken.Decode(token, "UniversityId");
            bool isSameUniversity = await _userRepo.CheckExistedUser(universityId, studentId);

            if (studentId.Equals(userId)
                || roleId.Equals(1) && isSameUniversity)
                return true;

            return false;
        }

        public async Task<ViewSeedsWallet> GetById(string token, int id)
        {
            ViewSeedsWallet seedsWallet = await _seedsWalletRepo.GetById(id);
            if (seedsWallet == null) throw new NullReferenceException();

            bool valid = await CheckAuthorized(token, seedsWallet.StudentId);
            if (valid) return seedsWallet;

            throw new UnauthorizedAccessException("You do not have permission to access this resource");
        }

        public async Task<PagingResult<ViewSeedsWallet>> GetByConditions(string token, SeedsWalletRequestModel request)
        {
            //bool isValid = CheckAuthorized(token, request.)

            PagingResult<ViewSeedsWallet> seedsWallets = await _seedsWalletRepo.GetByConditions(request);
            throw new NotImplementedException();
        }

        public Task<ViewSeedsWallet> Insert(string token, SeedsWalletInsertModel seedsWallet)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(string token, ViewSeedsWallet seedsWallet)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string token, int id)
        {
            throw new NotImplementedException();
        }
    }
}
