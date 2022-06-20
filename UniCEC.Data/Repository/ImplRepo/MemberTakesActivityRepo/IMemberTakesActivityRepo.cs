using System;
using System.Threading.Tasks;
using UniCEC.Data.Enum;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.MemberTakesActivity;

namespace UniCEC.Data.Repository.ImplRepo.MemberTakesActivityRepo
{
    public interface IMemberTakesActivityRepo : IRepository<MemberTakesActivity>
    {

        //--------------------------Refactor
        //Manager
        public Task<PagingResult<ViewMemberTakesActivity>> GetAllTasksByConditions(MemberTakesActivityRequestModel request);

        //Member
        public Task<PagingResult<ViewMemberTakesActivity>> GetAllTasksMemberByConditions(MemberTakesActivityRequestModel request, int userId);

        //check mem take task
        public Task<MemberTakesActivity> CheckMemberTakesTask(int competitionActivityId, int memberId);

        //remove member take task 
        public Task<bool> RemoveMemberTakeTask(int memberTakeActivityId);

        //
        public Task<bool> CheckTaskBelongToStudent(int memberTakesActivityId, int userId, int clubId);

        //
        public Task<int> GetNumberOfMemberIsSubmitted(int competitionActivityId);

        //Update DeadLine Date 
        public Task UpdateDeadlineDate(int competitionActivityId, DateTime deadline);
    }
}
