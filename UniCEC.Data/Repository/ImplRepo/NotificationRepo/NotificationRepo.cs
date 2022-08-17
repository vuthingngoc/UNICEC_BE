using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;

namespace UniCEC.Data.Repository.ImplRepo.NotificationRepo
{
    public class NotificationRepo : Repository<Notification>, INotificationRepo
    {
        public NotificationRepo(UniCECContext context) : base(context)
        {
        }

        public async Task<int> CheckExistedToken(int userId)
        {
            Notification notification = await context.Notifications.FirstOrDefaultAsync(noti => noti.UserId.Equals(userId));
            return (notification != null) ? notification.Id : 0;
        }

        public async Task<string> GetDeviceIdByUser(int userId)
        {
            Notification notification =  await context.Notifications.FirstOrDefaultAsync(noti => noti.UserId.Equals(userId));
            return (notification != null) ? notification.DeviceId : null;            
        }
    }
}
