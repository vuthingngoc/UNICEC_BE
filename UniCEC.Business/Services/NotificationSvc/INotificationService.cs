using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.ViewModels.Entities.Notification;

namespace UniCEC.Business.Services.NotificationSvc
{
    public interface INotificationService
    {
        public Task SendNotification(Notification notification, string title, string body);
        public Task InsertDeviceUser(NotificationInsertModel deviceInfo);
    }
}
