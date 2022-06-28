using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Common;
using System.Collections.Generic;
using UniCEC.Data.ViewModels.Entities.Major;
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
                        where d.UniversityId.Equals(request.UniversityId)
                        select d;


            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(d => d.Name.Contains(request.Name));

            if (request.MajorId.HasValue) query = query.Where(d => d.MajorId.Equals(request.MajorId));

            //if (!string.IsNullOrEmpty(request.MajorCode)) query = query.Where(m => m.MajorCode.Equals(request.MajorCode));

            if (request.Status.HasValue) query = query.Where(m => m.Status.Equals(request.Status.Value));

            int totalCount = query.Count();

            var items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                    .Select(m => new ViewDepartment()
                                    {
                                        Id = m.Id,
                                        UniversityId = m.UniversityId,
                                        MajorId = m.MajorId,
                                        Description = m.Description,
                                        //DepartmentCode = m.MajorCode,
                                        Name = m.Name,
                                        Status = m.Status
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
                .Select(m => new Department()
                {
                    Id = m.Id,
                    UniversityId = m.UniversityId,
                    MajorId = m.MajorId,
                    Description = m.Description,
                    //MajorCode = m.MajorCode,
                    Name = m.Name,
                    Status = m.Status
                }).ToListAsync();

            return (majors.Count > 0) ? new PagingResult<Department>(majors, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<int> CheckExistedDepartmentCode(int universityId, string code)
        {
            Department department = await context.Departments.FirstOrDefaultAsync(d => d.UniversityId.Equals(universityId));
                                                                        //&& d.MajorCode.Equals(code));
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
                        where d.Id.Equals(id)
                        select d;

            if (status.HasValue) query = query.Where(d => d.Status.Equals(status.Value));
            if (universityId.HasValue) query = query.Where(d => d.UniversityId.Equals(universityId));

            return await query.Select(m => new ViewDepartment()
            {
                UniversityId = m.UniversityId,
                MajorId = m.MajorId,
                Description = m.Description,
                Id = m.Id,
                //DepartmentCode = m.MajorCode,
                Name = m.Name,
                Status = m.Status
            }).FirstOrDefaultAsync();
        }

        public async Task<ViewDepartment> GetByCode(string majorCode, bool? status, int? universityId)
        {
            var query = from d in context.Departments
                        //where d.MajorCode.Equals(majorCode)
                        select d;

            if (status.HasValue) query = query.Where(d => d.Status.Equals(status.Value));

            if (universityId.HasValue) query = query.Where(d => d.UniversityId.Equals(universityId));

            return await query.Select(m => new ViewDepartment()
            {
                UniversityId = m.UniversityId,
                MajorId = m.MajorId,
                Description = m.Description,
                Id = m.Id,
                //DepartmentCode = m.MajorCode,
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
    }
}
