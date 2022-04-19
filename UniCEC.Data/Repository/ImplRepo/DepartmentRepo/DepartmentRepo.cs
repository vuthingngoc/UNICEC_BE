using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UniCEC.Data.Repository.ImplRepo.DepartmentRepo
{
    public class DepartmentRepo : Repository<Department>, IDepartmentRepo
    {
        public DepartmentRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<List<Department>> GetByName(string name)
        {
            var query = from d in context.Departments
                        where d.Name.Contains(name)
                        select new { d };
            return await query.Select(x => new Department()
            {
                Id = x.d.Id,
                Name = x.d.Name
            }).ToListAsync();
        }

        public async Task<List<Department>> GetByCompetition(int competitionId)
        {
            var query = from cid in context.CompetitionInDepartments
                        join d in context.Departments on cid.DepartmentId equals d.Id
                        where cid.CompetitionId == competitionId
                        select new { d };
            return await query.Select(x => 
                new Department()
                {
                    Id = x.d.Id,
                    Name= x.d.Name
                }
            ).ToListAsync();            
        }
    }
}
