using System;
using System.Runtime.InteropServices;
using AOT;

namespace CarrotHood.PlatformGateway.Vk
{
	public static class Advertisement
	{
#region Interstitial
		[DllImport("__Internal")]
		private static extern void VkShowInterstitial(Action closeCallback, Action<string> errorCallback);

		private static Action interstitialCloseCallback;
		private static Action<string> interstitialErrorCallback;
		public static void ShowInterstitialAd(Action closeCallback = null, Action<string> errorCallback = null)
		{
			interstitialCloseCallback = closeCallback;
			interstitialErrorCallback = errorCallback;
			
			#if !UNITY_EDITOR
            VkShowInterstitial(OnInterstitialClose, OnInterstitialError);
			#else
			OnInterstitialClose();
			#endif
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
		private static extern void VkShowRewarded(Action closeCallback, Action<string> errorCallback);

		private static Action rewardedCloseCallback;
		private static Action<string> rewardedErrorCallback;
		public static void ShowRewardedAd(Action closeCallback = null, Action<string> errorCallback = null)
		{
			rewardedCloseCallback = closeCallback;
			rewardedErrorCallback = errorCallback;
			
			#if !UNITY_EDITOR
            VkShowRewarded(OnRewardedClose, OnRewardedError);
			#else
			OnRewardedClose();
			#endif
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