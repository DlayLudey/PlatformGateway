using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Qt.OkSdk
{
	public static class Advertisement
	{
#region InterstitialAds
		[DllImport("__Internal")]
		private static extern void OkShowInterstitial(Action onOpen, Action onClose, Action<string> onError);

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
			OkShowInterstitial(OnInterstitialOpen, OnInterstitialClosed, OnInterstitialError);
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
		private static extern void OkShowRewarded(Action onRewarded, Action<string> onError);

		private static bool s_showingRewardedAd;

		private static Action s_onRewardedShown;
		private static Action<string> s_onRewardedError;
		
		public static void ShowRewardedAd(Action onRewarded, Action<string> onError = null)
		{
			if(s_showingRewardedAd)
				return;
			
			s_showingRewardedAd = true;

			s_onRewardedShown = onRewarded;
			s_onRewardedError = onError;
			
			#if !UNITY_EDITOR
			OkShowRewarded(OnRewardedShown, OnRewardedError);
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
		
		[DllImport("__Internal")]
		private static extern void OkLoadRewardedAd(Action onSuccess, Action<string> onError);

		private static Action s_onLoadRewardedAdSuccess;
		private static Action<string> s_onLoadRewardedAdError;
		public static void LoadRewardedAd(Action onSuccess, Action<string> onError)
		{
			s_onLoadRewardedAdSuccess = onSuccess;
			s_onLoadRewardedAdError = onError;
			
			#if !UNITY_EDITOR
			OkLoadRewardedAd(OnLoadRewardedAdSuccess, OnLoadRewardedAdError);
			#else
			OnLoadRewardedAdSuccess();
			#endif
		}
		
		[MonoPInvokeCallback(typeof(Action))]
		private static void OnLoadRewardedAdSuccess()
		{
			s_onLoadRewardedAdSuccess?.Invoke();
		}
		
		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnLoadRewardedAdError(string error)
		{
			s_onLoadRewardedAdError?.Invoke(error);
		}
#endregion
	}
}
