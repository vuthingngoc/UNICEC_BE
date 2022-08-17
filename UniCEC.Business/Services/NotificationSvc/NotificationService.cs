using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UniCEC.Data.Models.Notification;
using UniCEC.Data.ViewModels.Entities.Notification;
using CorePush.Google;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.NotificationRepo;
using CorePush.Apple;

namespace UniCEC.Business.Services.NotificationSvc
{
    public class NotificationService : INotificationService
    {
        private readonly FcmNotificationSettings _settings;
        private INotificationRepo _notificationRepo;

        public NotificationService(IOptions<FcmNotificationSettings> settings, INotificationRepo notificationRepo)
        {
            _settings = settings.Value;
            _notificationRepo = notificationRepo;
        }

        public async Task InsertDeviceUser(NotificationInsertModel deviceInfo)
        {
            if (deviceInfo.UserId.Equals(0) || string.IsNullOrEmpty(deviceInfo.DeviceId)) throw new Exception();

            int checkExistedId = await _notificationRepo.CheckExistedToken(deviceInfo.UserId);
            if (checkExistedId != 0)
            {
                Notification row = await _notificationRepo.Get(checkExistedId);
                row.DeviceId = deviceInfo.DeviceId;
                await _notificationRepo.Update();
            }

            Notification notification = new Notification()
            {
                DeviceId = deviceInfo.DeviceId,
                UserId = deviceInfo.UserId,
                IsAndroidDevice = deviceInfo.IsAndroidDevice,
            };

            await _notificationRepo.Insert(notification);
        }

        public async Task SendNotification(Notification notification, string title, string body)
        {
            ViewNotification viewNotification = new ViewNotification();
            try
            {
                if (notification.IsAndroidDevice)
                {
                    FcmSettings fcmSettings = new FcmSettings()
                    {
                        SenderId = _settings.SenderId,
                        ServerKey = _settings.ServerKey,
                    };
                    HttpClient httpClient = new HttpClient();

                    string authorizationKey = string.Format("keyy={0}", fcmSettings.ServerKey);
                    string deviceToken = notification.DeviceId;

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload payload = new DataPayload();
                    payload.Title = title;
                    payload.Body = body;

                    GoogleNotification googleNotification = new GoogleNotification();
                    googleNotification.Data = payload;
                    googleNotification.Notification = payload;

                    var fcm = new FcmSender(fcmSettings, httpClient);
                    var fcmSendResponse = await fcm.SendAsync(deviceToken, googleNotification);

                    if (fcmSendResponse.IsSuccess())
                    {
                        viewNotification.IsSuccess = true;
                        viewNotification.Message = "Notification sent successfully";
                    }
                    else
                    {
                        viewNotification.IsSuccess = false;
                        viewNotification.Message = fcmSendResponse.Results[0].Error;
                    }
                }
                else
                {
                    // code here for IOS device => future
                    // var apn = new ApnSender(apnSettings, httpClient);
                    // await apn.SendAsync(googleNotification, deviceToken);

                    // below code is not correct yet just copy code in android platform
                    ApnSettings apnSettings = new ApnSettings()
                    {
                        P8PrivateKeyId = _settings.SenderId,
                        P8PrivateKey = _settings.ServerKey,
                    };
                    HttpClient httpClient = new HttpClient();

                    string authorizationKey = string.Format("keyy={0}", apnSettings.P8PrivateKey);
                    string deviceToken = notification.DeviceId;

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload payload = new DataPayload();
                    payload.Title = title;
                    payload.Body = body;

                    GoogleNotification googleNotification = new GoogleNotification();
                    googleNotification.Data = payload;
                    googleNotification.Notification = payload;

                    // var apn = new ApnSender(apnSettings, httpClient);
                    // await apn.SendAsync(googleNotification, deviceToken);

                    var apn = new ApnSender(apnSettings, httpClient);
                    var apnSendResponse = await apn.SendAsync(googleNotification, deviceToken);

                    if (apnSendResponse.IsSuccess)
                    {
                        viewNotification.IsSuccess = true;
                        viewNotification.Message = "Notification sent successfully";
                    }
                    else
                    {
                        viewNotification.IsSuccess = false;
                        viewNotification.Message = apnSendResponse.Error.ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
    }
}

