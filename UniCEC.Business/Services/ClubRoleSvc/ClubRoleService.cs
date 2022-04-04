using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.ClubRoleRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubRole;

namespace UniCEC.Business.Services.ClubRoleSvc
{
    public class ClubRoleService : IClubRoleService
    {
        private IClubRoleRepo _clubRoleRepo;

        public ClubRoleService(IClubRoleRepo clubRoleRepo)
        {
            _clubRoleRepo = clubRoleRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewClubRole>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewClubRole> GetByClubRoleId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewClubRole> Insert(ClubRoleInsertModel clubRole)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewClubRole clubRole)
        {
            throw new NotImplementedException();
        }
    }
}
