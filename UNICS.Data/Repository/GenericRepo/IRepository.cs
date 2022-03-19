using System.Threading.Tasks;
using UNICS.Data.ViewModels.Common;

namespace UNICS.Data.Repository.GenericRepo
{
    public interface IRepository<T> where T : class
    {
        // Get All
        Task<PagingResult<T>> GetAll(PagingRequest request);
        // Get By Id
        Task<T> Get(string id);
        // Insert
        Task<bool> Insert(T entity);
        // Update / Delete = Enum status equal 0
        Task<bool> Update(T entity);
    }
}
