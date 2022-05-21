using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.RoleRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
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

        private ViewRole TransformViewRole(Role role)
        {

            return new ViewRole()
            {
                Id = role.Id,
                RoleName = role.RoleName,
            };
        }

        //Get ALL ROLES
        public async Task<PagingResult<ViewRole>> GetAllPaging(PagingRequest request)
        {
            PagingResult<Role> result = await _roleRepo.GetAllPaging(request);
            if (result != null)
            {
                List<ViewRole> listViewRole = new List<ViewRole>();
                result.Items.ForEach(e =>
                {
                    ViewRole viewRole = TransformViewRole(e);
                    listViewRole.Add(viewRole);
                });

                return new PagingResult<ViewRole>(listViewRole, result.TotalCount, request.CurrentPage, request.PageSize);
            }
            throw new NullReferenceException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<ViewRole> GetByRoleId(int id)
        {
            //
            Role role = await _roleRepo.Get(id);
            if (role != null)
            {
                ViewRole viewRole = TransformViewRole(role);
                return viewRole;
            }
            throw new NullReferenceException();

        }

        //Insert-Role
        public async Task<ViewRole> Insert(RoleInsertModel model)
        {
            try
            {

                if (string.IsNullOrEmpty(model.RoleName))
                    throw new ArgumentNullException(" RoleName Null");

                Role role = new Role();
                role.RoleName = model.RoleName;
                int result = await _roleRepo.Insert(role);
                if (result > 0)
                {
                    Role getRole = await _roleRepo.Get(result);
                    return TransformViewRole(getRole);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Update-Role
        public async Task<bool> Update(ViewRole role)
        {
            try
            {
                if (string.IsNullOrEmpty(role.RoleName) || role.Id == 0)
                    throw new ArgumentNullException(" RoleName Null || Role Id Null");

                //get Role
                Role getRole = await _roleRepo.Get(role.Id);
                if (getRole != null)
                {
                    //Update Role Name
                    getRole.RoleName = (!role.RoleName.Equals("")) ? role.RoleName : getRole.RoleName;
                    await _roleRepo.Update();
                    return true;
                }
                else
                {
                    throw new ArgumentException(" Role not found");
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
