using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.ClubActivity;

namespace UniCEC.Data.Repository.ImplRepo.ClubActivityRepo
{
    public interface IClubActivityRepo : IRepository<ClubActivity>
    {
        //
        public Task<bool> CheckExistCode(string code);
        //
        public Task<PagingResult<ViewClubActivity>> GetListClubActivitiesByConditions(ClubActivityRequestModel conditions);
    }
}
