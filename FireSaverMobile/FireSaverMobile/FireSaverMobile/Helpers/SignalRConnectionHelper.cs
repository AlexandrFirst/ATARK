using FireSaverMobile.Pages;
using FireSaverMobile.Popups.PopupNotification;
using FireSaverMobile.Services;
using KinderMobile.PopupYesNo;
using Microsoft.AspNetCore.SignalR.Client;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Helpers
{
    public class SignalRConnectionHelper : BaseHttpService
    {
        private static SignalRConnectionHelper _instance;
        public static SignalRConnectionHelper Instance
        {
            get
            {
                if (_instance == null)
                    throw new Exception("Socket connection is not inited");
                return _instance;
            }
        }



        public bool IsConnected
        {
            get; set;
        }

        public static void InitConnection()
        {
            _instance = new SignalRConnectionHelper($"http://{serverAddr}/socket");
        }

        HubConnection hubConnection;

        private SignalRConnectionHelper(string connectionUrl)
        {
            hubConnection = new HubConnectionBuilder()
               .WithUrl(connectionUrl, Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets, (con) =>
               {
                   con.SkipNegotiation = true;
                   con.AccessTokenProvider = async () => { var userData = await retrieveAuthValues(); return userData.Token; };
               })
               .Build();
            Task.Run(async () =>
            {
                await Connect();
            });
        }

        public async Task Connect()
        {
            if (IsConnected)
                return;
            try
            {
                await hubConnection.StartAsync();
                IsConnected = true;
            }
            catch (Exception ex)
            {
                await PopupNavigation.Instance.PushAsync(new PopupNotificationView("Unable to connect to server", MessageType.Warning));
            }

            hubConnection.Closed += async (error) =>
            {
                IsConnected = false;
                await Task.Delay(5000);
                await Connect();
            };

            hubConnection.On("Alarm", async () =>
            {
                await PopupNavigation.Instance.PushAsync(new PopupYesActionView(async () => {
                    await NavigationDispetcher.Instance.Navigation.PushModalAsync(new EvacuationPlanPage());
                }, "ALARM!!!", true));
            });
        }


    }
}
