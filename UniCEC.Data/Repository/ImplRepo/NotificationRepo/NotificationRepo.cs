using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using UniCEC.Data.ViewModels.Common;
using System.Collections.Generic;
using System.Linq;
using UniCEC.Data.ViewModels.Entities.Notification;

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

        public async Task<ViewNotification> GetNotiById(int id)
        {
            var query = from noti in context.Notifications
                        where noti.Id.Equals(id)
                        select noti;

            return (query.Any()) ? await query.Select(noti => new ViewNotification()
            {
                Id = noti.Id,
                Body = noti.Body,
                UserId = noti.UserId,
                Title = noti.Title,
                CreateTime = noti.CreateTime,
                RedirectUrl = noti.RedirectUrl
            }).FirstOrDefaultAsync()
            : null;
        }

        public async Task<PagingResult<ViewNotification>> GetNotiesByUser(int userId, PagingRequest request)
        {
            var query = from noties in context.Notifications
                        where noties.UserId.Equals(userId)
                        orderby noties.CreateTime descending
                        select noties;

            int totalCount = query.Count();

            List<ViewNotification> notifications = await query.Skip((request.CurrentPage - 1) * request.PageSize).Take(request.PageSize)
                                                            .Select(noti => new ViewNotification()
                                                            {
                                                                Id = noti.Id,
                                                                UserId = userId,
                                                                Title = noti.Title,
                                                                Body = noti.Body,
                                                                RedirectUrl = noti.RedirectUrl,
                                                                CreateTime = noti.CreateTime
                                                            }).ToListAsync();

            return (notifications.Count() > 0) ? new PagingResult<ViewNotification>(notifications, totalCount, request.CurrentPage, request.PageSize) : null;
        }

        public async Task InsertNotifications(List<Notification> notifications)
        {
            await context.Notifications.AddRangeAsync(notifications);
            await context.SaveChangesAsync();
        }
    }
}
