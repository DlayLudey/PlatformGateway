using System;

namespace CarrotHood.PlatformGateway
{
	public interface IPayments
	{
		bool isSupported { get; }

		void ConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null);
		void GetPurchases(Action<object> onSuccessCallback, Action<string> onErrorCallback = null);
		void Purchase(string productId, Action<object> onSuccessCallback = null, Action<string> onErrorCallback = null);
	}
}