using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.University;

namespace UniCEC.Data.Repository.ImplRepo.UniversityRepo
{
    public interface IUniversityRepo : IRepository<University>
    {
        //Get-Universities-By-Conditions
        public Task<PagingResult<ViewUniversity>> GetUniversitiesByConditions(UniversityRequestModel request);
    }
}
