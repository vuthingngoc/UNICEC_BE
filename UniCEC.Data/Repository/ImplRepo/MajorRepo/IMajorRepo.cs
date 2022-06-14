using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Major;

namespace UniCEC.Data.Repository.ImplRepo.MajorRepo
{
    public interface IMajorRepo : IRepository<Major>
    {
        public Task<ViewMajor> GetById(int id, bool? status, int? universityId);
        public Task<ViewMajor> GetByCode(string majorCode, bool? status, int? universityId);
        public Task<PagingResult<ViewMajor>> GetByConditions(MajorRequestModel request);
        public Task<PagingResult<Major>> GetByUniversity(int universityId, PagingRequest request);
        public Task<List<int>> GetIdsByDepartmentId(int departmentId, bool? status);
        public Task<int> CheckExistedMajorCode(int departmentId, string code);
    }
}
