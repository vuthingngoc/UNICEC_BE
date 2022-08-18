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

namespace UniCEC.Business.Services.NotificationSvc
{
    public class NotificationService : INotificationService
    {
        private INotificationRepo _notificationRepo;
        private IConfiguration _configuration;

        public NotificationService(INotificationRepo notificationRepo, IConfiguration configuration)
        {
            _notificationRepo = notificationRepo;
            _configuration = configuration;
        }

        public async Task<PagingResult<ViewNotification>> GetNotiesByUser(int userId, PagingRequest request)
        {
            return await _notificationRepo.GetNotiesByUser(userId, request);            
        }

        private async Task SaveNotification(NotificationInsertModel notification)
        {

            //if (deviceInfo.UserId.Equals(0) || string.IsNullOrEmpty(deviceInfo.DeviceId)) throw new Exception();

            //int checkExistedId = await _notificationRepo.CheckExistedToken(deviceInfo.UserId);
            //if (checkExistedId != 0)
            //{
            //    Notification row = await _notificationRepo.Get(checkExistedId);
            //    row.DeviceId = deviceInfo.DeviceId;
            //    await _notificationRepo.Update();
            //    return;
            //}

            //Notification notification = new Notification()
            //{
            //    UserId = deviceInfo.UserId,

            //};

            //await _notificationRepo.Insert(notification);
        }

        public void SendNotification(Notification notification, string deviceToken)
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

                        icon = "myicon"

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

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

