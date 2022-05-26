using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.Competition;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionInDeparmentRepo
{
    public class CompetitionInDepartmentRepo : Repository<CompetitionInDepartment>, ICompetitionInDepartmentRepo
    {
        public CompetitionInDepartmentRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<List<int>> GetListDepartmentId_In_Competition(int CompetitionId)
        {
            List<int> department_Id_List = await (from cid in context.CompetitionInDepartments
                                                  where CompetitionId == cid.CompetitionId
                                                  select cid.DepartmentId).ToListAsync();
            if (department_Id_List.Count > 0)
            {
                return department_Id_List;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<ViewDeparmentInComp>> GetListDepartment_In_Competition(int CompetitionId)
        {
            List<CompetitionInDepartment> department_In_CompetitionList = await (from cid in context.CompetitionInDepartments
                                                                                 where CompetitionId == cid.CompetitionId
                                                                                 select cid).ToListAsync();
            List<ViewDeparmentInComp> list_vdic = new List<ViewDeparmentInComp>();
            if (department_In_CompetitionList.Count > 0)
            {
                foreach (var department_In_Competition in department_In_CompetitionList)
                {
                    Department department = await (from d in context.Departments
                                                   where d.Id == department_In_Competition.DepartmentId
                                                   select d).FirstOrDefaultAsync();

                    ViewDeparmentInComp vdic = new ViewDeparmentInComp()
                    {
                        Id = department.Id,
                        Name = department.Name,
                    };
                    list_vdic.Add(vdic);
                }

                if (list_vdic.Count > 0)
                {
                    return list_vdic;
                }
            }
            return null;
        }
    }
}
