using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.DepartmentRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Department;
using System.Collections.Generic;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.MajorRepo;
using UniCEC.Data.RequestModels;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace UniCEC.Business.Services.DepartmentSvc
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepo _departmentRepo;
        private IMajorRepo _majorRepo;

        private JwtSecurityTokenHandler _tokenHandler;

        public DepartmentService(IDepartmentRepo departmentRepo, IMajorRepo majorRepo)
        {
            _departmentRepo = departmentRepo;
            _majorRepo = majorRepo;
        }

        private int DecodeToken(string token, string nameClaim)
        {
            if(_tokenHandler == null) _tokenHandler = new JwtSecurityTokenHandler();
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
            return Int32.Parse(claim.Value);
        }

        public async Task<ViewDepartment> GetById(string token, int id)
        {
            int roleId = DecodeToken(token, "RoleId");
            bool? status = null;
            if (!roleId.Equals(4)) status = true;
            ViewDepartment department = await _departmentRepo.GetById(id, status);
            return (department != null) ? department : throw new NullReferenceException("Not found this department");
        }

        public async Task<PagingResult<ViewDepartment>> GetByConditions(string token, DepartmentRequestModel request) // not finish
        {
            int roleId = DecodeToken(token, "RoleId");
            if (!roleId.Equals(4)) request.Status = true;
            PagingResult<ViewDepartment> departments = await _departmentRepo.GetByConditions(request);
            if (departments == null) throw new NullReferenceException("Not found any departments");
            return (departments != null) ? departments : throw new NullReferenceException();
        }

        public async Task<PagingResult<ViewDepartment>> GetByCompetition(int competitionId, PagingRequest request)
        {
            PagingResult<ViewDepartment> departments = await _departmentRepo.GetByCompetition(competitionId, request);
            return (departments != null) ? departments : throw new NullReferenceException();
        }

        public async Task<ViewDepartment> Insert(string token, string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Name Null");

            int roleId = DecodeToken(token, "RoleId");
            if (!roleId.Equals(4)) throw new UnauthorizedAccessException("You do not have permission to access this resource"); // system admin

            Department department = new Department()
            {
                Name = name,
                Status = true // default inserted status is true 
            };
            int id = await _departmentRepo.Insert(department);
            return (id > 0) ? await _departmentRepo.GetById(id, department.Status) : null;
        }

        public async Task Update(string token, DepartmentUpdateModel model)
        {
            int roleId = DecodeToken(token, "RoleId");
            if (!roleId.Equals(4)) throw new UnauthorizedAccessException("You do not have permission to access this resource"); // system admin

            Department department = await _departmentRepo.Get(model.Id);
            if (department == null) throw new NullReferenceException("Not found this element");
            
            if (model.Status.HasValue && model.Status.Value.Equals(true))
            {
                List<int> majorIds = await _majorRepo.GetIdsByDepartmentId(model.Id, model.Status.Value);
                if (majorIds != null)
                {
                    foreach (int majorId in majorIds)
                    {
                        Major major = await _majorRepo.Get(majorId);
                        major.Status = model.Status.Value;
                    }
                    await _majorRepo.Update();
                }

                department.Status = model.Status.Value;
            }

            if (!string.IsNullOrEmpty(model.Name)) department.Name = model.Name;

            await _departmentRepo.Update();
        }

        public async Task Delete(string token, int id)
        {
            int roleId = DecodeToken(token, "RoleId");
            if (!roleId.Equals(4)) throw new UnauthorizedAccessException("You do not have permission to access this resource"); // system admin
            
            Department department = await _departmentRepo.Get(id);
            if (department == null) throw new NullReferenceException("Not found this element");

            if (department.Status.Equals(false)) return; // already deleted 

            department.Status = false;
            // delete concerned major
            List<int> majorIds = await _majorRepo.GetIdsByDepartmentId(id, department.Status);
            if (majorIds != null)
            {
                foreach (int majorId in majorIds)
                {
                    Major major = await _majorRepo.Get(majorId);
                    major.Status = false;
                }
                await _majorRepo.Update();
            }

            await _departmentRepo.Update();
        }
    }
}
