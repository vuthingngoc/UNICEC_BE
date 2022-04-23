using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.Repository.ImplRepo.ClubPreviousRepo
{
    public interface IClubPreviousRepo : IRepository<ClubPreviou>
    {
        Task<PagingResult<ClubPreviou>> GetByConditions(ClubPreviousRequestModel request);
        Task<List<int>> GetIdsByMember(int memberID);
        Task<int> CheckDuplicated(int clubId, int clubRoleId, int memberId, string year);

    }
}
