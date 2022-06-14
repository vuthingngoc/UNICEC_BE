using System;
using System.Collections.Generic;
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

        public async Task<ViewMajor> Insert(string token, MajorInsertModel major)
        {
            if (major.DepartmentId == 0 || string.IsNullOrEmpty(major.MajorCode) ||
                string.IsNullOrEmpty(major.Name) || string.IsNullOrEmpty(major.Description))
                throw new ArgumentNullException("DepartmentId null || MajorCode null || Name null || Description null");

            int majorId = await _majorRepo.CheckExistedMajorCode(major.DepartmentId, major.MajorCode);
            if (majorId > 0) throw new ArgumentException("Duplicated MajorCode");
            Department department = await _departmentRepo.Get(major.DepartmentId);
            if (department == null) throw new ArgumentException("Can not find this department");

            // default status when insert is true
            bool status = true;
            Major element = new Major()
            {
                DepartmentId = major.DepartmentId,
                Description = major.Description,
                MajorCode = major.MajorCode,
                Name = major.Name,
                Status = status
            };
            int id = await _majorRepo.Insert(element);
            if (id > 0)
            {
                element.Id = id;
                return TransformViewMajor(element);
            }
            return null;
        }

        public async Task Update(string token, ViewMajor major)
        {
            Major element = await _majorRepo.Get(major.Id);
            if (element == null) throw new NullReferenceException("Not found this element");

            int majorId = await _majorRepo.CheckExistedMajorCode(major.DepartmentId, major.MajorCode);
            if (majorId > 0 && majorId != major.Id) throw new ArgumentException("Duplicated MajorCode");

            Department department = await _departmentRepo.Get(major.DepartmentId);
            if (department == null) throw new ArgumentException("Can not find this department");

            if (major.DepartmentId != 0) element.DepartmentId = major.DepartmentId;
            if (!string.IsNullOrEmpty(major.Description)) element.Description = major.Description;
            if (!string.IsNullOrEmpty(major.MajorCode)) element.MajorCode = major.MajorCode;
            if (!string.IsNullOrEmpty(major.Name)) element.Name = major.Name;
            element.Status = major.Status;

            await _majorRepo.Update();
        }

        public async Task Delete(string token, int id)
        {
            Major major = await _majorRepo.Get(id);
            if (major == null) throw new NullReferenceException($"Not found this id: {id}");
            major.Status = false;
            await _majorRepo.Update();
        }
    }
}
