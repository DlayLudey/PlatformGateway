using System;
using System.Runtime.InteropServices;
using AOT;

namespace Qt.OkSdk
{
	public static class Billing
	{
		[DllImport("__Internal")]
		private static extern void ShowPayment(string name, string description, string code, int price,
			Action onSuccess, Action<string> onError);

		private static Action s_onPaymentSuccess;
		private static Action<string> s_onPaymentError;

		public static void Purchase(string name, string description, string code, int price, Action onSuccess,
			Action<string> onError = null)
		{
			s_onPaymentSuccess = onSuccess;
			s_onPaymentError = onError;

			#if !UNITY_EDITOR
			ShowPayment(name, description, code, price, OnPaymentSuccess, OnPaymentError);
			#else
			OnPaymentSuccess();
			#endif
		}

		[MonoPInvokeCallback(typeof(Action))]
		private static void OnPaymentSuccess()
		{
			s_onPaymentSuccess?.Invoke();
		}

		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnPaymentError(string error)
		{
			s_onPaymentError?.Invoke(error);
		}
	}
}