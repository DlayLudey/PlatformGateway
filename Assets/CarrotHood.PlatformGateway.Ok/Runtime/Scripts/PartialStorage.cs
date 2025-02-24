using System;
using System.Runtime.InteropServices;
using AOT;
using Qt.OkSdk;
using UnityEngine;

namespace CarrotHood.PlatformGateway.Ok
{
	public static class PartialStorage
	{
#region SetStorage
		[DllImport("__Internal")]
		private static extern void OkSetPartialStorage(string key, string value, Action onSuccess, Action<string> onError);

		private static Action onSetPartialStorageSuccess;
		private static Action<string> onSetPartialStorageError;
		
		public static void SetPartialStorage(string key, string value, Action onSuccess, Action<string> onError)
		{
			onSetPartialStorageSuccess = onSuccess;
			onSetPartialStorageError = onError;
			
			#if !UNITY_EDITOR
			OkSetPartialStorage(key, value, OnSetPartialStorageSuccess, OnSetPartialStorageError);
			#else
			PlayerPrefs.SetString(key, value);
			OnSetPartialStorageSuccess();
			#endif
		}
		
		[MonoPInvokeCallback(typeof(Action))]
		private static void OnSetPartialStorageSuccess()
		{
			onSetPartialStorageSuccess?.Invoke();
		}
		
		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnSetPartialStorageError(string error)
		{
			onSetPartialStorageError?.Invoke(error);
		}
#endregion
#region GetStorage
		[DllImport("__Internal")]
		private static extern void OkGetPartialStorage(string key, Action<string> onSuccess, Action<string> onError);

		private static Action<string> onGetPartialStorageSuccess;
		private static Action<string> onGetPartialStorageError;

		public static void GetPartialStorage(string key, Action<string> onSuccess, Action<string> onError)
		{
			onGetPartialStorageSuccess = onSuccess;
			onGetPartialStorageError = onError;
			
			#if !UNITY_EDITOR
			OkGetPartialStorage(key, OnGetPartialStorageSuccess, OnGetPartialStorageError);
			#else
			OnGetPartialStorageSuccess(PlayerPrefs.GetString(key));
			#endif
		}
		
		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnGetPartialStorageSuccess(string value)
		{
			onGetPartialStorageSuccess?.Invoke(value);
		}
		
		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnGetPartialStorageError(string error)
		{
			onGetPartialStorageError?.Invoke(error);
		}
#endregion
	}
}