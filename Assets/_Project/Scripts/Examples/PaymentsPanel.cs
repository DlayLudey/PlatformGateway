using System;
using System.Collections.Generic;
using CarrotHood.PlatformGateway;
using UnityEngine;
using UnityEngine.UI;

namespace Examples
{
	public class PaymentsPanel : MonoBehaviour
	{
		[SerializeField] private Text _isSupported;
		[SerializeField] private Button _getCatalogButton;
		[SerializeField] private Button _getPurchasesButton;
		[SerializeField] private Button _purchaseButton;
		[SerializeField] private Button _consumePurchaseButton;
		[SerializeField] private GameObject _overlay;

		private IPayments Payments => PlatformGateway.Payments;
		private void Start()
		{
			_isSupported.text = $"Is Supported: {Payments.paymentsSupported}";
			_getCatalogButton.onClick.AddListener(OnGetCatalogButtonClicked);
			_getPurchasesButton.onClick.AddListener(OnGetPurchasesButtonClicked);
			_purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
			_consumePurchaseButton.onClick.AddListener(OnConsumePurchaseButtonClicked);
		}

		private void OnGetCatalogButtonClicked()
		{
			_overlay.SetActive(true);

			//Payments.GetCatalog((success, list) =>
			//         {
			//             Debug.Log($"OnGetCatalogCompleted, success: {success}, items:");

			//             if (success)
			//             {
			//                 switch (Bridge.platform.id)
			//                 {
			//                     case "yandex":
			//                         foreach (var item in list)
			//                         {
			//                             Debug.Log("ID: " + item["id"]);
			//                             Debug.Log("Title: " + item["title"]);
			//                             Debug.Log("Description: " + item["description"]);
			//                             Debug.Log("Image URI: " + item["imageURI"]);
			//                             Debug.Log("Price: " + item["price"]);
			//                             Debug.Log("Price Currency Code: " + item["priceCurrencyCode"]);
			//                             Debug.Log("Price Currency Image: " + item["priceCurrencyImage"]);
			//                             Debug.Log("Price Value: " + item["priceValue"]);
			//                         }
			//                         break;
			//                 }
			//             }

			//             _overlay.SetActive(false);
			//         });
		}

		private void OnGetPurchasesButtonClicked()
		{
			// _overlay.SetActive(true);


			//Payments.GetPurchases((success, list) =>
			//         {
			//             Debug.Log($"OnGetPurchasesCompleted, success: {success}, items:");

			//             if (success)
			//             {
			//                 switch (Bridge.platform.id)
			//                 {
			//                     case "yandex":
			//                         foreach (var purchase in list)
			//                         {
			//                             Debug.Log("Product ID: " + purchase["productID"]);
			//                             Debug.Log("Purchase Token: " + purchase["purchaseToken"]);
			//                         }
			//                         break;
			//                 }
			//             }

			//             _overlay.SetActive(false);
			//         });
		}

		private void OnPurchaseButtonClicked()
		{
			_overlay.SetActive(true);

			//var options = new Dictionary<string, object>();
			//switch (Bridge.platform.id)
			//{
			//    case "yandex":
			//        options.Add("id", "YOUR_PRODUCT_ID");
			//        break;
			//}

			Payments.Purchase("bar",
				_ => { _overlay.SetActive(false); },
				(x) =>
				{
					Debug.Log("App:" + x);
					_overlay.SetActive(false);
				});
		}

		private void OnConsumePurchaseButtonClicked()
		{
			//         _overlay.SetActive(true);

			//         var options = new Dictionary<string, object>();
			//         switch (Bridge.platform.id)
			//         {
			//             case "yandex":
			//                 options.Add("purchaseToken", "YOUR_PURCHASE_TOKEN");
			//                 break;
			//         }

			//Payments.ConsumePurchase(options, _ => { _overlay.SetActive(false); });
		}
	}
}