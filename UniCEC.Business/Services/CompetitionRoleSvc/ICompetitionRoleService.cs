using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Entities.CompetitionRole;

namespace UniCEC.Business.Services.CompetitionRoleSvc
{
    public interface ICompetitionRoleService
    {
        public Task<List<ViewCompetitionRole>> GetAll();

        public Task<ViewCompetitionRole> GetByCompetitionRoleId(int id);

        public Task<ViewCompetitionRole> Insert(CompetitionRoleInsertModel model);

        public Task<bool> Update(ViewCompetitionRole model);
    }
}
