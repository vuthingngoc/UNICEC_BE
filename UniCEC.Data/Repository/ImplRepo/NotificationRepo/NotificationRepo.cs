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

        public async Task<PagingResult<ViewNotification>> GetNotiesByUser(int userId, PagingRequest request)
        {
            var query = from noties in context.Notifications
                        where noties.UserId.Equals(userId)
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
                                                            }).ToListAsync();

            return (notifications.Count() > 0) ? new PagingResult<ViewNotification>(notifications, totalCount, request.CurrentPage, request.PageSize) : null;
        }
    }
}
