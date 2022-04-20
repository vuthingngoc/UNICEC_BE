using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.DepartmentRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Department;
using System.Collections.Generic;
using UniCEC.Data.Models.DB;

namespace UniCEC.Business.Services.DepartmentSvc
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepo _departmentRepo;

        public DepartmentService(IDepartmentRepo departmentRepo)
        {
            _departmentRepo = departmentRepo;
        }

        private ViewDepartment TranformViewDepartment(Department department)
        {
            return new ViewDepartment()
            {
                Id = department.Id,
                Name = department.Name,
                Status = department.Status,                
            };
        }

        public async Task<ViewDepartment> GetByDepartment(int id)
        {
            Department department = await _departmentRepo.Get(id);            
            return (department != null) ? TranformViewDepartment(department) : throw new NullReferenceException("Not found this department");
        }

        public async Task<PagingResult<ViewDepartment>> GetAllPaging(PagingRequest request)
        {
            PagingResult<Department> departments = await _departmentRepo.GetAllPaging(request);
            if (departments.Items == null) throw new NullReferenceException("Not Found");

            List<ViewDepartment> items = new List<ViewDepartment>();
            departments.Items.ForEach(item =>
            {
                ViewDepartment department = TranformViewDepartment(item);
                items.Add(department);
            });
            return new PagingResult<ViewDepartment>(items, departments.TotalCount, departments.CurrentPage, departments.PageSize);
        }

        public async Task<List<ViewDepartment>> GetByName(string name)
        {
            List<Department> listDepartment = await _departmentRepo.GetByName(name);
            if (listDepartment == null) throw new NullReferenceException("Not found any departments");

            List<ViewDepartment> departments = new List<ViewDepartment>();
            listDepartment.ForEach(element =>
            {
                ViewDepartment department = TranformViewDepartment(element);
                departments.Add(department);
            });
            return departments;
        }

        public async Task<List<ViewDepartment>> GetByCompetition(int competitionId)
        {
            List<Department> listDepartment = await _departmentRepo.GetByCompetition(competitionId);
            if (listDepartment == null) throw new NullReferenceException("Not found any departments");

            List<ViewDepartment> departments = new List<ViewDepartment>();
            listDepartment.ForEach(element =>
            {
                ViewDepartment department = TranformViewDepartment(element);
                departments.Add(department);
            });
            return departments;
        }

        public async Task<ViewDepartment> Insert(DepartmentInsertModel department)
        {
            if (department == null) throw new ArgumentNullException("Null Argument");
            // default inserted status is true
            bool status = true;
            Department element = new Department()
            {
                Name = department.Name,
                Status = status
            };
            int id = await _departmentRepo.Insert(element);
            if (id > 0)
            {
                element.Id = id;
                return TranformViewDepartment(element);
            }

            return null;
        }

        public async Task<bool> Update(ViewDepartment department)
        {
            if (department == null) throw new ArgumentNullException("Null Argument");
            Department element = await _departmentRepo.Get(department.Id);
            if(element != null)
            {
                element.Name = department.Name;
                element.Status = department.Status;
                return await _departmentRepo.Update();                 
            }
            throw new NullReferenceException("Not found this element");
        }

        public async Task<bool> Delete(int id)
        {
            Department element = await _departmentRepo.Get(id);
            if(element != null)
            {
                if (element.Status == false) return true;
                element.Status = false;
                return await _departmentRepo.Update();
            }
            throw new NullReferenceException("Not found this element");
        }
    }
}
