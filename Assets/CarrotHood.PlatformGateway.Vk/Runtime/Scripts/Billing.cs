using System;
using System.Runtime.InteropServices;
using AOT;

namespace CarrotHood.PlatformGateway.Vk
{
	public static class Billing
	{
		[DllImport("__Internal")]
		private static extern void VkShowPayment(string key, Action onSuccess, Action<string> onError);

		private static Action onPurchaseSuccess;
		private static Action<string> onPurchaseError;
		
		public static void Purchase(string key, Action onSuccess, Action<string> onError = null)
		{
			onPurchaseSuccess = onSuccess;
			onPurchaseError = onError;
			
			#if !UNITY_EDITOR
			VkShowPayment(key, onSuccess, onError);
			#else
			OnPaymentSuccess();
			#endif
		}
		
		[MonoPInvokeCallback(typeof(Action))]
		private static void OnPaymentSuccess()
		{
			onPurchaseSuccess?.Invoke();
		}

		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnPaymentError(string error)
		{
			onPurchaseError?.Invoke(error);
		}
	}
}