using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CarrotHood.PlatformGateway
{
	public abstract class PaymentsBase
	{
		public PaymentsBase(StorageBase storageBase)
		{
			storage = storageBase;
		}

		private readonly StorageBase storage;

		public abstract Product[] Products { get; protected set;}

		public abstract string CurrencyName { get; protected set; }
		public abstract Sprite CurrencySprite { get; protected set;}

		public abstract bool PaymentsSupported { get; }
		public abstract bool ConsummationSupported { get; }

		public void ConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null)
		{
			if(!ConsummationSupported)
			{
				onErrorCallback?.Invoke("Consummation is not supported on this platform.");
				return;
			}
			
			InternalConsumePurchase(productToken, onSuccessCallback, onErrorCallback);
			OnApplyPurchase();
		}

		public void GetPurchases(Action<PurchasedProduct[]> onSuccessCallback, Action<string> onErrorCallback = null)
		{
			if (!ConsummationSupported)
			{
				onErrorCallback?.Invoke("Consummation is not supported on this platform.");
				return;
			}
			
			InternalGetPurchases(onSuccessCallback, onErrorCallback);
		}

		public void Purchase(string productId, bool consumable, Action onSuccessCallback = null, Action<string> onErrorCallback = null)
		{
			if (!PaymentsSupported)
			{
				onErrorCallback?.Invoke("Payment is not supported on this platform.");
				return;
			}
			
			InternalPurchase(productId, (product) =>
			{
				if(consumable && ConsummationSupported)
					ConsumePurchase(product!.Value.consummationToken, onSuccessCallback, onErrorCallback);
				else
				{
					onSuccessCallback?.Invoke();
					OnApplyPurchase();
				}
			}, onErrorCallback);
		}
		
		protected abstract void InternalConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null);
		protected abstract void InternalGetPurchases(Action<PurchasedProduct[]> onSuccessCallback, Action<string> onErrorCallback = null);
		protected abstract void InternalPurchase(string productId, Action<PurchasedProduct?> onSuccessCallback = null, Action<string> onErrorCallback = null);

		private void OnApplyPurchase()
		{
			storage.SaveStoredData();
		}
	}

	[Serializable]
	public struct Product
	{
		public string productId;
		public string name;
		public string description;
		public int price;
	}

	[Serializable]
	public struct PurchasedProduct
	{
		public string productId;
		public string consummationToken;
	}
}