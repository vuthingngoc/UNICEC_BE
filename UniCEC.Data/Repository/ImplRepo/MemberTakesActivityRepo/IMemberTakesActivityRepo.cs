using System.Threading.Tasks;
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

        //check mem in club
        public Task<bool> CheckMemberInClub(int clubId, int memberId, int termId);

        //check mem take task
        public Task<bool> CheckMemberTakesTask(int clubActivityId, int memberId);

    }
}
