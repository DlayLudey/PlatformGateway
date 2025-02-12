using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace CarrotHood.PlatformGateway.Vk
{
	public static class WebApplication
	{
		[DllImport("__Internal")]
		private static extern void VkWebApplicationInitialize(Action<bool> onGameFocusChangeCallback);
		
		private static Action<bool> onGameFocusChangeCallback;
		
		public static void Initialize(Action<bool> onGameFocusChange)
		{
			onGameFocusChangeCallback = onGameFocusChange;
			
			#if !UNITY_EDITOR
			VkWebApplicationInitialize(OnGameFocusChange);
			#endif
		}
		
		[MonoPInvokeCallback(typeof(Action<bool>))]
		private static void OnGameFocusChange(bool isFocused)
		{
			onGameFocusChangeCallback?.Invoke(isFocused);
		}
	}
}