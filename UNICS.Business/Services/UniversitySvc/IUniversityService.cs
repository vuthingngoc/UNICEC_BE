using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.University;

namespace UNICS.Business.Services.UniversitySvc
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
