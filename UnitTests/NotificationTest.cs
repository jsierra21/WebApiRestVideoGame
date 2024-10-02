using Core.DTOs;
using Core.Options;
using Core.Services;
using CorePush.Firebase;
using Microsoft.Extensions.Options;

namespace UnitTests
{
    public class NotificationTest
    {
        [Fact]
        public async void SendNotificationAsync()
        {
            // Arrrange
            FirebaseConfig fcmNotificationSetting = new()
            {
                SenderId = "644778068650",
                ServerKey = "AAAAlh_Eiqo:APA91bE8yApYxRerGBi8ikzC0_nvDTvsAiab42ope8Fr6MyOy1BJQWGYeNqZgpFcOg7chYr2KYyn9BKWV_M5GiAOr6nQbruQ55naWWv6aL-NnFEAGoiVFI644NBgSNlxPqoZyRg4HeSW",
                ServiceAccountFilePath = "../../../../Api/op360-29fb1-firebase.json"
            };

            IOptions<FirebaseConfig> iOptions = Options.Create(fcmNotificationSetting);

            string jsonContent = File.ReadAllText("../../../../Api/op360-29fb1-firebase.json");
            FirebaseSender fb = new(jsonContent, new HttpClient());

            //FirebaseService notificationService = new(iOptions);
            FirebaseService notificationService = new(fb);

            FirebaseDTO notification = new()
            {
                Cuerpo = "Test",
                Estado = "1",
                Id_dispositivo = "cyYyIEScQSmSwcsP1LaJb3:APA91bFtAO9ztm72EbkBD0dGWCnqlPRTbd3A5Q5OoaXYWtCcCH3xilQ2bFOpZb8e2YkjlgnjgPeiFIhlFt7LHUDt-DUaT_Wv7Vol6Jdh2i-viPY3KieOXAEO0wTUXZV2ndPkdpGhSCoD",
                Id_solicitud = 1,
                Ind_android = true,
                Notification = "Hola este es un test",
                Tipo = "test",
                Titulo = "PRUEBA NOTIFICATION"
            };

            // Act
            ResponseDTO result = await notificationService.SendNotification(notification);

            // Assert
            Assert.Equal(200, result.Estado);
        }
    }
}
