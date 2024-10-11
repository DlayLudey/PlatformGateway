using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Qt.OkSdk
{
	public static class OkSdk
	{
		[DllImport("__Internal")]
		private static extern void OkSdkInitialize(Action onSuccess);

		public static bool IsInitialized { get; private set; }

		public static IEnumerator Initialize()
		{
			if (IsInitialized)
			{
				Debug.LogWarning("Sdk is already initialized!");
				yield break;
			}
			
			#if !UNITY_EDITOR
			OkSdkInitialize(OnSdkInitialized);
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