using FireSaverMobile.Contracts;
using FireSaverMobile.DI;
using FireSaverMobile.Helpers;
using FireSaverMobile.Models;
using FireSaverMobile.Models.QRModel;
using FireSaverMobile.Pages;
using FireSaverMobile.Popups.PopupNotification;
using FireSaverMobile.Popups.qrScannerProxy;
using FireSaverMobile.Services;
using KinderMobile.PopupYesNo;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FireSaverMobile.ViewModels
{
    public class CurrentCompartmentViewModel : BaseViewModel
    {
        private AuthentificationResponse authUserInfo = null;

        private IUserService userSerice;
        private ILoginService loginService;
        private IEvacuationService evacService;
        private IBuildingService buildingService;
        private ICompartmentEnterService compartmentService;

        public EventHandler<EvacuationPlanDto> OnEvacPlanRecieved;

        public ObservableCollection<UserInfoDto> ResponsibleUsers { get; set; } = new ObservableCollection<UserInfoDto>();

        public ICommand RefreshInfo { get; set; }
        public ICommand SendFireSignal { get; set; }
        public ICommand SendHelpSignal { get; set; }
        public ICommand GoToRulePage { get; set; }

        private bool isBusy = false;
        public bool IsBusy { get { return isBusy; } set { SetValue(ref isBusy, value); } }

        private string compartmentName = "";
        public string CompartmentName
        {
            get { return compartmentName; }
            set { SetValue(ref compartmentName, value); }
        }

        private string compartmentDescription = "";
        public string CompartmentDescription
        {
            get { return compartmentDescription; }
            set { SetValue(ref compartmentDescription, value); }
        }

        private string compartmentRules = "";

        private INavigation navigation;

        public CurrentCompartmentViewModel(INavigation navigation)
        {
            userSerice = TinyIOC.Container.Resolve<IUserService>();
            loginService = TinyIOC.Container.Resolve<ILoginService>();
            evacService = TinyIOC.Container.Resolve<IEvacuationService>();
            buildingService = TinyIOC.Container.Resolve<IBuildingService>();
            compartmentService = TinyIOC.Container.Resolve<ICompartmentEnterService>();

            this.navigation = navigation;

            RefreshInfo = new Command(async () =>
            {
                IsBusy = true;
                await InitInfo();
                IsBusy = false;
            });

            SendFireSignal = new Command(async () =>
            {
                IsBusy = true;
                await userSerice.SendMessage(new MessageDto()
                {
                    UserId = authUserInfo.UserId,
                    MessageType = UserMessageType.FIRE
                });

                IsBusy = false;
            });

            SendHelpSignal = new Command(async () =>
            {
                IsBusy = true;
                await userSerice.SendMessage(new MessageDto()
                {
                    UserId = authUserInfo.UserId,
                    MessageType = UserMessageType.OTHER_HELP
                });

                IsBusy = false;
            });

            GoToRulePage = new Command<INavigation>(async (nav) =>
            {

                //var scanner = DependencyService.Get<IQrScaninngService>();
                //var result = await scanner.ScanAsync();
                //QrModel scannedQrModel = JsonConvert.DeserializeObject<QrModel>(result);

                await PopupNavigation.Instance.PushAsync(new InputPopUp(async (InputResult result) =>
                {

                    if (result.CompartmentId == 0)
                    {
                        await PopupNavigation.Instance.PushAsync(new PopupNotificationView("Invalid data", MessageType.Error), true);
                        return;
                    }

                    QrModel scannedQrModel = new QrModel()
                    {
                        IOTId = result.IotId,
                        CompatrmentId = result.CompartmentId,
                    };

                    var compInfo = await compartmentService.GetCompartmentById(result.CompartmentId);

                    await nav.PushAsync(new CompartmentRulePage(compInfo.SafetyRules, scannedQrModel));

                    while (PopupNavigation.Instance.PopupStack.Count > 0)
                        await PopupNavigation.Instance.PopAsync(true);

                }));
            });
        }

        public async Task<bool> InitInfo()
        {
            authUserInfo = await loginService.ReadDataFromStorage();
            var userInfo = await userSerice.GetUserInfoById(authUserInfo.UserId);
            if (userInfo.CurrentCompartment == null)
            {
                GoToRulePage.Execute(navigation);
                
                return false;
            }
            else
            {
                CompartmentDescription = userInfo.CurrentCompartment.Description;
                CompartmentName = userInfo.CurrentCompartment.Name;
                compartmentRules = userInfo.CurrentCompartment.SafetyRules;
                var evacPlans = await evacService.GetEvacuationPlansFromCompartment();


                OnEvacPlanRecieved?.Invoke(null, evacPlans.First());

                var buidingInfo = await buildingService.GetBuildingInfo(userInfo.CurrentCompartment.Id);
                ResponsibleUsers.Clear();
                foreach (var user in buidingInfo.ResponsibleUsers)
                {
                    ResponsibleUsers.Add(user);
                }
                return true;

            }
        }
    }
}
