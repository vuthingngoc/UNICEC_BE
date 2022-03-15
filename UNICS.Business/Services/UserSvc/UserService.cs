using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.UserRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.User;

namespace UNICS.Business.Services.UserSvc
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

        public Task<PagingResult<ViewUser>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewUser> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(UserInsertModel user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewUser user)
        {
            throw new NotImplementedException();
        }
    }
}
