﻿using System;
using UnityEngine;

namespace CarrotHood.PlatformGateway
{
	public interface IPayments
	{
		Product[] Products { get; }
		
		string CurrencyName { get; }
		Sprite CurrencySprite { get; }

		bool paymentsSupported { get; }
		bool consummationSupported { get; }

		void ConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null);
		void GetPurchases(Action<PurchasedProduct[]> onSuccessCallback, Action<string> onErrorCallback = null);
		void Purchase(string productId, Action<PurchasedProduct?> onSuccessCallback = null, Action<string> onErrorCallback = null);
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