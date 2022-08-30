using System.Collections.Generic;
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
        public Task<ViewUniversity> GetById(int id);
        //Get-Universities-By-Conditions
        public Task<PagingResult<ViewUniversity>> GetUniversitiesByConditions(UniversityRequestModel request);
        public Task<List<int>> GetListIdsUniByCity(int cityId); // for processing in city service
        //Check-Email-University
        public Task<bool> CheckEmailUniversity(string email);
        //Get-List-Universities-By-Email
        public Task<List<ViewUniversity>> GetListUniversityByEmail(string email);
        //
        public Task<List<ViewUniversity>> GetUniversities();
        public Task<string> GetNameUniversityById(int id);
        //test delete
        public Task DeleteUniversity(int UniversityId);
        public Task UpdateStatusByCityId(int cityId, bool status);
        public Task<int> CheckDuplicatedUniversity(string name, int cityId, string uniCode);
        public Task<bool> CheckExistedUniversity(int universityId);

    }
}
