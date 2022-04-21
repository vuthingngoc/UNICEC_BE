using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Common;
using System.Collections.Generic;
using System;

namespace UniCEC.Data.Repository.ImplRepo.MajorRepo
{
    public class MajorRepo : Repository<Major>, IMajorRepo
    {
        public MajorRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<PagingResult<Major>> GetByCondition(MajorRequestModel request)
        {
            var query = from m in context.Majors
                        select new { m };
            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(x => x.m.Name.Contains(request.Name));

            if (request.DepartmentId.HasValue) query = query.Where(x => x.m.DepartmentId == request.DepartmentId);

            if (!string.IsNullOrEmpty(request.MajorCode)) query = query.Where(x => x.m.MajorCode.Equals(request.MajorCode));

            if (request.Status.HasValue) query = query.Where(x => x.m.Status == request.Status);

            int totalCount = query.Count();

            var items = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                    .Select(x => new Major()
                                        {       
                                            Id = x.m.Id,
                                            DepartmentId = x.m.DepartmentId,
                                            Description = x.m.Description,
                                            MajorCode = x.m.MajorCode,
                                            Name = x.m.Name,
                                            Status = x.m.Status
                                        }
                                    ).ToListAsync();
            return (items.Count > 0) ? new PagingResult<Major>(items, totalCount, request.CurrentPage, request.PageSize)
                                     : new PagingResult<Major>(null, 0, request.CurrentPage, request.PageSize);
        }

        public async Task<PagingResult<Major>> GetByUniversity(int universityId, PagingRequest request)
        {
            var query = from diu in context.DepartmentInUniversities                        
                        join d in context.Departments on diu.DepartmentId equals d.Id
                        join m in context.Majors on d.Id equals m.DepartmentId
                        where diu.UniversityId == universityId
                        select new { m };
            int totalCount = query.Count();

            List<Major> majors =  await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(x => new Major()
            {
                Id= x.m.Id,
                DepartmentId = x.m.DepartmentId,
                Description= x.m.Description,
                MajorCode= x.m.MajorCode,
                Name= x.m.Name,
                Status= x.m.Status
            }).ToListAsync();

            return (majors.Count > 0) ? new PagingResult<Major>(majors, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<int> CheckExistedMajorCode(int departmentId, string code)
        {
            Major major = await context.Majors.FirstOrDefaultAsync(m => m.DepartmentId.Equals(departmentId)
                                                                        && m.MajorCode.Equals(code));
            return (major != null) ? major.Id : 0;
        }
    }
}
