using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.DepartmentInUniversityRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.DepartmentInUniversity;

namespace UNICS.Business.Services.DepartmentInUniversitySvc
{
    public class DepartmentInUniversityService : IDepartmentInUniversityService
    {
        private IDepartmentInUniversityRepo _departmentInUniversityRepo;

        public DepartmentInUniversityService(IDepartmentInUniversityRepo departmentInUniversityRepo)
        {
            _departmentInUniversityRepo = departmentInUniversityRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewDepartmentInUniversity>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewDepartmentInUniversity> GetByDepartmentInUniversityId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewDepartmentInUniversity> Insert(DepartmentInUniversityInsertModel departmentInUniversity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewDepartmentInUniversity departmentInUniversity)
        {
            throw new NotImplementedException();
        }
    }
}
