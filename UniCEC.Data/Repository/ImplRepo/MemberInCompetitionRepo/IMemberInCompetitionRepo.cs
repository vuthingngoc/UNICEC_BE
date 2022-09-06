using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.MemberInCompetition;

namespace UniCEC.Data.Repository.ImplRepo.MemberInCompetitionRepo
{
    public interface IMemberInCompetitionRepo : IRepository<MemberInCompetition>
    {
        public bool CheckValidManagerByUser(int competitionId, int userId, int? competitionRoleId);

        public Task<PagingResult<ViewMemberInCompetition>> GetAllManagerCompOrEve(MemberInCompetitionRequestModel request);

        public Task<List<MemberInCompetition>> GetAllManagerCompOrEve(int competitionId); // for sending noti

        public Task<MemberInCompetition> GetMemberInCompetition(int competitionId, int memberId);

       
    }
}
