using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.TeamRoleRepo
{
    public interface ITeamRoleRepo : IRepository<TeamRole>
    {
        //
        public Task<int> GetRoleIdByName(string roleName);
    }
}
