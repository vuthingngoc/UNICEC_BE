using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;
using UNICS.Data.ViewModels.Entities.GroupUniversity;

namespace UNICS.Business.Services.GroupUniversitySvc
{
    public interface IGroupUniversitySvc
    {
        Task<PagingResult<ViewGroupUniversity>> GetAll(PagingRequest request);
        Task<ViewGroupUniversity> GetById(int id);
        Task<bool> Insert(GroupUniversityInsertModel groupUniversity);
        Task<bool> Update(ViewGroupUniversity groupUniversity);
        Task<bool> Delete(int id);
    }
}
