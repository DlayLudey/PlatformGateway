using System;
using System.Collections;
using System.Collections.Generic;
using Agava.YandexGames;
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
			yield return YandexGamesSdk.Initialize();
			yield return Billing.Initialize();

			var payments = new PaymentsYandex();

			yield return payments.Init();
			
			builder.AddPayments(payments);

			yield return Cloud.Initialize();
			builder.AddStorage(new StorageYandex());
			
			Advertisement.Initialize();
			builder.AddAdvertisement(new AdvertisementYandex(Settings.interstitialCooldown));
		}
	}

	public class PaymentsYandex : IPayments
	{
		public Product[] Products { get; }
		public bool isSupported { get; } = true;

		public IEnumerator Init()
		{
			if(Billing.CatalogProducts == null || Billing.CatalogProducts.Length == 0)
				yield break;

			CurrencyName = Billing.CatalogProducts[0].priceCurrencyCode;
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

		public void GetPurchases(Action<object> onSuccessCallback, Action<string> onErrorCallback = null)
		{
			Billing.GetPurchasedProducts(onSuccessCallback, onErrorCallback);
		}

		public void Purchase(string productId, Action<object> onSuccessCallback = null, Action<string> onErrorCallback = null)
		{
			Billing.PurchaseProduct(productId, onSuccessCallback, onErrorCallback);
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

		public override void ShowRewarded(Action onRewarded, Action<string> onError)
		{
			Advertisement.ShowVideoAd(onRewardedCallback: onRewarded, onErrorCallback: onError);
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