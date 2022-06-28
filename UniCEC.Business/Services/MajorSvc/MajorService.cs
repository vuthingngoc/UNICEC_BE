using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.DepartmentRepo;
using UniCEC.Data.ViewModels.Common;
using System.Collections.Generic;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.MajorRepo;
using UniCEC.Data.RequestModels;

using System.Text.RegularExpressions;
using UniCEC.Business.Utilities;
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
            _departmentRepo = departmentRepo;
            _majorRepo = majorRepo;
            _decodeToken = new DecodeToken();
        }

        public async Task<ViewMajor> GetById(string token, int id)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            bool? status = null;
            if (!roleId.Equals(4)) status = true;
            ViewMajor major = await _majorRepo.GetById(id, status);
            return (major != null) ? major : throw new NullReferenceException("Not found this major");
        }

        public async Task<PagingResult<ViewMajor>> GetByConditions(string token, MajorRequestModel request) // not finish
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (!roleId.Equals(4)) request.Status = true;
            PagingResult<ViewMajor> majors = await _majorRepo.GetByConditions(request);
            if (majors == null) throw new NullReferenceException("Not found any majors");
            return (majors != null) ? majors : throw new NullReferenceException();
        }

        public async Task<PagingResult<ViewMajor>> GetByCompetition(int competitionId, PagingRequest request)
        {
            PagingResult<ViewMajor> majors = await _majorRepo.GetByCompetition(competitionId, request);
            return (majors != null) ? majors : throw new NullReferenceException();
        }

        private ViewMajor TransferToViewMajor(Major major)
        {
            return new ViewMajor()
            {
                Id = major.Id,
                Name = major.Name,
                Status = major.Status                
            };
        }

        private void CheckValidAuthorized(string token)
        {
            int roleId = _decodeToken.Decode(token, "RoleId");
            if (!roleId.Equals(4)) throw new UnauthorizedAccessException("You do not have permission to access this resource"); // system admin
        }

        public async Task<ViewMajor> Insert(string token, string name)
        {
            name = Regex.Replace(name.Trim(), @"\s{2,}", " ");
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Name Null");

            CheckValidAuthorized(token);

            int duplicatedId = await _majorRepo.CheckDuplicatedName(name);
            if (duplicatedId > 0) throw new ArgumentException("Duplicated major");

            Major major = new Major()
            {
                Name = name,
                Status = true // default inserted status is true 
            };
            int id = await _majorRepo.Insert(major);
            if(id > 0)
            {
                major.Id = id;
                return TransferToViewMajor(major);
            }

            return null;
        }

        public async Task Update(string token, MajorUpdateModel model)
        {
            CheckValidAuthorized(token);

            Major major = await _majorRepo.Get(model.Id);
            if (major == null) throw new NullReferenceException("Not found this major");
            
            if (model.Status.HasValue && model.Status.Value.Equals(true))
            {
                List<int> departmentIds = await _departmentRepo.GetIdsByMajorId(model.Id, model.Status.Value);
                if (departmentIds != null)
                {
                    foreach (int departmentId in departmentIds)
                    {
                        Department department = await _departmentRepo.Get(departmentId);
                        department.Status = model.Status.Value;
                    }
                    await _departmentRepo.Update();
                }

                major.Status = model.Status.Value;
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                model.Name = Regex.Replace(model.Name.Trim(), @"\s{2,}", " ");
                int duplicatedId = await _majorRepo.CheckDuplicatedName(model.Name);
                if (duplicatedId > 0 && !duplicatedId.Equals(model.Id)) throw new ArgumentException("Duplicated major");
                major.Name = model.Name;
            }

            await _majorRepo.Update();
        }

        public async Task Delete(string token, int id)
        {
            CheckValidAuthorized(token);

            Major major = await _majorRepo.Get(id);
            if (major == null) throw new NullReferenceException("Not found this major");

            if (major.Status.Equals(false)) return; // already deleted 

            major.Status = false;
            // delete concerned major
            List<int> departmentIds = await _departmentRepo.GetIdsByMajorId(id, major.Status);
            if (departmentIds != null)
            {
                foreach (int departmentId in departmentIds)
                {
                    Department department = await _departmentRepo.Get(departmentId);
                    department.Status = false;
                }
                await _departmentRepo.Update();
            }

            await _majorRepo.Update();
        }
    }
}
