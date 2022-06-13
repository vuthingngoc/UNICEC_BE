using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Department;
using UniCEC.Data.RequestModels;

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
                        select d;

            if (!string.IsNullOrEmpty(request.Name)) query = query.Where(department => department.Name.Contains(request.Name));

            if (request.Status.HasValue) query = query.Where(department => department.Status.Equals(request.Status.Value));

            int totalCount = query.Count();
            List<ViewDepartment> departments = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                        .Select(department => new ViewDepartment()
                                                        {
                                                            Id = department.Id,
                                                            Name = department.Name,
                                                            Status = department.Status
                                                        }).ToListAsync();

            return (departments.Count > 0) ? new PagingResult<ViewDepartment>(departments, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<PagingResult<ViewDepartment>> GetByCompetition(int competitionId, PagingRequest request)
        {
            var query = from cid in context.CompetitionInDepartments
                        join d in context.Departments on cid.DepartmentId equals d.Id
                        where cid.CompetitionId == competitionId
                        select d;

            int totalCount = query.Count();

            List<ViewDepartment> departments = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                            .Select(department => new ViewDepartment()
                                                            {
                                                                Id = department.Id,
                                                                Name = department.Name,
                                                                Status = department.Status
                                                            }).ToListAsync();

            return (departments.Count > 0) ? new PagingResult<ViewDepartment>(departments, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        //
        public async Task<bool> checkDepartment(List<int> listDepartmentId)
        {
            bool result = true;
            foreach (int DepId in listDepartmentId)
            {
                var query = await (from dep in context.Departments
                                   where dep.Id == DepId
                                   select dep).FirstOrDefaultAsync();

                if (query == null)
                {
                    result = false;
                }
            }
            return result;
        }

        public async Task<ViewDepartment> GetById(int id, bool? status)
        {
            var query = from d in context.Departments
                        where d.Id.Equals(id)
                        select d;

            if (status.HasValue) query = query.Where(department => department.Status.Equals(status.Value));

            return await query.Select(d => new ViewDepartment()
            {
                Id = d.Id,
                Name = d.Name,
                Status = d.Status,
            }).FirstOrDefaultAsync();
        }
    }
}
