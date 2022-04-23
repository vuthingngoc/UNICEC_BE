using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Common;

namespace UniCEC.Data.Repository.GenericRepo
{
    public interface IRepository<T> where T : class
    {
        Task<PagingResult<T>> GetAllPaging(PagingRequest request);
        Task<T> Get(int id);
        Task<int> Insert(T entity);
        // Update / Delete = Enum status equal 0
        Task Update();
    }
}
