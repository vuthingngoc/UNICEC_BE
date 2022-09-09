using System;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Entities.Notification;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.NotificationRepo;
using Microsoft.Extensions.Configuration;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.Common;
using UniCEC.Business.Utilities;
using System.Collections.Generic;

namespace UniCEC.Business.Services.NotificationSvc
{
    public class NotificationService : INotificationService
    {
        private INotificationRepo _notificationRepo;
        private IConfiguration _configuration;
        private DecodeToken _decodeToken;

        public NotificationService(INotificationRepo notificationRepo, IConfiguration configuration)
        {
            _notificationRepo = notificationRepo;
            _configuration = configuration;
            _decodeToken = new DecodeToken();
        }

        public async Task<PagingResult<ViewNotification>> GetNotiesByUser(int userId, string token, PagingRequest request)
        {
            int id = _decodeToken.Decode(token, "Id");
            if (!userId.Equals(id)) throw new UnauthorizedAccessException("You do not have permission to access this resource");

            PagingResult<ViewNotification> notifications = await _notificationRepo.GetNotiesByUser(userId, request);
            if (notifications == null) throw new NullReferenceException();

            return notifications;
        }

        public async Task<ViewNotification> GetNotiById(int id, string token)
        {
            int userId = _decodeToken.Decode(token, "Id");
            ViewNotification notification =  await _notificationRepo.GetNotiById(id);
            if(notification == null) throw new NullReferenceException();

            if (!notification.UserId.Equals(userId)) throw new UnauthorizedAccessException("You do not have permission to access this resource");
            return notification;
        }

        public async Task SendNotification(Notification notification, string deviceToken)
        {
            var message = new FirebaseAdmin.Messaging.Message()
            {
                Token = deviceToken,
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = notification.Title,
                    Body = notification.Body,
                },
                Android = new FirebaseAdmin.Messaging.AndroidConfig()
                {
                    Priority = FirebaseAdmin.Messaging.Priority.High
                }
            };

            await FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance.SendAsync(message);

            // Save notification
            notification.CreateTime = new LocalTime().GetLocalTime().DateTime;
            await _notificationRepo.Insert(notification);
        }

        public async Task SendNotification(Notification notification, List<string> deviceTokens)
        {
            var message = new FirebaseAdmin.Messaging.MulticastMessage()
            {
                Tokens = deviceTokens,
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = notification.Title,
                    Body = notification.Body,
                },
                Android = new FirebaseAdmin.Messaging.AndroidConfig()
                {
                    Priority = FirebaseAdmin.Messaging.Priority.High
                }
            };

            await FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).ConfigureAwait(false);
        }
    }
}

