using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.UserRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.Business.Services.UserSvc
{
    public class UserService : IUserService
    {
        private IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewUser>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewUser> GetByUserId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewUser> Insert(UserInsertModel user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewUser user)
        {
            throw new NotImplementedException();
        }
    }
}
