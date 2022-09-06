using System;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Entities.Notification;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.NotificationRepo;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.IO;
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
            var appSettingsSection = _configuration.GetSection("FcmNotification");
            try
            {
                var applicationID = appSettingsSection.GetSection("ServerKey").Value; 

                var senderId = appSettingsSection.GetSection("SenderId").Value;

                WebRequest tRequest = WebRequest.Create(_configuration.GetSection("FcmNotification").GetSection("GoogleApi").Value);

                tRequest.Method = "post";

                tRequest.ContentType = "application/json";

                var data = new

                {

                    to = deviceToken,

                    notification = new

                    {

                        body = notification.Body,

                        title = notification.Title,

                        icon = "myicon",

                        click_action = notification.RedirectUrl

                    }
                };

                var json = JsonConvert.SerializeObject(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));

                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

                tRequest.ContentLength = byteArray.Length;


                using (Stream dataStream = tRequest.GetRequestStream())
                {

                    dataStream.Write(byteArray, 0, byteArray.Length);


                    using (WebResponse tResponse = tRequest.GetResponse())
                    {

                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {

                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {

                                String sResponseFromServer = tReader.ReadToEnd();

                                string str = sResponseFromServer;

                            }
                        }
                    }
                }

                // Save notification
                notification.CreateTime = new LocalTime().GetLocalTime().DateTime;
                await _notificationRepo.Insert(notification);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

