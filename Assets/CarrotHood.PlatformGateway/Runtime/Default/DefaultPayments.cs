using System;
using UnityEngine;


namespace CarrotHood.PlatformGateway
{
	public class DefaultPayments : PaymentsBase
	{
		public DefaultPayments(Product[] products, StorageBase storageBase) : base(storageBase)
		{
			Products = products;
		}
		
		public override Product[] Products { get; protected set; }

		public override string CurrencyName { get; protected set; } = "$";
		public override Sprite CurrencySprite { get; protected set; } = Resources.Load<Sprite>("PlatformGateway/CurrencyIcons/Dabloon");

		public override bool PaymentsSupported => true;
		public override bool ConsummationSupported => true;
		
		protected override void InternalConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null)
		{
			onSuccessCallback?.Invoke();
		}

		protected override void InternalGetPurchases(Action<PurchasedProduct[]> onSuccessCallback, Action<string> onErrorCallback = null)
		{
			onSuccessCallback?.Invoke(new PurchasedProduct[]{});
		}

		protected override void InternalPurchase(string productId, Action<PurchasedProduct?> onSuccessCallback = null, Action<string> onErrorCallback = null, string payload = null)
		{
			onSuccessCallback?.Invoke(new PurchasedProduct
			{
				productId = productId,
				consummationToken = "12345678",
				payload = payload
			});
		}
	}
}