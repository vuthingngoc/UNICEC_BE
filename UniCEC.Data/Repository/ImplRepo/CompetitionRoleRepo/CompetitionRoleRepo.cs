using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.CompetitionRoleRepo
{
    public class CompetitionRoleRepo : Repository<CompetitionRole>, ICompetitionRoleRepo
    {
        public CompetitionRoleRepo(UniCECContext context) : base(context)
        {

        }
    }
}
