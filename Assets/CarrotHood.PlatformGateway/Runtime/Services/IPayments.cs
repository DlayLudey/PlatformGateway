using System;
using UnityEngine;

namespace CarrotHood.PlatformGateway
{
	public interface IPayments
	{
		Product[] Products { get; }
		
		string CurrencyName { get; }
		Sprite CurrencySprite { get; }

		bool isSupported { get; }

		void ConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null);
		void GetPurchases(Action<object> onSuccessCallback, Action<string> onErrorCallback = null);
		void Purchase(string productId, Action<object> onSuccessCallback = null, Action<string> onErrorCallback = null);
	}

	[Serializable]
	public struct Product
	{
		public string productId;
		public string name;
		public string description;
		public int price;
	}
}