using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Entities.CompetitionRole;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionRoleRepo
{
    public class CompetitionRoleRepo : Repository<CompetitionRole>, ICompetitionRoleRepo
    {
        public CompetitionRoleRepo(UniCECContext context) : base(context)
        {

        }

        public async Task<List<ViewCompetitionRole>> GetAll()
        {

            List<CompetitionRole> list_competitionRole = await (from cr in context.CompetitionRoles
                                                                select cr).ToListAsync();

            List<ViewCompetitionRole> listVCR = new List<ViewCompetitionRole>();

            foreach (CompetitionRole competitionRole in list_competitionRole)
            {
                ViewCompetitionRole vcr = new ViewCompetitionRole()
                {
                    Id = competitionRole.Id,
                    Name = competitionRole.RoleName
                };

                listVCR.Add(vcr);
            }
            return (listVCR.Count > 0) ? listVCR : null;
        }
    }
}
