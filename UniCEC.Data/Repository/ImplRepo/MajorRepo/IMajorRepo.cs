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
        public Task<Major> GetByMajorCode(string code);
    }
}
