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
using UniCEC.Data.ViewModels.Entities.Major;

namespace UniCEC.Business.Services.MajorSvc
{
    public class MajorService : IMajorService
    {
        private IMajorRepo _majorRepo;
        private IDepartmentRepo _departmentRepo;

        private DecodeToken _decodeToken;

        public MajorService(IMajorRepo majorRepo, IDepartmentRepo departmentRepo)
        {
            _majorRepo = majorRepo;
            _departmentRepo = departmentRepo;
            _decodeToken = new DecodeToken();
        }

        private ViewMajor TransformViewMajor(Major major)
        {
            return new ViewMajor()
            {
                Id = major.Id,
                DepartmentId = major.DepartmentId,
                Description = major.Description,
                MajorCode = major.MajorCode,
                Name = major.Name,
                Status = major.Status,
            };
        }

        public async Task<ViewMajor> GetById(string token, int id)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            bool? status = null;
            int? universityId = null;

            if (!roleId.Equals(1) && !roleId.Equals(4)) status = true;
            if (!roleId.Equals(4) && !roleId.Equals(2)) universityId = _decodeToken.Decode(token, "UniversityId"); // system admin and sponsor            

            ViewMajor major = await _majorRepo.GetById(id, status, universityId);
            if (major == null) throw new NullReferenceException();
            return major;
        }

        public async Task<ViewMajor> GetByCode(string token, string majorCode)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            bool? status = null;
            int? universityId = null;

            if (!roleId.Equals(1) && !roleId.Equals(4)) status = true;
            if (!roleId.Equals(2) && !roleId.Equals(4)) universityId = _decodeToken.Decode(token, "UniversityId");

            ViewMajor major = await _majorRepo.GetByCode(majorCode, status, universityId);
            if (major == null) throw new NullReferenceException();
            return major;
        }

        //public async Task<PagingResult<ViewMajor>> GetAllPaging(PagingRequest request)
        //{
        //    PagingResult<Major> result = await _majorRepo.GetAllPaging(request);
        //    if (result != null)
        //    {
        //        List<ViewMajor> majors = new List<ViewMajor>();
        //        result.Items.ForEach(element =>
        //        {
        //            ViewMajor viewMajor = TransformViewMajor(element);
        //            majors.Add(viewMajor);
        //        });

        //        return new PagingResult<ViewMajor>(majors, result.TotalCount, result.CurrentPage, result.PageSize);
        //    }

        //    throw new NullReferenceException("Not found");
        //}

        //public async Task<PagingResult<ViewMajor>> GetByUniversity(int universityId, PagingRequest request)
        //{
        //    PagingResult<Major> majors = await _majorRepo.GetByUniversity(universityId, request);
        //    if (majors == null) throw new NullReferenceException("No any majors with this university");

        //    List<ViewMajor> viewMajors = new List<ViewMajor>();
        //    majors.Items.ForEach(element =>
        //    {
        //        ViewMajor viewMajor = TransformViewMajor(element);
        //        viewMajors.Add(viewMajor);
        //    });
        //    return new PagingResult<ViewMajor>(viewMajors, majors.TotalCount, majors.CurrentPage, majors.PageSize);
        //}

        public async Task<PagingResult<ViewMajor>> GetMajorByConditions(string token, MajorRequestModel request)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            
            if (!roleId.Equals(1) && !roleId.Equals(4)) request.Status = true;            
            if(!roleId.Equals(4) && !roleId.Equals(2))
            {
                int universityId = _decodeToken.Decode(token, "UniversityId");
                if (!universityId.Equals(request.UniversityId)) throw new NullReferenceException();
            }

            PagingResult<ViewMajor> majors = await _majorRepo.GetByConditions(request);
            if(majors == null) throw new NullReferenceException();
            return majors;
        }

        private void checkAuthorizedUser(string token, int uniId)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (!roleId.Equals(1)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            int universityId = _decodeToken.Decode(token, "UniversityId");
            if (!uniId.Equals(universityId)) throw new UnauthorizedAccessException("You do not have permission to access this resource");
        }

        public async Task<ViewMajor> Insert(string token, MajorInsertModel model)
        {
            if (model.DepartmentId.Equals(0) || model.UniversityId.Equals(0) || string.IsNullOrEmpty(model.MajorCode) ||
                string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description))
                throw new ArgumentNullException("DepartmentId null || UniversityId null || MajorCode null || Name null || Description null");

            checkAuthorizedUser(token, model.UniversityId);
            //int roleId = _decodeToken.Decode(token, "RoleId");
            //if (!roleId.Equals(1)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            //int universityId = _decodeToken.Decode(token, "UniversityId");
            //if (!major.UniversityId.Equals(universityId)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            model.Name = Regex.Replace(model.Name.Trim(), @"\s{2,}", " ");
            int duplicatedId = await _majorRepo.CheckDuplicatedName(model.UniversityId, model.Name);
            if (duplicatedId > 0) throw new ArgumentException("Duplicated major name");

            int majorCode = await _majorRepo.CheckExistedMajorCode(model.UniversityId, model.MajorCode);
            if (majorCode > 0) throw new ArgumentException("Duplicated MajorCode");
            Department department = await _departmentRepo.Get(model.DepartmentId);
            if (department == null) throw new ArgumentException("Can not find this department");

            // default status when insert is true
            bool status = true;
            Major major = new Major()
            {
                UniversityId = model.UniversityId,
                DepartmentId = model.DepartmentId,
                Description = model.Description,
                MajorCode = model.MajorCode,
                Name = model.Name,
                Status = status
            };
            int id = await _majorRepo.Insert(major);
            ViewMajor viewMajor = await _majorRepo.GetById(id, status, model.UniversityId);
            return viewMajor;
        }

        public async Task Update(string token, ViewMajor model)
        {
            checkAuthorizedUser(token, model.UniversityId);
            //int roleId = _decodeToken.Decode(token, "RoleId");
            //if (!roleId.Equals(1)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            //int universityId = _decodeToken.Decode(token, "UniversityId");
            //if (!major.UniversityId.Equals(universityId)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            Major major = await _majorRepo.Get(model.Id);
            if (major == null) throw new NullReferenceException("Not found this element");

            int majorCode = await _majorRepo.CheckExistedMajorCode(model.UniversityId, model.MajorCode);
            if (majorCode > 0 && majorCode != model.Id) throw new ArgumentException("Duplicated MajorCode");

            Department department = await _departmentRepo.Get(model.DepartmentId);
            if (department == null) throw new ArgumentException("Can not find this department");

            if (model.DepartmentId != 0) major.DepartmentId = model.DepartmentId;
            
            if (!string.IsNullOrEmpty(model.Description)) major.Description = model.Description;
            
            if (!string.IsNullOrEmpty(model.MajorCode)) major.MajorCode = model.MajorCode;
            
            if (!string.IsNullOrEmpty(model.Name)) major.Name = model.Name;
            
            if(model.Status.Equals(true)) major.Status = model.Status;

            await _majorRepo.Update();
        }

        public async Task Delete(string token, int id)
        {
            Major major = await _majorRepo.Get(id);
            if (major == null) throw new NullReferenceException($"Not found this id: {id}");

            checkAuthorizedUser(token, major.UniversityId);

            if (major.Status.Equals(false)) return;
            major.Status = false;
            await _majorRepo.Update();
        }
    }
}
