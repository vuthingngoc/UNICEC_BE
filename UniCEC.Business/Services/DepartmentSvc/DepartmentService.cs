using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UniCEC.Business.Utilities;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.DepartmentRepo;
using UniCEC.Data.Repository.ImplRepo.MajorRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Department;

namespace UniCEC.Business.Services.DepartmentSvc
{
    public class DepartmentService : IDepartmentService
    {
        private IMajorRepo _majorRepo;
        private IDepartmentRepo _departmentRepo;

        private DecodeToken _decodeToken;

        public DepartmentService(IMajorRepo majorRepo, IDepartmentRepo departmentRepo)
        {
            _majorRepo = majorRepo;
            _departmentRepo = departmentRepo;
            _decodeToken = new DecodeToken();
        }

        private ViewDepartment TransformViewDepartment(Department department)
        {
            return new ViewDepartment()
            {
                Id = department.Id,
                MajorId = department.MajorId,
                Description = department.Description,
                DepartmentCode = department.DepartmentCode,
                Name = department.Name,
                Status = department.Status,
                UniversityId = department.UniversityId
            };
        }

        public async Task<ViewDepartment> GetById(string token, int id)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            bool? status = null;
            int? universityId = null;

            if (!roleId.Equals(1) && !roleId.Equals(4)) status = true;
            if (!roleId.Equals(4) && !roleId.Equals(2)) universityId = _decodeToken.Decode(token, "UniversityId"); // system admin and sponsor            

            ViewDepartment department = await _departmentRepo.GetById(id, status, universityId);
            if (department == null) throw new NullReferenceException();
            return department;
        }

        public async Task<ViewDepartment> GetByCode(string token, string departmentCode)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            bool? status = null;
            int? universityId = null;

            if (!roleId.Equals(1) && !roleId.Equals(4)) status = true;
            if (!roleId.Equals(2) && !roleId.Equals(4)) universityId = _decodeToken.Decode(token, "UniversityId");

            ViewDepartment department = await _departmentRepo.GetByCode(departmentCode, status, universityId);
            if (department == null) throw new NullReferenceException();
            return department;
        }

        public async Task<PagingResult<ViewDepartment>> GetByConditions(string token, DepartmentRequestModel request)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");

            if (!roleId.Equals(1) && !roleId.Equals(4)) request.Status = true;
            //if(!roleId.Equals(4) && !roleId.Equals(2))
            //{
            //    //int universityId = _decodeToken.Decode(token, "UniversityId");
            //    //if (!universityId.Equals(request.UniversityId)) throw new NullReferenceException();
            //}

            PagingResult<ViewDepartment> departments = await _departmentRepo.GetByConditions(request);
            if (departments == null) throw new NullReferenceException();
            return departments;
        }

        private void checkAuthorizedUser(string token, int uniId)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            int universityId = _decodeToken.Decode(token, "UniversityId");

            if (!roleId.Equals(1) || !uniId.Equals(universityId))
                throw new UnauthorizedAccessException("You do not have permission to access this resource");
        }

        public async Task<ViewDepartment> Insert(string token, DepartmentInsertModel model)
        {
            if (model.MajorId.Equals(0) || model.UniversityId.Equals(0) || string.IsNullOrEmpty(model.DepartmentCode) ||
                string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description))
                throw new ArgumentNullException("DepartmentId null || UniversityId null || DepartmentCode null || Name null || Description null");

            checkAuthorizedUser(token, model.UniversityId);

            model.Name = Regex.Replace(model.Name.Trim(), @"\s{2,}", " ");
            int duplicatedId = await _departmentRepo.CheckDuplicatedName(model.UniversityId, model.Name);
            if (duplicatedId > 0) throw new ArgumentException("Duplicated department name");

            int departmentCode = await _departmentRepo.CheckExistedDepartmentCode(model.UniversityId, model.DepartmentCode);
            if (departmentCode > 0) throw new ArgumentException("Duplicated DepartmentCode");
            Major major = await _majorRepo.Get(model.MajorId);
            if (major == null) throw new ArgumentException("Can not find this major");

            // default status when insert is true
            bool status = true;
            Department department = new Department()
            {
                UniversityId = model.UniversityId,
                MajorId = model.MajorId,
                Description = model.Description,
                DepartmentCode = model.DepartmentCode,
                Name = model.Name,
                Status = status
            };
            int id = await _departmentRepo.Insert(department);
            department.Id = id;
            return TransformViewDepartment(department);
        }

        public async Task Update(string token, DepartmentUpdateModel model)
        {
            checkAuthorizedUser(token, model.UniversityId);

            Department department = await _departmentRepo.Get(model.Id);
            if (department == null) throw new NullReferenceException("Not found this department");

            int departmentCode = await _departmentRepo.CheckExistedDepartmentCode(model.UniversityId, model.DepartmentCode);
            if (departmentCode > 0 && departmentCode != model.Id) throw new ArgumentException("Duplicated DepartmentCode");

            Major major = await _majorRepo.Get(model.MajorId);
            if (major == null) throw new ArgumentException("Can not find this major");

            if (model.MajorId != 0) department.MajorId = model.MajorId;

            if (!string.IsNullOrEmpty(model.Description)) department.Description = model.Description;

            if (!string.IsNullOrEmpty(model.DepartmentCode)) department.DepartmentCode = model.DepartmentCode;

            if (!string.IsNullOrEmpty(model.Name)) department.Name = model.Name;

            if (model.Status.Equals(true)) department.Status = model.Status;

            await _majorRepo.Update();
        }

        public async Task Delete(string token, int id)
        {
            Department department = await _departmentRepo.Get(id);
            if (department == null) throw new NullReferenceException("Not found this department");

            checkAuthorizedUser(token, department.UniversityId);

            if (department.Status.Equals(false)) return;
            department.Status = false;
            await _departmentRepo.Update();
        }

        //TA
        public async Task<List<ViewDepartment>> GetAllByUniversity(int universityId,string token)
        {
            try
            {
                //int uni = _decodeToken.Decode(token, "UniversityId");
                List<ViewDepartment> result = await _departmentRepo.GetAllByUniversity(universityId);
                if (result == null) throw new NullReferenceException();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
