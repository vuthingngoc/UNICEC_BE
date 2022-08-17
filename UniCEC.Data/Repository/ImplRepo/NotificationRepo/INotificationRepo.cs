using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;

namespace UniCEC.Data.Repository.ImplRepo.NotificationRepo
{
    public interface INotificationRepo : IRepository<Notification>
    {
        public Task<string> GetDeviceIdByUser(int userId);
        public Task<int> CheckExistedToken(int userId);
    }
}
