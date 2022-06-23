using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.RoleRepo;
using UniCEC.Data.ViewModels.Entities.Role;

namespace UniCEC.Business.Services.RoleSvc
{
    public class RoleService : IRoleService
    {
        private IRoleRepo _roleRepo;

        public RoleService(IRoleRepo roleRepo)
        {
            _roleRepo = roleRepo;
        }

        //Get ALL ROLES
        public async Task<List<ViewRole>> GetAll()
        {
            List<ViewRole> roles = await _roleRepo.GetAll();
            if (roles == null) throw new NullReferenceException();
            return roles;
        }

        //public async Task Delete(int id)
        //{
        //    Role role = await _roleRepo.Get(id);
        //    if(role == null) throw new NullReferenceException();
        //    await _roleRepo.Delete(role);
        //}


        public async Task<ViewRole> GetByRoleId(int id)
        {
            ViewRole role = await _roleRepo.GetById(id);
            if (role == null) throw new NullReferenceException();
            return role;
        }

        //Insert-Role
        public async Task<ViewRole> Insert(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException(" RoleName Null");

            Role role = new Role()
            {
                RoleName = roleName
            };
            int id = await _roleRepo.Insert(role);
            return (id > 0) ? await _roleRepo.GetById(id) : null;
        }

        //Update-Role
        public async Task Update(ViewRole model)
        {
            if (string.IsNullOrEmpty(model.RoleName) || model.Id == 0)
                throw new ArgumentNullException(" RoleName Null || Role Id Null");

            //get Role
            Role role = await _roleRepo.Get(model.Id);
            if(role == null) throw new NullReferenceException("Not found this role");

            //Update Role Name
            role.RoleName = model.RoleName;
            await _roleRepo.Update();
        }
    }
}
