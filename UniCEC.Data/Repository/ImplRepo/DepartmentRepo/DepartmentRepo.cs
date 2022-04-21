using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.Repository.ImplRepo.DepartmentRepo
{
    public class DepartmentRepo : Repository<Department>, IDepartmentRepo
    {
        public DepartmentRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<PagingResult<Department>> GetByName(string name, PagingRequest request)
        {
            var query = from d in context.Departments
                        where d.Name.Contains(name)
                        select new { d };
            int totalCount = query.Count();
            List<Department> departments = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(x => new Department()
            {
                Id = x.d.Id,
                Name = x.d.Name
            }).ToListAsync();

            return (departments.Count > 0) ? new PagingResult<Department>(departments, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task<PagingResult<Department>> GetByCompetition(int competitionId, PagingRequest request)
        {
            var query = from cid in context.CompetitionInDepartments
                        join d in context.Departments on cid.DepartmentId equals d.Id
                        where cid.CompetitionId == competitionId
                        select new { d };
            int totalCount = query.Count();

            List<Department> departments = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize).Select(x =>
                new Department()
                {
                    Id = x.d.Id,
                    Name = x.d.Name
                }
            ).ToListAsync();
            return (departments.Count > 0) ? new PagingResult<Department>(departments, totalCount, request.CurrentPage, request.PageSize) : null;
        }
    }
}
