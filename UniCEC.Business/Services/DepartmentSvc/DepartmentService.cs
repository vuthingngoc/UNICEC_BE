using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.DepartmentRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Department;
using System.Collections.Generic;

namespace UniCEC.Business.Services.DepartmentSvc
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepo _departmentRepo;

        public DepartmentService(IDepartmentRepo departmentRepo)
        {
            _departmentRepo = departmentRepo;
        }

        public Task<PagingResult<ViewDepartment>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<List<ViewDepartment>> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<ViewDepartment>> GetByCompetition(int competitionId)
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

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
