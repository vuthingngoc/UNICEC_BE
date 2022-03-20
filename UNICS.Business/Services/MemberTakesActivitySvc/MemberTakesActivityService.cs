using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.MemberTakesActivityRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.MemberTakesActivity;

namespace UNICS.Business.Services.MemberTakesActivitySvc
{
    public class MemberTakesActivityService : IMemberTakesActivityService
    {
        private IMemberTakesActivityRepo _memberTakesActivityRepo;

        public MemberTakesActivityService(IMemberTakesActivityRepo memberTakesActivityRepo)
        {
            _memberTakesActivityRepo = memberTakesActivityRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewMemberTakesActivity>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewMemberTakesActivity> GetByMemberTakesActivityId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewMemberTakesActivity> Insert(MemberTakesActivityInsertModel memberTakesActivity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewMemberTakesActivity memberTakesActivity)
        {
            throw new NotImplementedException();
        }
    }
}
