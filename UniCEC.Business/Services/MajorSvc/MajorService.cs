using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public MajorService(IMajorRepo majorRepo, IDepartmentRepo departmentRepo)
        {
            _majorRepo = majorRepo;
            _departmentRepo = departmentRepo;
        }

        public async Task<PagingResult<ViewMajor>> GetAllPaging(PagingRequest request)
        {
            PagingResult<Major> result = await _majorRepo.GetAllPaging(request);
            if (result.Items != null)
            {
                List<ViewMajor> majors = new List<ViewMajor>();
                result.Items.ForEach(e =>
                {
                    ViewMajor viewMajor = new ViewMajor()
                    {
                        Id = e.Id,
                        DepartmentId = e.DepartmentId,
                        Description = e.Description,
                        MajorCode = e.MajorCode,
                        Name = e.Name,
                        Status = e.Status
                    };
                    majors.Add(viewMajor);
                });

                return new PagingResult<ViewMajor>(majors, result.TotalCount, result.CurrentPage, result.PageSize);
            }

            throw new NullReferenceException("Not found");
        }

        public async Task<List<ViewMajor>> GetByUniversity(int universityId)
        {
            List<Major> majorList = await _majorRepo.GetByUniversity(universityId);
            if (majorList == null) throw new NullReferenceException("No any majors with this university");

            List<ViewMajor> viewMajors = new List<ViewMajor>();
            majorList.ForEach(m =>
            {
                ViewMajor viewMajor = new ViewMajor()
                {
                    Id = m.Id,
                    DepartmentId = m.DepartmentId,
                    Description = m.Description,
                    MajorCode = m.MajorCode,
                    Name = m.Name,
                    Status = m.Status,
                };
                viewMajors.Add(viewMajor);
            });
            return viewMajors;
        }

        public async Task<PagingResult<ViewMajor>> GetMajorByCondition(MajorRequestModel request)
        {
            PagingResult<Major> majors = await _majorRepo.GetByCondition(request);
            if (majors.Items != null)
            {
                List<ViewMajor> items = new List<ViewMajor>();
                majors.Items.ForEach(x =>
                {
                    ViewMajor viewMajor = new ViewMajor()
                    {
                        Id = x.Id,
                        DepartmentId = x.DepartmentId,
                        Description = x.Description,
                        MajorCode = x.MajorCode,
                        Name = x.Name,
                        Status = x.Status
                    };
                    items.Add(viewMajor);
                });

                return new PagingResult<ViewMajor>(items, majors.TotalCount, majors.CurrentPage, majors.PageSize);
            }

            throw new NullReferenceException("Not found any majors satisfying the condition");
        }

        public async Task<ViewMajor> Insert(MajorInsertModel major)
        {
            if (major == null) throw new ArgumentNullException("Null Argument");

            Major majorObject = await _majorRepo.CheckExistedMajorCode(major.DepartmentId, major.MajorCode);
            if (majorObject != null) throw new ArgumentException("Duplicated MajorCode");
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
                return new ViewMajor()
                {
                    Id = id,
                    DepartmentId = major.DepartmentId,
                    Description = major.Description,
                    MajorCode = major.MajorCode,
                    Name = major.Name,
                    Status = status
                };
            }
            return null;
        }

        public async Task<bool> Update(ViewMajor major)
        {
            if (major == null) throw new ArgumentNullException("Null Argument");

            Major element = await _majorRepo.Get(major.Id);
            if (element == null) throw new NullReferenceException("Not found this element");

            Major majorObject = await _majorRepo.CheckExistedMajorCode(major.DepartmentId, major.MajorCode);
            if(majorObject != null)
            {
                if (majorObject.Id != major.Id) throw new ArgumentException("Duplicated MajorCode");                
            }

            Department department = await _departmentRepo.Get(major.DepartmentId);
            if (department == null) throw new ArgumentException("Can not find this department");

            element.DepartmentId = major.DepartmentId;
            element.Description = major.Description;
            element.MajorCode = major.MajorCode;
            element.Name = major.Name;
            element.Status = major.Status;

            return await _majorRepo.Update();
        }

        public async Task<bool> Delete(int id)
        {
            Major major = await _majorRepo.Get(id);
            if (major != null)
            {
                major.Status = false;
                return await _majorRepo.Update();
            }

            throw new NullReferenceException($"Not found this id: {id}");
        }
    }
}
