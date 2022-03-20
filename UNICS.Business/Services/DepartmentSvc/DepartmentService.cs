using System;
using System.Threading.Tasks;
using UNICS.Data.Repository.ImplRepo.DepartmentRepo;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.Department;

namespace UNICS.Business.Services.DepartmentSvc
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepo _departmentRepo;

        public DepartmentService(IDepartmentRepo departmentRepo)
        {
            _departmentRepo = departmentRepo;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagingResult<ViewDepartment>> GetAll(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ViewDepartment> GetByDepartmentId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewDepartment> Insert(DepartmentInsertModel department)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewDepartment department)
        {
            throw new NotImplementedException();
        }
    }
}
