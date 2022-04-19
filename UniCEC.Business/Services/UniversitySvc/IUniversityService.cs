﻿using System.Threading.Tasks;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.University;

namespace UniCEC.Business.Services.UniversitySvc
{
    public interface IUniversityService
    {
        public Task<PagingResult<ViewUniversity>> GetUniversitiesByConditions(UniversityRequestModel request );
        public Task<ViewUniversity> GetUniversityById(int id);
        public Task<ViewUniversity> Insert(UniversityInsertModel university);
        public Task<bool> Update(ViewUniversity university);
        public Task<bool> Delete(int id);

        //Check Email Of University
        public Task<bool> CheckEmailUniversity(string email);
    }
}
