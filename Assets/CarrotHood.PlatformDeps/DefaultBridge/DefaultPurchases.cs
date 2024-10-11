using System;


namespace CarrotHood.PlatformDeps
{
	public class DefaultPayments : IPayments
	{
		public bool isSupported => false;

		public void ConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null) { }

		public void GetPurchases(Action<object> onSuccessCallback, Action<string> onErrorCallback = null) { }

		public void Purchase(string productId, Action<object> onSuccessCallback = null,
			Action<string> onErrorCallback = null)
		{
			onSuccessCallback?.Invoke(null);
		}
	}
}