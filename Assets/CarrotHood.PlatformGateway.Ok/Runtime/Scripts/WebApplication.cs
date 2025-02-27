using System;
using System.Runtime.InteropServices;
using AOT;

namespace CarrotHood.PlatformGateway.Ok
{
	public static class WebApplication
	{
		[DllImport("__Internal")]
		private static extern void OkWebApplicationInitialize(Action<bool> onGameFocusChangeCallback);
		
		private static Action<bool> onGameFocusChangeCallback;
		
		public static void Initialize(Action<bool> onGameFocusChange)
		{
			onGameFocusChangeCallback = onGameFocusChange;
			
			#if !UNITY_EDITOR
			OkWebApplicationInitialize(OnGameFocusChange);
			#endif
		}
		
		[MonoPInvokeCallback(typeof(Action<bool>))]
		private static void OnGameFocusChange(bool isFocused)
		{
			onGameFocusChangeCallback?.Invoke(isFocused);
		}
	}
}