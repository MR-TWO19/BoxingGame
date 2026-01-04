#if UNITY_IOS
using System.Collections;
using Unity.Advertisement.IosSupport;
using Unity.Notifications.iOS;
using System.Runtime.InteropServices;

namespace AudienceNetwork
{
    public static class AdSettings
    {
        [DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeSetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled);

        public static void SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled)
        {
            FBAdSettingsBridgeSetAdvertiserTrackingEnabled(advertiserTrackingEnabled);
        }
    }
}

namespace vgame
{
    internal class iOSAuthorizationRequest
    {
        internal static string DeviceToken { get; private set; } = null;

        internal static IEnumerator RequestAuthorization()
        {
            yield return RequestPushNotificationAuthorization();
            yield return null;
            RequestATTAuthorization();
        }

        static IEnumerator RequestPushNotificationAuthorization()
        {
            var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
            using var req = new AuthorizationRequest(authorizationOption, true);
            while (!req.IsFinished)
            {
                yield return null;
            }

            DeviceToken = string.IsNullOrEmpty(req.Error) ? req.DeviceToken : "";
        }

        static void RequestATTAuthorization()
        {
            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking();
            }

            bool setFanFlag;

            if ((int)ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == 3)
                setFanFlag = true; //If==3, App is AUTHORIZED in settings
            else setFanFlag = false;  //DENIED, RESTRICTED or NOT DETERMINED (==2,1,0)

            AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(setFanFlag);
        }
    }
}
#endif