using System;
using System.Collections.Generic;

namespace CarrotHood.PlatformDeps
{
	public interface IPayments
	{
		bool isSupported { get; }

		void ConsumePurchase(Dictionary<string, object> options, Action<bool> onComplete = null);
		void GetCatalog(Action<bool, List<Dictionary<string, string>>> onComplete = null);
		void GetPurchases(Action<bool, List<Dictionary<string, string>>> onComplete = null);
		void Purchase(Dictionary<string, object> options, Action<bool> onComplete = null);
	}
}