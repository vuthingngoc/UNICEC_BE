using System;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Models.DB;
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

            if (roleId.Equals(4)) return true;

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
            PagingResult<ViewSeedsWallet> seedsWallets = await _seedsWalletRepo.GetByConditions(request);
            if (seedsWallets == null) throw new NullReferenceException("Not found any seedswallets");

            bool valid = await CheckAuthorized(token, seedsWallets.Items[0].StudentId);
            if (valid) return seedsWallets;

            throw new UnauthorizedAccessException("You do not have permission to access this resource");
        }

        private ViewSeedsWallet TransferViewModel(SeedsWallet seedsWallet)
        {
            return new ViewSeedsWallet()
            {
                Id = seedsWallet.Id,
                StudentId = seedsWallet.StudentId,
                Amount = seedsWallet.Amount,
                Status = seedsWallet.Status
            };
        }

        public async Task InsertSeedsWallet(int studentId) // for new user
        {
            SeedsWallet seedsWallet = await _seedsWalletRepo.GetByStudentId(studentId);
            if (seedsWallet != null) return;

            seedsWallet = new SeedsWallet()
            {
                StudentId = studentId,
                Amount = 100, // default scores
                Status = true // default status
            };

            await _seedsWalletRepo.Insert(seedsWallet);
        }

        public async Task UpdateAmount(int studentId, double amount) 
        {
            SeedsWallet seedsWallet = await _seedsWalletRepo.GetByStudentId(studentId);
            if (seedsWallet == null) throw new NullReferenceException("Not found this seeds wallet");

            seedsWallet.Amount += amount;
            await _seedsWalletRepo.Update();
        }

        public async Task<ViewSeedsWallet> Insert(string token, int studentId) // future ...
        {
            if (studentId.Equals(0)) throw new ArgumentException("Student Null");

            int userId = _decodeToken.Decode(token, "Id");
            if (!userId.Equals(studentId)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            SeedsWallet seedsWallet = new SeedsWallet()
            {
                Amount = 0, // default amount
                StudentId = studentId,
                Status = true // default status
            };

            int id = await _seedsWalletRepo.Insert(seedsWallet);
            seedsWallet.Id = id;
            return TransferViewModel(seedsWallet);
        }

        public async Task Update(string token, SeedsWalletUpdateModel model) // future ...
        {
            bool isValid = await CheckAuthorized(token, model.StudentId);
            if (!isValid) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            //SeedsWallet seedsWallet = await _seedsWalletRepo.Get(model.Id);
            //if (seedsWallet == null) throw new NullReferenceException("Not found this seeds wallet");

            //int userId = _decodeToken.Decode(token, "Id");

            //if(model.Amount.HasValue && model.Amount.Value >= 0) seedsWallet.Amount = model.Amount.Value;

            //if(model.Status.HasValue && model.Status.Value.Equals(true) 
            //    && userId.Equals(model.StudentId)) 
            //        seedsWallet.Status = model.Status.Value;

            await UpdateAmount(model.StudentId, model.Amount.Value);

            //await _seedsWalletRepo.Update();
        }

        public async Task Delete(string token, int id) // future ...
        {
            SeedsWallet seedsWallet = await  _seedsWalletRepo.Get(id);
            if (seedsWallet == null) throw new NullReferenceException("Not found this seeds wallet");

            int userId = _decodeToken.Decode(token, "Id");
            if (!userId.Equals(seedsWallet.StudentId)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            seedsWallet.Status = false;
            await _seedsWalletRepo.Update();
        }
    }
}
