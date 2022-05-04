using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.Member;

namespace UniCEC.Data.Repository.ImplRepo.MemberRepo
{
    public interface IMemberRepo : IRepository<Member>
    {
        public Task<List<ViewMember>> GetLeadersByClub(int clubId);
        public Task<int> GetQuantityNewMembersByClub(int clubId);
        public Task<bool> CheckExistedMemberInClub(int userId, int clubId);
    }
}
