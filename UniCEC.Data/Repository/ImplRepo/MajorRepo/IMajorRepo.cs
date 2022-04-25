using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.RequestModels;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.Repository.ImplRepo.MajorRepo
{
    public interface IMajorRepo : IRepository<Major>
    {
        public Task<PagingResult<Major>> GetByCondition(MajorRequestModel request);
        public Task<PagingResult<Major>> GetByUniversity(int universityId, PagingRequest request);
        public Task<List<int>> GetByDepartment(int departmentId);
        public Task<int> CheckExistedMajorCode(int departmentId, string code);
    }
}
