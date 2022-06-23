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
    public interface ICompetitionRoleRepo : IRepository<CompetitionRole>
    {
        public Task<List<ViewCompetitionRole>> GetAll();
    }
}
