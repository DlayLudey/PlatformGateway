﻿using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace CarrotHood.PlatformGateway.Vk
{
	public static class VkSdk
	{
#region Initialize
		[DllImport("__Internal")]
		private static extern void VkSdkInitialize(Action onSuccess);

		public static bool IsInitialized { get; private set; }

		public static IEnumerator Initialize()
		{
			if (IsInitialized)
			{
				Debug.LogWarning("Sdk is already initialized!");
				yield break;
			}

			#if !UNITY_EDITOR
			VkSdkInitialize(OnSdkInitialized);
			#else
			OnSdkInitialized();
			#endif

			yield return new WaitUntil(() => IsInitialized);
		}

		[MonoPInvokeCallback(typeof(Action))]
		private static void OnSdkInitialized()
		{
			IsInitialized = true;
		}
#endregion

#region GetLaunchParams
		[DllImport("__Internal")]
		private static extern void VkGetLaunchParams(Action<string> onSuccess, Action<string> onError);

		private static Action<LaunchParams> onGetLaunchParamsSuccess;
		private static Action<string> onGetLaunchParamsError;
		
		public static void GetLaunchParams(Action<LaunchParams> onSuccess, Action<string> onError = null)
		{
			onGetLaunchParamsSuccess = onSuccess;
			onGetLaunchParamsError = onError;

			#if !UNITY_EDITOR
			VkGetLaunchParams(OnGetLaunchParamsSuccess, OnGetLaunchParamsError);
			#else
			onGetLaunchParamsSuccess?.Invoke(new LaunchParams
			{
				appId = 123456,
				language = "ru",
				platform = "desktop_web"
			});
			#endif
		}
		
		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnGetLaunchParamsSuccess(string data)
		{
			onGetLaunchParamsSuccess?.Invoke(JsonConvert.DeserializeObject<LaunchParams>(data));
		}
		
		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnGetLaunchParamsError(string error)
		{
			onGetLaunchParamsError?.Invoke(error);
		}
#endregion

#region AuthToken
		[DllImport("__Internal")]
		private static extern void VkGetAuthToken(int appId, Action<string> onSuccess, Action<string> onError);
		
		private static Action<string> onGetAuthTokenSuccess;
		private static Action<string> onGetAuthTokenError;
	
		public static void GetAuthToken(int appId, Action<string> onSuccess, Action<string> onError)
		{
			onGetAuthTokenSuccess = onSuccess;
			onGetAuthTokenError = onError;
			
			#if !UNITY_EDITOR
			VkGetAuthToken(appId, OnGetAuthTokenSuccess, OnGetAuthTokenError);
			#else
			OnGetAuthTokenSuccess("Auth Token Example");
			#endif
		}

		
		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnGetAuthTokenSuccess(string token)
		{
			onGetAuthTokenSuccess?.Invoke(token);
		}
		
		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnGetAuthTokenError(string error)
		{
			onGetAuthTokenError?.Invoke(error);
		}
#endregion
	}

	[Serializable]
	public class LaunchParams
	{
		[field: Preserve, JsonProperty("vk_app_id")]
		public int appId;
		[field: Preserve, JsonProperty("vk_language")]
		public string language;
		[field: Preserve, JsonProperty("vk_platform")]
		public string platform;
		[field: Preserve, JsonProperty("vk_user_id")]
		public int userId;
	}
}