using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Agava.YandexGames;
using Kimicu.YandexGames;
using UnityEngine;
using static CarrotHood.PlatformGateway.IAccount;
using Billing = Kimicu.YandexGames.Billing;
using WebApplication = Kimicu.YandexGames.WebApplication;
using YandexGamesSdk = Kimicu.YandexGames.YandexGamesSdk;

namespace CarrotHood.PlatformGateway.Yandex
{
	[CreateAssetMenu(fileName = "YandexPlatform", menuName = "Platforms/Yandex")]
	public class YandexPlatform : PlatformBase
	{
		public override PlatformType Type => PlatformType.Yandex;
		public override string Language => YandexGamesSdk.Environment.i18n.lang;
		public override DateTime CurrentTime => YandexGamesSdk.ServerTime;

		public override IEnumerator Init(PlatformBuilder builder)
		{
			yield return YandexGamesSdk.Initialize();
			yield return Billing.Initialize(ProductPictureSize.svg);
			Advertisement.Initialize();
			
			WebApplication.Initialize((isFocused) => OnGameFocusChanged?.Invoke(isFocused));
			
			builder.Storage = new StorageYandex(saveCooldown);

			yield return builder.Storage.Initialize();
			
			var payments = new PaymentsYandex(builder.Storage);

			yield return payments.Init();

			builder.Payments = payments;

			builder.Account = new AccountYandex();

			yield return builder.Account.GetPlayerData();
			
			builder.Advertisement = new AdvertisementYandex(interstitialCooldown);
			builder.Social = new SocialYandex();
		}

		public override void GameReady()
		{
			YandexGamesSdk.GameReady();
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

			bool downloadedCurrency = false;
			Utils.DownloadSvg(Billing.CatalogProducts[0].priceCurrencyPicture, tex =>
			{
				CurrencySprite = Utils.TextureToSprite(tex);
				downloadedCurrency = true;
			});

			yield return new WaitUntil(() => downloadedCurrency);
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

		public override void ShowRewarded(Action onRewarded, Action onOpen = null, Action onClose = null, Action<string> onError = null)
		{
			Advertisement.ShowVideoAd(onOpen, onRewarded, onClose, onError);
		}
	}

	public class StorageYandex : StorageBase
	{
		public StorageYandex(float savePeriod) : base(savePeriod) { }

		public override void LoadData(string key, Action<string> successCallback, Action<string> errorCallback = null)
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

	public class SocialYandex : ISocial
	{
		public bool isAddToFavoritesSupported => false;
		public bool isAddToHomeScreenSupported => false;
		public bool isCreatePostSupported => false;
		public bool isExternalLinksAllowed => false;
		public bool isInviteFriendsSupported => false;
		public bool isJoinCommunitySupported => false;
		public bool isRateSupported => false;
		public bool isShareSupported => false;
		public void AddToFavorites(Action<bool> onComplete = null) { }

		public void AddToHomeScreen(Action<bool> onComplete = null) { }

		public void CreatePost(Dictionary<string, object> options, Action<bool> onComplete = null) { }

		public void InviteFriends(string inviteText, Action<bool> onComplete = null, Action<string> onError = null) { }

		public void JoinCommunity(Dictionary<string, object> options, Action<bool> onComplete = null) { }

		public void Rate(Action<bool> onComplete = null) { }

		public void Share(Dictionary<string, object> options, Action<bool> onComplete = null) { }
	}

	public class AccountYandex : IAccount
	{
		public PlayerData Player { get; private set; }
		
		public IEnumerator GetPlayerData()
		{
			bool gotPlayer = false;
			
			Account.GetProfileData(x =>
			{
				gotPlayer = true;
				Player = new PlayerData()
				{
					id = x.uniqueID,
					name = x.publicName,
					profilePictureUrl = x.profilePicture
				};
			}, Debug.LogError);

			yield return new WaitUntil(() => gotPlayer);
		}
	}
}