using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace CarrotHood.PlatformGateway.Telegram
{
	public static class TelegramSdk
	{
		[DllImport("__Internal")]
		private static extern void TelegramSdkInitialize(Action onSuccess);

		public static bool IsInitialized { get; private set; }

		public static IEnumerator Initialize()
		{
			if (IsInitialized)
			{
				Debug.LogWarning("Sdk is already initialized!");
				yield break;
			}
			
			#if !UNITY_EDITOR
			TelegramSdkInitialize(OnSdkInitialized);
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
	}
}