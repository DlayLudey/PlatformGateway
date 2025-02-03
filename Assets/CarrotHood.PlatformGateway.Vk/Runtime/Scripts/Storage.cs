using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace CarrotHood.PlatformGateway.Vk
{
	public static class Storage
	{
#region SetStorage
		[DllImport("__Internal")]
		private static extern void VkSetStorage(string key, string value, Action onSuccess, Action<string> onError);

		private static Action onSetStorageSuccess;
		private static Action<string> onSetStorageError;
        
		public static void SetStorage(string key, string value, Action onSuccess = null, Action<string> onError = null)
		{
			onSetStorageSuccess = onSuccess;
			onSetStorageError = onError;
            
			#if !UNITY_EDITOR
			VkSetStorage(key, value, OnSetStorageSuccess, OnSetStorageError);
			#else
			PlayerPrefs.SetString(key, value);
			OnSetStorageSuccess();
			#endif
		}

		[MonoPInvokeCallback(typeof(Action))]
		private static void OnSetStorageSuccess()
		{
			onSetStorageSuccess?.Invoke();
		}
        
		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnSetStorageError(string error)
		{
			onSetStorageError?.Invoke(error);
		}

#endregion
		
#region GetStorage
		[DllImport("__Internal")]
		private static extern void VkGetStorage(string key, Action<string> onSuccess, Action<string> onError);

		private static Action<string> onGetStorageSuccess;

		private static Action<string> onGetStorageError;

		public static void GetStorage(string key, Action<string> onSuccess, Action<string> onError = null)
		{
			onGetStorageSuccess = onSuccess;
			onGetStorageError = onError;
            
			#if !UNITY_EDITOR
            VkGetStorage(key, OnGetStorageSuccess, OnGetStorageError);
			#else
			OnGetStorageSuccess(PlayerPrefs.GetString(key));
			#endif
		}

		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnGetStorageSuccess(string value)
		{
			onGetStorageSuccess?.Invoke(value);
		}

		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnGetStorageError(string error)
		{
			onGetStorageError?.Invoke(error);
		}
#endregion
	}
}