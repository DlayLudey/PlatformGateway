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
	public class YandexPlatform : PlatformBase
	{
		public override PlatformType Type => PlatformType.Yandex;
		public override string Language => YandexGamesSdk.Environment.i18n.lang;

		public override IEnumerator Init(PlatformBuilder builder)
		{
			yield return YandexGamesSdk.Initialize();
			yield return Cloud.Initialize();
			yield return Billing.Initialize();
			Advertisement.Initialize();

			builder.Storage = new StorageYandex(saveCooldown);

			yield return builder.Storage.Initialize();
			
			var payments = new PaymentsYandex(builder.Storage);

			yield return payments.Init();

			builder.Payments = payments;

			builder.Advertisement = new AdvertisementYandex(interstitialCooldown);
		}
	}

	public class PaymentsYandex : PaymentsBase
	{
		public PaymentsYandex(StorageBase storageBase) : base(storageBase) { }

		public override Product[] Products { get; protected set; }
		public override string CurrencyName { get; protected set; } 
		public override Sprite CurrencySprite { get; protected set; }
		public override bool PaymentsSupported => true;
		public override bool ConsummationSupported => true;
		
		public IEnumerator Init()
		{
			if(Billing.CatalogProducts == null || Billing.CatalogProducts.Length == 0)
				yield break;
		
			CurrencyName = Billing.CatalogProducts[0].priceCurrencyCode;
			
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
		
		protected override void InternalConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null)
		{
			Billing.ConsumeProduct(productToken, onSuccessCallback, onErrorCallback);
		}

		protected override void InternalGetPurchases(Action<PurchasedProduct[]> onSuccessCallback, Action<string> onErrorCallback = null)
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

		protected override void InternalPurchase(string productId, Action<PurchasedProduct?> onSuccessCallback = null, Action<string> onErrorCallback = null)
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
		public AdvertisementYandex(float platformInterstitialCooldown) : base(platformInterstitialCooldown) { }

		public override void CheckAdBlock(Action<bool> callback) => callback?.Invoke(AdBlock.Enabled);

		public override void ShowBanner(Dictionary<string, object> options = null) { }

		public override void HideBanner() { }

		protected override void ShowInterstitialInternal(Action onOpen, Action onClose, Action<string> onError)
		{
			Advertisement.ShowInterstitialAd(onOpen, onClose, onError);
		}

		public override void ShowRewarded(Action onRewarded, Action onOpened = null, Action onClose = null, Action<string> onError = null)
		{
			Advertisement.ShowVideoAd(onOpened, onRewarded, onClose, onError);
		}
	}

	public class StorageYandex : StorageBase
	{
		public StorageYandex(float savePeriod) : base(savePeriod) { }

		public void GetValue(string key, Action<string> onSuccess, Action<string> onError = null)
		{
			onSuccess?.Invoke(Cloud.GetValue(key, ""));
		}

		public void SetValue(string key, string value, Action onSuccess = null, Action<string> onError = null)
		{
			Cloud.SetValue(key, value, true);
			onSuccess?.Invoke();
		}

		protected override void LoadData(string key, Action<string> successCallback, Action<string> errorCallback = null)
		{
			#if !UNITY_EDITOR
			Agava.YandexGames.PlayerAccount.GetCloudSaveData(successCallback, errorCallback);
			#else
			successCallback?.Invoke(PlayerPrefs.GetString(key));
			#endif
		}

		public override void SaveData(string key, string value, Action successCallback = null, Action<string> errorCallback = null)
		{
			#if !UNITY_EDITOR
			Agava.YandexGames.PlayerAccount.SetCloudSaveData(value, true, successCallback, errorCallback);
			#else
			PlayerPrefs.SetString(key, value);
			successCallback?.Invoke();
			#endif
		}
	}
}