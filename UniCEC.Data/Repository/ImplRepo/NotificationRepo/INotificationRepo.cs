using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Notification;

namespace UniCEC.Data.Repository.ImplRepo.NotificationRepo
{
    public interface INotificationRepo : IRepository<Notification>
    {
        public Task<PagingResult<ViewNotification>> GetNotiesByUser(int userId, PagingRequest request);
        public Task<ViewNotification> GetNotiById(int id);
        public Task<int> CheckExistedToken(int userId);
        public Task InsertNotifications(List<Notification> notifications);
    }
}
