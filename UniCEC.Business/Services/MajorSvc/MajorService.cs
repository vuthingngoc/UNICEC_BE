﻿using System;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.MajorRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Major;

namespace UniCEC.Business.Services.MajorSvc
{
    public class MajorService : IMajorService
    {
        private IMajorRepo _majorRepo;

        public MajorService(IMajorRepo majorRepo)
        {
            _majorRepo = majorRepo;
        }

        public async Task<bool> Delete(int id)
        {
            Major major = await _majorRepo.Get(id);
            if(major != null)
            {
                major.Status = false;
                return await _majorRepo.Update();
            }

            throw new NullReferenceException($"Not found this id: {id}");
        }

        public Task<PagingResult<ViewMajor>> GetAllPaging(PagingRequest request)
        {

            throw new NotImplementedException();
        }

        public Task<ViewMajor> GetByMajorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ViewMajor> Insert(MajorInsertModel major)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ViewMajor major)
        {
            throw new NotImplementedException();
        }
    }
}
