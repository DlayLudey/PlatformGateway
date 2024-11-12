using System;
using UnityEngine;


namespace CarrotHood.PlatformGateway
{
	public class DefaultPayments : IPayments
	{
		public DefaultPayments(PlatformSettings settings)
		{
			Products = settings?.products;
		}
		
		public Product[] Products { get; }
		public bool paymentsSupported => false;
		public bool consummationSupported => false;

		public string CurrencyName => "Dabloons";
		public Sprite CurrencySprite => Resources.Load<Sprite>("PlatformGateway/CurrencyIcons/Dabloon");

		public void ConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null) { }

		public void GetPurchases(Action<PurchasedProduct[]> onSuccessCallback, Action<string> onErrorCallback = null) { }

		public void Purchase(string productId, Action<PurchasedProduct?> onSuccessCallback = null,
			Action<string> onErrorCallback = null)
		{
			onSuccessCallback?.Invoke(null);
		}
	}
}