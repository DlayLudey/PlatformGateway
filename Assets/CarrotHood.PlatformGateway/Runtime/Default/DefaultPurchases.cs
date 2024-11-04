using System;
using UnityEngine;


namespace CarrotHood.PlatformGateway
{
	public class DefaultPayments : IPayments
	{
		public Product[] Products { get; }
		public bool paymentsSupported => false;
		public bool consummationSupported => false;

		public string CurrencyName => "Dabloons";
		public Sprite CurrencySprite => Resources.Load<Sprite>("PlatformGateway/CurrencyIcons/Dabloon");

		public void ConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null) { }

		public void GetPurchases(Action<object> onSuccessCallback, Action<string> onErrorCallback = null) { }

		public void Purchase(string productId, Action<object> onSuccessCallback = null,
			Action<string> onErrorCallback = null)
		{
			onSuccessCallback?.Invoke(null);
		}
	}
}