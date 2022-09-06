using System.Collections.Generic;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Notification;

namespace UniCEC.Business.Services.NotificationSvc
{
    public interface INotificationService
    {
        public Task SendNotification(Notification notification, string deviceToken);
        public Task SendNotification(Notification notification, List<string> deviceTokens);
        public Task<PagingResult<ViewNotification>> GetNotiesByUser(int userId, string token, PagingRequest request);
        public Task<ViewNotification> GetNotiById(int id, string token);
    }
}
