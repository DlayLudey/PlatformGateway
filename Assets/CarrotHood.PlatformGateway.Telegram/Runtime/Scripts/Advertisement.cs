using System;
using System.Runtime.InteropServices;
using AOT;

namespace CarrotHood.PlatformGateway.Telegram
{
    public static class Advertisement
    {
#region Interstitial
        [DllImport("__Internal")]
        private static extern void TgShowInterstitial(Action openCallback, Action closeCallback, Action<string> errorCallback);

        private static Action interstitialOpenCallback;
        private static Action interstitialCloseCallback;
        private static Action<string> interstitialErrorCallback;
        public static void ShowInterstitialAd(Action openCallback = null, Action closeCallback = null, Action<string> errorCallback = null)
        {
            interstitialOpenCallback = openCallback;
            interstitialCloseCallback = closeCallback;
            interstitialErrorCallback = errorCallback;
            #if !UNITY_EDITOR
            TgShowInterstitial(OnInterstitialOpen, OnInterstitialClose, OnInterstitialError);
            #else
            OnInterstitialOpen();
            OnInterstitialClose();
            #endif
        }
        
        [MonoPInvokeCallback(typeof(Action))]
        private static void OnInterstitialOpen()
        {
            interstitialOpenCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnInterstitialClose()
        {
            interstitialCloseCallback?.Invoke();
        }
        
        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnInterstitialError(string error)
        {
            interstitialErrorCallback?.Invoke(error);
        }
#endregion
#region Rewarded
        [DllImport("__Internal")]
        private static extern void TgShowRewarded(Action rewardedCallback, Action closeCallback, Action<string> errorCallback);

        private static Action rewardedSuccessCallback;
        private static Action rewardedCloseCallback;
        private static Action<string> rewardedErrorCallback;
        public static void ShowRewardedAd(Action rewardedCallback, Action closeCallback = null, Action<string> errorCallback = null)
        {
            rewardedSuccessCallback = rewardedCallback;
            rewardedCloseCallback = closeCallback;
            rewardedErrorCallback = errorCallback;
            #if !UNITY_EDITOR
            TgShowRewarded(OnRewardedSuccess, OnRewardedClose, OnRewardedError);
            #else
            OnRewardedSuccess();
            #endif
        }
        
        [MonoPInvokeCallback(typeof(Action))]
        private static void OnRewardedSuccess()
        {
            rewardedSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnRewardedClose()
        {
            rewardedCloseCallback?.Invoke();
        }
        
        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnRewardedError(string error)
        {
            rewardedErrorCallback?.Invoke(error);
        }
#endregion
    }
}
