using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.MemberTakesActivity;

namespace UniCEC.Business.Services.MemberTakesActivitySvc
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

        public Task<PagingResult<ViewMemberTakesActivity>> GetAllPaging(PagingRequest request)
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
