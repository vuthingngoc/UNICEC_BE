using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Notification;

namespace UniCEC.Business.Services.NotificationSvc
{
    public interface INotificationService
    {
        public void SendNotification(Notification notification, string deviceToken);
        public Task<PagingResult<ViewNotification>> GetNotiesByUser(int userId, PagingRequest request);
    }
}
