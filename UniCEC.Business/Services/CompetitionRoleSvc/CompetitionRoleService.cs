using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.CompetitionRoleRepo;
using UniCEC.Data.ViewModels.Entities.CompetitionRole;

namespace UniCEC.Business.Services.CompetitionRoleSvc
{
    public class CompetitionRoleService : ICompetitionRoleService
    {
        private ICompetitionRoleRepo _competitionRoleRepo;
        public CompetitionRoleService(ICompetitionRoleRepo competitionRoleRepo)
        {
            _competitionRoleRepo = competitionRoleRepo;
        }

        public async Task<List<ViewCompetitionRole>> GetAll()
        {
            try
            {
                List<ViewCompetitionRole> competitionRoles = await _competitionRoleRepo.GetAll();
                if (competitionRoles == null) throw new NullReferenceException();
                return competitionRoles;
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public async Task<ViewCompetitionRole> GetByCompetitionRoleId(int id)
        {
            try
            {
                CompetitionRole competitionRole = await _competitionRoleRepo.Get(id);
                if (competitionRole == null) throw new NullReferenceException();

                return new ViewCompetitionRole()
                {
                    Id = competitionRole.Id,
                    Name = competitionRole.RoleName
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ViewCompetitionRole> Insert(CompetitionRoleInsertModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Name))
                    throw new ArgumentNullException(" RoleName Null");

                CompetitionRole competitionRole = new CompetitionRole()
                {
                    RoleName = model.Name
                };
                int id = await _competitionRoleRepo.Insert(competitionRole);
                if (id < 0) { throw new ArgumentException(" Add Failed ! "); }
                return new ViewCompetitionRole()
                {
                    Id = id,
                    Name = model.Name
                };
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> Update(ViewCompetitionRole model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Name) || model.Id == 0)
                    throw new ArgumentNullException(" RoleName Null || Role Id Null");

                //get Competition Role
                CompetitionRole competitionRole = await _competitionRoleRepo.Get(model.Id);
                if (competitionRole == null) throw new NullReferenceException("Not found this role");

                //Update Competition Role Name
                competitionRole.RoleName = model.Name;
                await _competitionRoleRepo.Update();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
