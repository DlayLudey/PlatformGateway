using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kimicu.YandexGames;
using UnityEngine;
using Billing = Kimicu.YandexGames.Billing;
using YandexGamesSdk = Kimicu.YandexGames.YandexGamesSdk;

namespace CarrotHood.PlatformGateway.Yandex
{
	[CreateAssetMenu(fileName = "YandexPlatform", menuName = "Platforms/Yandex")]
	public partial class YandexPlatform : Platform
	{
		public override PlatformType Type => PlatformType.Yandex;
		public override string Language => YandexGamesSdk.Environment.i18n.lang;

		public override IEnumerator Init(PlatformBuilder builder)
		{
			yield return base.Init(builder);

			yield return YandexGamesSdk.Initialize();
			yield return Billing.Initialize();

			var payments = new PaymentsYandex();

			yield return payments.Init(Settings);
			
			builder.AddPayments(payments);

			yield return Cloud.Initialize();
			builder.AddStorage(new StorageYandex());
			
			Advertisement.Initialize();
			builder.AddAdvertisement(new AdvertisementYandex(Settings.interstitialCooldown));
		}
	}

	public class PaymentsYandex : IPayments
	{
		public Product[] Products { get; private set; }
		public bool paymentsSupported => true;
		public bool consummationSupported => true;

		public IEnumerator Init(PlatformSettings settings)
		{
			if(Billing.CatalogProducts == null || Billing.CatalogProducts.Length == 0)
				yield break;

			CurrencyName = Billing.CatalogProducts[0].priceCurrencyCode;
			
			#if UNITY_EDITOR
			if(settings.products != null && settings.products.Length > 0)
			{
				Debug.LogWarning($"Warning, Products from Platform Settings are only for Editor purposes on this Platform ({nameof(YandexPlatform)})");
				Products = settings.products;
			}
			#endif
			
			Products = Billing.CatalogProducts.Select(x => new Product
			{
				productId = x.id,
				name = x.title,
				description = x.description,
				price = int.Parse(x.priceValue),
			}).ToArray();
			yield return Utils.DownloadSprite(Billing.CatalogProducts[0].priceCurrencyPicture, tex =>
			{
				CurrencySprite = Utils.TextureToSprite(tex);
			});
		}

		public string CurrencyName { get; private set; }
		public Sprite CurrencySprite { get; private set; }

		public void ConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null)
		{
			Billing.ConsumeProduct(productToken, onSuccessCallback, onErrorCallback);
		}

		public void GetPurchases(Action<PurchasedProduct[]> onSuccessCallback, Action<string> onErrorCallback = null)
		{
			Billing.GetPurchasedProducts(response =>
			{
				onSuccessCallback?.Invoke(response.purchasedProducts.Select(x => new PurchasedProduct
				{
					productId = x.productID,
					consummationToken = x.purchaseToken
				}).ToArray());
			}, onErrorCallback);
		}

		public void Purchase(string productId, Action<PurchasedProduct?> onSuccessCallback = null, Action<string> onErrorCallback = null)
		{
			Billing.PurchaseProduct(productId, response =>
			{
				onSuccessCallback?.Invoke(new PurchasedProduct
				{
					productId = response.purchaseData.productID,
					consummationToken = response.purchaseData.purchaseToken
				});
			}, onErrorCallback);
		}
	}

	public class AdvertisementYandex : AdvertisementBase
	{
		public AdvertisementYandex(int platformInterstitialCooldown) : base(platformInterstitialCooldown) { }

		public override void CheckAdBlock(Action<bool> callback) => callback?.Invoke(AdBlock.Enabled);

		public override void ShowBanner(Dictionary<string, object> options = null) { }

		public override void HideBanner() { }

		protected override void ShowInterstitialInternal(Action onOpen, Action onClose, Action<string> onError)
		{
			Advertisement.ShowInterstitialAd(onOpen, onClose, onError);
		}

		public override void ShowRewarded(Action onRewarded, Action onOpened = null, Action<string> onError = null)
		{
			Advertisement.ShowVideoAd(onOpened, onRewarded, onErrorCallback: onError);
		}
	}

	public class StorageYandex : IStorage
	{
		public void GetValue(string key, Action<string> onSuccess, Action<string> onError = null)
		{
			onSuccess?.Invoke(Cloud.GetValue(key, ""));
		}

		public void SetValue(string key, string value, Action onSuccess = null, Action<string> onError = null)
		{
			Cloud.SetValue(key, value, true);
			onSuccess?.Invoke();
		}
	}
}