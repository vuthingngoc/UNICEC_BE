﻿using System;
using System.Threading.Tasks;
using UniCEC.Data.Repository.ImplRepo.DepartmentRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Department;
using System.Collections.Generic;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.MajorRepo;
using UniCEC.Data.RequestModels;

namespace UniCEC.Business.Services.DepartmentSvc
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepo _departmentRepo;
        private IMajorRepo _majorRepo;

        public DepartmentService(IDepartmentRepo departmentRepo, IMajorRepo majorRepo)
        {
            _departmentRepo = departmentRepo;
            _majorRepo = majorRepo;
        }

        public async Task<ViewDepartment> GetById(int id)
        {
            ViewDepartment department = await _departmentRepo.GetById(id);
            return (department != null) ? department : throw new NullReferenceException("Not found this department");
        }

        public async Task<PagingResult<ViewDepartment>> GetByConditions(DepartmentRequestModel request)
        {
            PagingResult<ViewDepartment> departments = await _departmentRepo.GetByConditions(request);
            if (departments == null) throw new NullReferenceException("Not found any departments");
            return (departments != null) ? departments : throw new NullReferenceException();
        }

        public async Task<PagingResult<ViewDepartment>> GetByCompetition(int competitionId, PagingRequest request)
        {
            PagingResult<ViewDepartment> departments = await _departmentRepo.GetByCompetition(competitionId, request);
            return (departments != null) ? departments : throw new NullReferenceException();
        }

        public async Task<ViewDepartment> Insert(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Name Null");

            Department element = new Department()
            {
                Name = name,
                Status = true // default inserted status is true 
            };
            int id = await _departmentRepo.Insert(element);
            return (id > 0) ? await _departmentRepo.GetById(id) : null;
        }

        public async Task Update(DepartmentUpdateModel model)
        {
            Department department = await _departmentRepo.Get(model.Id);
            if (department == null) throw new NullReferenceException("Not found this element");
            if (model.Status.HasValue && model.Status.Value.Equals(true))
            {
                List<int> majorIds = await _majorRepo.GetByDepartment(model.Id);
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

        public async Task Delete(int id)
        {
            Department department = await _departmentRepo.Get(id);
            if (department == null) throw new NullReferenceException("Not found this element");

            if (department.Status.Equals(false)) return; // already deleted 

            department.Status = false;
            // delete concerned major
            List<int> majorIds = await _majorRepo.GetByDepartment(id);
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
