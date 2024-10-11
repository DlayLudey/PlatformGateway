using System;
using System.Collections.Generic;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Payments;


namespace CarrotHood.PlatformDeps
{
	public class DefaultPayments : IPayments
	{
		PaymentsModule Payments => Bridge.payments;
		public bool isSupported => Payments;

		public void ConsumePurchase(Dictionary<string, object> options, Action<bool> onComplete = null)
			=> Payments.ConsumePurchase(options, onComplete);

		public void GetCatalog(Action<bool, List<Dictionary<string, string>>> onComplete = null)
			=> Payments.GetCatalog(onComplete);

		public void GetPurchases(Action<bool, List<Dictionary<string, string>>> onComplete = null)
			=> Payments.GetPurchases(onComplete);

		public void Purchase(Dictionary<string, object> options, Action<bool> onComplete = null)
			=> Payments.Purchase(options, onComplete);
	}
}

