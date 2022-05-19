using FireSaverMobile.Contracts;
using FireSaverMobile.DI;
using FireSaverMobile.Helpers;
using FireSaverMobile.Models;
using FireSaverMobile.Models.PointModels;
using FireSaverMobile.Models.QRModel;
using FireSaverMobile.Pages;
using FireSaverMobile.Popups.PopupNotification;
using FireSaverMobile.Popups.QRCodePopUp;
using FireSaverMobile.Popups.qrScannerProxy;
using FireSaverMobile.Services;
using KinderMobile.PopupYesNo;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FireSaverMobile.ViewModels
{
    public class EvacuationPlanViewModel : BaseViewModel
    {
        private IUserService userService;
        private ILoginService loginService;
        private IEvacuationService evacuationService;

        private CompartmentCommonInfo currentCompartment;

        private int evacuationPlanIndex = 0;

        public List<EvacuationPlanDto> EvacuationsPlans { get; internal set; } = new List<EvacuationPlanDto>();


        public Action<EvacuationPlanDto, RoutePointsDto> OnEvacuationPlanInit;
        public Action<Position> OnUserPositonChange;

        private Position currentUserPostion;
        public Position CurrentUserPostion
        {
            get { return currentUserPostion; }
            set
            {
                currentUserPostion = value;
                OnUserPositonChange(currentUserPostion);
            }
        }

        private bool isBusy = false;
        private int userId;

        public bool IsBusy { get { return isBusy; } set { SetValue(ref isBusy, value); } }

        public ICommand SyncPosBtnClick { get; set; }
        public ICommand FireBtnClick { get; set; }
        public ICommand HelpBtnClick { get; set; }
        public ICommand NextCompartment { get; set; }
        public ICommand QrCodeClick { get; set; }
        public ICommand BlockSelectedPoint { get; set; }

        public bool IsUserReachedExit { get; set; } = false;

        public EvacuationPlanViewModel()
        {
            userService = TinyIOC.Container.Resolve<IUserService>();
            loginService = TinyIOC.Container.Resolve<ILoginService>();
            evacuationService = TinyIOC.Container.Resolve<IEvacuationService>();


            QrCodeClick = new Command(async () =>
            {
                IsBusy = true;
                await ShowQRCode();
                IsBusy = false;
            });

            SyncPosBtnClick = new Command(async () =>
            {
                IsBusy = true;
                await SyncPosHandler();
                await InitEvacPlan(userId);
                IsBusy = false;
            });

            NextCompartment = new Command(async () =>
            {
                IsBusy = true;
                if (evacuationPlanIndex <= EvacuationsPlans.Count - 1)
                {
                    var userInfoData = await loginService.ReadDataFromStorage();
                    await InitEvacPlan(userInfoData.UserId);
                    evacuationPlanIndex++;
                }
                else
                {
                    IsUserReachedExit = true;
                    await PopupNavigation.Instance.PushAsync(new PopupYesActionView(async () =>
                    {
                        await NavigationDispetcher.Instance.Navigation.PushModalAsync(new ShelterRoutePage());
                    }, "You have reached the exit. Get route to compartment?", true));

                }
                IsBusy = false;
            });

            FireBtnClick = new Command(async () =>
            {
                IsBusy = true;
                var userInfoData = await loginService.ReadDataFromStorage();
                await userService.SendMessage(new MessageDto()
                {
                    UserId = userInfoData.UserId,
                    MessageType = UserMessageType.FIRE
                });
                IsBusy = false;
            });

            HelpBtnClick = new Command(async () =>
            {
                IsBusy = true;
                var userInfoData = await loginService.ReadDataFromStorage();
                await userService.SendMessage(new MessageDto()
                {
                    UserId = userInfoData.UserId,
                    MessageType = UserMessageType.PERSONAL_HELP
                });
                IsBusy = false;
            });

            BlockSelectedPoint = new Command<Position>(async (pointCoords) =>
            {
                await userService.BlockPoint(pointCoords);
                await InitEvacPlans();
            });

        }


        public async Task InitEvacPlans()
        {
            IsBusy = true;
            var userInfoData = await loginService.ReadDataFromStorage();

            if (userInfoData == null)
            {
                return;
            }

            userId = userInfoData.UserId;
            var userFullData = await userService.GetUserInfoById(userId);
            this.currentCompartment = userFullData.CurrentCompartment;

            if (this.currentCompartment == null)
            {
                await RequestQrCode(userId);
                return;
            }

            await InitEvacuationInfo();

            IsBusy = false;
        }

        private async Task InitEvacuationInfo()
        {

            EvacuationsPlans = await evacuationService.GetEvacuationPlansFromCompartment();

            await SyncPosHandler();

            await InitEvacPlan(userId);

        }

        private async Task InitEvacPlan(int userId)
        {
            
            await userService.SetUserCompartmentByEvacPlanId(userId, EvacuationsPlans[evacuationPlanIndex].Id);
            var evacCompartmentRoutePoints = await evacuationService.BuildCompartmentEvacRouteForUser();

            OnEvacuationPlanInit(EvacuationsPlans[evacuationPlanIndex], evacCompartmentRoutePoints);

        }

        private async Task SyncPosHandler()
        {
            var cts = new CancellationTokenSource();

            var currentPostion = await LocationSyncer.GetCurrentLocation(cts);
            var newPos = new Position()
            {
                Latitude = currentPostion.Latitude.ToString(),
                Longtitude = currentPostion.Longitude.ToString()
            };

            var updatedWorldUserPos = await userService.UpdateUserWorldPosition(newPos);
            try
            {
                var mappedUserPos = await userService.GetTransformedWorldPosition(
                    EvacuationsPlans[evacuationPlanIndex].Id,
                    updatedWorldUserPos);

                if (mappedUserPos != null)
                    CurrentUserPostion = mappedUserPos;
                else
                    await PopupNavigation.Instance.PushAsync(new PopupNotificationView("Can't sync position. Map scale is not set", MessageType.Error));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async Task RequestQrCode(int userId)
        {
            var yesActionView = new PopupYesActionView(async () =>
            {
                //var scanner = DependencyService.Get<IQrScaninngService>();
                //var result = await scanner.ScanAsync();
                //QrModel scannedQrModel = JsonConvert.DeserializeObject<QrModel>(result);

                await PopupNavigation.Instance.PushAsync(new InputPopUp(async (InputResult result) =>
                {
                    QrModel scannedQrModel = new QrModel()
                    {
                        CompatrmentId = result.CompartmentId,
                    };
                    await userService.SetUserCompartmentByCompartmentId(userId, scannedQrModel.CompatrmentId.Value);
                    var userFullData = await userService.GetUserInfoById(userId);
                    this.currentCompartment = userFullData.CurrentCompartment;

                    await InitEvacuationInfo();

                    while (PopupNavigation.Instance.PopupStack.Count > 0)
                        await PopupNavigation.Instance.PopAsync(true);
                }));


                //if (!scannedQrModel.CompatrmentId.HasValue)
                //{
                //    await RequestQrCode(userId);
                //}
                //else
                //{
                //    await userService.SetUserCompartment(userId, scannedQrModel.CompatrmentId.Value);

                //    var userFullData = await userService.GetUserInfoById(userId);
                //    this.currentCompartment = userFullData.CurrentCompartment;

                //    await InitEvacuationInfo(userId);
                //}

            },
            "Scan qr code to get current compartment info", false);
            await PopupNavigation.Instance.PushAsync(yesActionView);

        }

        private async Task ShowQRCode()
        {
            if (currentCompartment != null)
            {

                var userInfoData = await loginService.ReadDataFromStorage();
                var userId = userInfoData.UserId;
                var qrModel = new QrModel()
                {
                    CompatrmentId = currentCompartment.Id,
                    UserId = userId
                };

                await PopupNavigation.Instance.PushAsync(new QRCodePopup(qrModel));
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new PopupNotificationView("Unable to show QR code", MessageType.Error));
            }

        }
    }
}
