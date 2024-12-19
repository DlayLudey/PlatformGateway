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
		[SerializeField] private Image currencyImage;
		[SerializeField] private GameObject _overlay;

		private PaymentsBase Payments => PlatformGateway.Payments;
		private void Start()
		{
			_isSupported.text = $"Is Supported: {Payments.PaymentsSupported}";
			_getCatalogButton.onClick.AddListener(OnGetCatalogButtonClicked);
			_getPurchasesButton.onClick.AddListener(OnGetPurchasesButtonClicked);
			_purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
			_consumePurchaseButton.onClick.AddListener(OnConsumePurchaseButtonClicked);
			currencyImage.sprite = PlatformGateway.Payments.CurrencySprite;
		}

		private void OnGetCatalogButtonClicked()
		{
			_overlay.SetActive(true);
		}

		private void OnGetPurchasesButtonClicked()
		{
			
		}

		private void OnPurchaseButtonClicked()
		{
			_overlay.SetActive(true);
			

			Payments.Purchase("bar", true,
				() => { _overlay.SetActive(false); },
				(x) =>
				{
					Debug.Log("App:" + x);
					_overlay.SetActive(false);
				});
		}

		private void OnConsumePurchaseButtonClicked()
		{
		}
	}
}