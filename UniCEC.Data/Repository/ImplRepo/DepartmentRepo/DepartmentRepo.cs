using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Common;
using System.Collections.Generic;
using UniCEC.Data.ViewModels.Entities.Department;

namespace UniCEC.Data.Repository.ImplRepo.DepartmentRepo
{
    public class DepartmentRepo : Repository<Department>, IDepartmentRepo
    {
        public DepartmentRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<PagingResult<ViewDepartment>> GetByConditions(DepartmentRequestModel request)
        {
            var query = from d in context.Departments
                        join m in context.Majors on d.MajorId equals m.Id
                        where d.UniversityId.Equals(request.UniversityId)
                        select new { d, m };


            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(selector => selector.d.Name.Contains(request.Name));

            if (request.MajorId.HasValue) query = query.Where(selector => selector.d.MajorId.Equals(request.MajorId));

            if (request.Status.HasValue) query = query.Where(selector => selector.d.Status.Equals(request.Status.Value));

            int totalCount = query.Count();

            var items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                    .Select(selector => new ViewDepartment()
                                    {
                                        Id = selector.d.Id,
                                        UniversityId = selector.d.UniversityId,
                                        MajorId = selector.d.MajorId,
                                        MajorName = selector.m.Name,
                                        Description = selector.d.Description,
                                        DepartmentCode = selector.d.DepartmentCode,
                                        Name = selector.d.Name,
                                        Status = selector.d.Status
                                    }).ToListAsync();

            return (query.Any()) ? new PagingResult<ViewDepartment>(items, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<PagingResult<Department>> GetByUniversity(int universityId, PagingRequest request)
        {
            var query = from d in context.Departments
                        where d.UniversityId.Equals(universityId)
                        select d;

            int totalCount = query.Count();

            List<Department> majors = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                .Select(d => new Department()
                {
                    Id = d.Id,
                    UniversityId = d.UniversityId,
                    MajorId = d.MajorId,
                    Description = d.Description,
                    DepartmentCode = d.DepartmentCode,
                    Name = d.Name,
                    Status = d.Status
                }).ToListAsync();

            return (majors.Count > 0) ? new PagingResult<Department>(majors, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<int> CheckExistedDepartmentCode(int universityId, string code)
        {
            Department department = await context.Departments.FirstOrDefaultAsync(d => d.UniversityId.Equals(universityId) && d.DepartmentCode.Equals(code));
            return (department != null) ? department.Id : 0;
        }

        public async Task<List<int>> GetIdsByMajorId(int majorId, bool? status)
        {
            var query = from d in context.Departments
                        where d.MajorId == majorId
                        select d;

            if (status.HasValue) query = query.Where(d => d.Status.Equals(status.Value));

            List<int> departmentIds = await query.Select(d => d.Id).ToListAsync();
            return (departmentIds.Count > 0) ? departmentIds : null;
        }

        public async Task<ViewDepartment> GetById(int id, bool? status, int? universityId)
        {
            var query = from d in context.Departments
                        join m in context.Majors on d.MajorId equals m.Id
                        where d.Id.Equals(id)
                        select new { d, m };

            if (status.HasValue) query = query.Where(selector => selector.d.Status.Equals(status.Value));
            if (universityId.HasValue) query = query.Where(selector => selector.d.UniversityId.Equals(universityId));

            return await query.Select(selector => new ViewDepartment()
            {
                UniversityId = selector.d.UniversityId,
                MajorId = selector.d.MajorId,
                MajorName = selector.m.Name,
                Description = selector.d.Description,
                Id = selector.d.Id,
                DepartmentCode = selector.d.DepartmentCode,
                Name = selector.d.Name,
                Status = selector.d.Status
            }).FirstOrDefaultAsync();
        }

        public async Task<ViewDepartment> GetByCode(string departmentCode, bool? status, int? universityId)
        {
            var query = from d in context.Departments
                        where d.DepartmentCode.Equals(departmentCode)
                        select d;

            if (status.HasValue) query = query.Where(d => d.Status.Equals(status.Value));

            if (universityId.HasValue) query = query.Where(d => d.UniversityId.Equals(universityId));

            return await query.Select(m => new ViewDepartment()
            {
                UniversityId = m.UniversityId,
                MajorId = m.MajorId,
                Description = m.Description,
                Id = m.Id,
                DepartmentCode = m.DepartmentCode,
                Name = m.Name,
                Status = m.Status
            }).FirstOrDefaultAsync();
        }

        public async Task<int> CheckDuplicatedName(int universityId, string name)
        {
            return await (from d in context.Departments
                          where d.UniversityId.Equals(universityId) && d.Name.ToLower().Equals(name.ToLower())
                          select d.Id).FirstOrDefaultAsync();
        }

        public async Task<List<ViewDepartment>> GetAllByUniversity(int universityId)
        {
            List<Department> departments = await (from d in context.Departments
                                                  where d.UniversityId == universityId && d.Status.Equals(true)
                                                  select d).ToListAsync();

            List<ViewDepartment> result = new List<ViewDepartment>();

            foreach (Department m in departments)
            {
                ViewDepartment vd = new ViewDepartment()
                {
                    UniversityId = m.UniversityId,
                    MajorId = m.MajorId,
                    Description = m.Description,
                    Id = m.Id,
                    DepartmentCode = m.DepartmentCode,
                    Name = m.Name,
                    Status = m.Status
                };
                result.Add(vd);
            }
            return (result.Count > 0) ? result : null;
        }
    }
}
