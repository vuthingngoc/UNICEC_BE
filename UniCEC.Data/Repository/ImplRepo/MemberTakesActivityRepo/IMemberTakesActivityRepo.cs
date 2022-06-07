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
        public Task<PagingResult<ViewMemberTakesActivity>> GetAllTaskesByConditions(MemberTakesActivityRequestModel request);
        //check mem take task
        public Task<bool> CheckMemberTakesTask(int competitionActivityId, int memberId);
        public Task<bool> CheckTaskBelongToStudent(int MemberTakesActivityId, int UserId, int UniversityId);    
        public Task<int> GetNumOfMemInTask(int competitionActivityId);
        //Get number of member in task with status
        public Task<int> GetNumOfMemInTask_Status(int competitionActivityId, MemberTakesActivityStatus Status);

        //Update DeadLine Date 
        public Task UpdateDeadlineDate(int competitionActivityId, DateTime deadline);
    }
}
