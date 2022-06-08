using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.ClubRoleRepo;
using UniCEC.Data.ViewModels.Entities.ClubRole;

namespace UniCEC.Business.Services.ClubRoleSvc
{
    public class ClubRoleService : IClubRoleService
    {
        private IClubRoleRepo _clubRoleRepo;

        private JwtSecurityTokenHandler _tokenHandler;

        public ClubRoleService(IClubRoleRepo clubRoleRepo)
        {
            _clubRoleRepo = clubRoleRepo;
        }

        public int DecodeToken(string token, string nameClaim)
        {
            if(_tokenHandler == null) _tokenHandler = new JwtSecurityTokenHandler();
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(x => x.Type.ToString().Equals(nameClaim));
            return Int32.Parse(claim.Value);
        }

        public async Task Delete(int id, string token)
        {
            int roleId = DecodeToken(token, "RoleId");
            // if not system admin
            if (roleId != 4) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            ClubRole clubRole = await _clubRoleRepo.Get(id);
            if (clubRole == null) throw new NullReferenceException("Not found this club role");

            await _clubRoleRepo.Delete(clubRole);
        }

        public async Task<List<ViewClubRole>> GetAll()
        {
            List<ViewClubRole> clubRoles = await _clubRoleRepo.GetAll();
            if (clubRoles == null) throw new NullReferenceException();
            return clubRoles;
        }

        public async Task<ViewClubRole> GetById(int id)
        {
            ViewClubRole clubRole = await _clubRoleRepo.GetById(id);
            if (clubRole == null) throw new NullReferenceException();
            return clubRole;
        }

        public async Task<ViewClubRole> Insert(string name, string token)
        {
            int roleId = DecodeToken(token, "RoleId");
            // if not system admin
            if (roleId != 4) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            ClubRole clubRole = new ClubRole()
            {
                Name = name,
            };
            int id = await _clubRoleRepo.Insert(clubRole);
            return await GetById(id);
        }

        public async Task Update(ViewClubRole model, string token)
        {
            int roleId = DecodeToken(token, "RoleId");
            // if not system admin
            if (roleId != 4) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            ClubRole clubRole = await _clubRoleRepo.Get(model.Id);
            if(clubRole == null) throw new NullReferenceException();

            clubRole.Name = model.Name;
            await _clubRoleRepo.Update();
            
        }
    }
}
