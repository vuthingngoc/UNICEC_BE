using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.University;

namespace UniCEC.Business.Services.UniversitySvc
{
    public interface IUniversityService
    {
        public Task<PagingResult<ViewUniversity>> GetAll(PagingRequest request);
        public Task<ViewUniversity> GetUniversityById(string id);
        public Task<ViewUniversity> Insert(UniversityInsertModel university);
        public Task<bool> Update(ViewUniversity university);
        public Task<bool> Delete(int id);
    }
}
