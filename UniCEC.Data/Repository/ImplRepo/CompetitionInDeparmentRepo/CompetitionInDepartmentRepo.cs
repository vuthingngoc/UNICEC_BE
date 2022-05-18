using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionInDeparmentRepo
{
    public class CompetitionInDepartmentRepo : Repository<CompetitionInDepartment>, ICompetitionInDepartmentRepo
    {
        public CompetitionInDepartmentRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<List<int>> GetListDepartmentId_In_Competition(int CompetitionId)
        {
            List<int> departmentIdList = await(from cid in context.CompetitionInDepartments
                                         where CompetitionId == cid.CompetitionId
                                         select cid.DepartmentId).ToListAsync();

            if (departmentIdList.Count > 0)
            {
                return departmentIdList;
            }

            return null;
        }
    }
}
