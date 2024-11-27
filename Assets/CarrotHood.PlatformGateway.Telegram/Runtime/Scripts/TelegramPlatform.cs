using AOT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CarrotHood.PlatformGateway.Telegram
{
    [CreateAssetMenu(fileName = "TelegramPlatform", menuName = "Platforms/Telegram")]
    public class TelegramPlatform : Platform
    {
        public override PlatformType Type => PlatformType.Telegram;
        public override string Language { get; }

        public override IEnumerator Init(PlatformBuilder baseDeps)
        {
            yield return base.Init(baseDeps);
            baseDeps.AddAdvertisement(new TelegramAds(120));
            baseDeps.AddStorage(new TelegramStorage());
        }
    }

    public class TelegramAds : AdvertisementBase
    {
        public TelegramAds(int platformInterstitialCooldown) : base(platformInterstitialCooldown)
        {
        }

        public override void CheckAdBlock(Action<bool> callback)
        {
            Debug.LogWarning("The CheckAdBlock is not supported!");
        }

        public override void HideBanner()
        {
            Debug.LogWarning("The banner is not supported!");
        }

        public override void ShowBanner(Dictionary<string, object> options = null)
        {
            Debug.LogWarning("The banner is not supported!");
        }

        public override void ShowRewarded(Action onRewarded, Action onOpened = null, Action<string> onError = null)
        {
            TelegramAdsInternal.ShowRewardedAd(onRewarded, onError);
        }

        protected override void ShowInterstitialInternal(Action onOpen, Action onClose, Action<string> onError)
        {
            TelegramAdsInternal.ShowInterstitialAd(onOpen, onClose, onError);
        }
    }
    public class TelegramStorage : IStorage
    {
        public void GetValue(string key, Action<string> onSuccess, Action<string> onError = null)
        {
            TelegramStorageInternal.GetStorageValue(key, onSuccess, onError);
        }

        public void SetValue(string key, string value, Action onSuccess = null, Action<string> onError = null)
        {
           TelegramStorageInternal.SetStorageValue(key, value, onSuccess, onError);
        }
    }
    public class TelegramPayments : IPayments
    {
     
        public Product[] Products => throw new NotImplementedException();

        public string CurrencyName => throw new NotImplementedException();

        public Sprite CurrencySprite => throw new NotImplementedException();

        public bool paymentsSupported => throw new NotImplementedException();

        public bool consummationSupported => false;

        public PlatformSettings Settings { get; }

        public TelegramPayments(PlatformSettings settings)
        {
            Settings = settings;
        }
        public void ConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            
        }

        public void GetPurchases(Action<PurchasedProduct[]> onSuccessCallback, Action<string> onErrorCallback = null)
        {
            
        }

        public void Purchase(string productId, Action<PurchasedProduct?> onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            
        }
    }
    public static class TelegramAdsInternal
    {
        #region InterstitialAds
        [DllImport("__Internal")]
        private static extern void ShowInterstitial(Action onOpen, Action onClose, Action<string> onError);

        private const int InterstitialInterval = 30;

        private static float s_lastInterstitialTime = -InterstitialInterval;

        public static bool InterstitialAvailable =>
            Time.realtimeSinceStartup - s_lastInterstitialTime > InterstitialInterval;

        private static Action s_onInterstitialOpen;
        private static Action s_onInterstitialClosed;
        private static Action<string> s_onInterstitialError;

        public static void ShowInterstitialAd(Action onOpen = null, Action onClose = null, Action<string> onError = null)
        {
            if (!InterstitialAvailable)
            {
                onError?.Invoke($"Advertisement is not available yet, wait for {InterstitialInterval - (Time.realtimeSinceStartup - s_lastInterstitialTime)} seconds");
                return;
            }

            s_onInterstitialOpen = onOpen;
            s_onInterstitialClosed = onClose;
            s_onInterstitialError = onError;

#if !UNITY_EDITOR
			ShowInterstitial(OnInterstitialOpen, OnInterstitialClosed, OnInterstitialError);
#else
            OnInterstitialOpen();
            OnInterstitialClosed();
#endif
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnInterstitialOpen()
        {
            s_onInterstitialOpen?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnInterstitialClosed()
        {
            s_lastInterstitialTime = Time.realtimeSinceStartup;

            s_onInterstitialClosed?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnInterstitialError(string error)
        {
            s_onInterstitialError?.Invoke(error);
        }
        #endregion

        #region RewardedAds
        [DllImport("__Internal")]
        private static extern void ShowRewarded(Action onRewarded, Action<string> onError);

        private static bool s_showingRewardedAd;

        private static Action s_onRewardedShown;
        private static Action<string> s_onRewardedError;

        public static void ShowRewardedAd(Action onRewarded, Action<string> onError = null)
        {
            if (s_showingRewardedAd)
                return;

            s_showingRewardedAd = true;

            s_onRewardedShown = onRewarded;
            s_onRewardedError = onError;

#if !UNITY_EDITOR
			ShowRewarded(OnRewardedShown, OnRewardedError);
#else
            OnRewardedShown();
#endif
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnRewardedShown()
        {
            s_onRewardedShown?.Invoke();
            s_showingRewardedAd = false;
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnRewardedError(string error)
        {
            s_onRewardedError?.Invoke(error);
            s_showingRewardedAd = false;
        }
        #endregion
    }

    public static class TelegramStorageInternal
    {
        public enum Scope
        {
            CUSTOM,
            GLOBAL
        }

        #region GetStorage
        [DllImport("__Internal")]
        private static extern void GetStorage(string key, string scope, Action<string> onSuccess, Action<string> onError);

        private static Action<string> s_onGetStorageSuccess;
        private static Action<string> s_onGetStorageError;

        public static void GetStorageValue(string key, Action<string> onSuccess, Action<string> onError = null, Scope scope = Scope.CUSTOM)
        {
            s_onGetStorageSuccess = onSuccess;
            s_onGetStorageError = onError;

#if !UNITY_EDITOR
            GetStorage(key, scope.ToString(), OnGetStorageSuccess, OnGetStorageError);
#else
            OnGetStorageSuccess(PlayerPrefs.GetString(key));
#endif
        }
        [MonoPInvokeCallback(typeof(Action))]
        private static void OnGetStorageSuccess(string data)
        {
            s_onGetStorageSuccess?.Invoke(data);
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnGetStorageError(string error)
        {
            s_onGetStorageError?.Invoke(error);
        }
        #endregion

        #region SetStorage
        [DllImport("__Internal")]
        private static extern void SetStorage(string key, string value, Action onSuccess, Action<string> onError);

        private static Action s_onSetStorageSuccess;
        private static Action<string> s_onSetStorageError;

        public static void SetStorageValue(string key, string value, Action onSuccess = null, Action<string> onError = null, Scope scope = Scope.CUSTOM)
        {
            s_onSetStorageSuccess = onSuccess;
            s_onSetStorageError = onError;

#if !UNITY_EDITOR
            SetStorage(key, value, OnSetStorageSuccess, OnSetStorageError);
#else
            PlayerPrefs.SetString(key, value);
            OnSetStorageSuccess();
#endif
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnSetStorageSuccess()
        {
            s_onSetStorageSuccess?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnSetStorageError(string error)
        {
            s_onSetStorageError?.Invoke(error);
        }
        #endregion
    }
}
