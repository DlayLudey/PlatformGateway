using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace CarrotHood.PlatformGateway.Telegram
{
	public static class TelegramSdk
	{
#region Initialization
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
#endregion

#region GameReady

		[DllImport("__Internal")]
		private static extern void TgGameReady();

		public static void GameReady()
		{
			#if !UNITY_EDITOR
			TgGameReady();
			#else
			Debug.Log("Telegram GameReady");
			#endif
		}

#endregion
	}
}