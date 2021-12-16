using FireSaverMobile.Popups.PopupNotification;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FireSaverMobile.Helpers
{
    public static class LocationSyncer
    {
        public static async Task<Location> GetCurrentLocation(CancellationTokenSource cts)
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    return location;
                }
                return null;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await PopupNavigation.Instance.PushAsync(new PopupNotificationView("Not supported on device", MessageType.Warning));

            }
            catch (FeatureNotEnabledException fneEx)
            {
                await PopupNavigation.Instance.PushAsync(new PopupNotificationView("Not enabled on device", MessageType.Warning));
            }
            catch (PermissionException pEx)
            {
                await PopupNavigation.Instance.PushAsync(new PopupNotificationView("No permission on device", MessageType.Warning));
            }
            catch (Exception ex)
            {
                await PopupNavigation.Instance.PushAsync(new PopupNotificationView("Can't get location", MessageType.Error));
            }
            return null;
        }
    }
}
