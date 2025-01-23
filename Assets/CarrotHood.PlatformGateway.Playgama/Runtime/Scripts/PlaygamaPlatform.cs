using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Playgama;
using Playgama.Modules.Advertisement;
using UnityEngine;

namespace CarrotHood.PlatformGateway.Playgama
{
	[CreateAssetMenu(fileName = "PlaygamaPlatform", menuName = "Platforms/Playgama")]
	public class PlaygamaPlatform : PlatformBase
	{
		[Tooltip("This will be used in case of Playgama not returning product list. " +
		         "(But in this case there is probably no support of payments on the current platform)")]
		[SerializeField] private Product[] products;
		
		public override PlatformType Type => PlatformType.Playgama;
		public override string Language => Bridge.platform.language;

		public override IEnumerator Init(PlatformBuilder builder)
		{
			builder.Storage = new StoragePlaygama(saveCooldown);

			yield return builder.Storage.Initialize();

			PaymentsPlaygama payments = new (builder.Storage);

			builder.Payments = payments;

			yield return payments.Initialize(products);
			
			builder.Advertisement = new AdvertisementPlaygama(interstitialCooldown);
			builder.Social = new SocialPlaygama();
		}
	}

	public class PaymentsPlaygama : PaymentsBase
	{
		public PaymentsPlaygama(StorageBase storageBase) : base(storageBase) { }

		public IEnumerator Initialize(Product[] products)
		{
			if (!Bridge.payments.isGetCatalogSupported)
			{
				Debug.Log("Getting catalog is not supported, using provided products.");
				Products = products;
				yield break;
			}

			bool gotProducts = false;
			string imageUri = "";
			
			Bridge.payments.GetCatalog((success, productDict) =>
			{
				if (!success)
				{
					gotProducts = true;
					return;
				}
				products = productDict.Select(x =>
				{
					// Creates shit SDK with the worst API I've ever seen
					// Ads this id/productID bullshit
					// Refuses to elaborate
					// Right, Play fucking gama?
					
					string productId = x.TryGetValue("id", out string id) ? id : x["productID"]; 
					
					return new Product()
					{
						productId = productId,
						name = x["title"],
						description = x["description"],
						// AND FUCKING AGAIN
						price = int.TryParse(x.TryGetValue("priceValue", out string price) ? price : x["priceAmount"], out int parsedPrice) ? parsedPrice : 0,
					};
				}).ToArray();

				if(productDict.Count > 0)
				{
					imageUri = productDict[0].GetValueOrDefault("imageUri", "");
					CurrencyName = productDict[0].GetValueOrDefault("priceCurrencyCode", "");
				}
				
				gotProducts = true;
			});

			yield return new WaitUntil(() => gotProducts);

			Products = products;

			if (!string.IsNullOrEmpty(imageUri))
			{
				yield return Utils.DownloadSprite(imageUri, texture =>
				{
					if(texture != null)
						CurrencySprite = Utils.TextureToSprite(texture);
				});
			}

		}

		public override Product[] Products { get; protected set; }
		public override string CurrencyName { get; protected set; }
		public override Sprite CurrencySprite { get; protected set; }
		public override bool PaymentsSupported => Bridge.payments.isSupported;
		public override bool ConsummationSupported => Bridge.payments.isConsumePurchaseSupported;
		protected override void InternalConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null)
		{
			Bridge.payments.ConsumePurchase(new Dictionary<string, object>{{"purchaseToken", productToken}}, success =>
			{
				if(success)
					onSuccessCallback?.Invoke();
				else
					onErrorCallback?.Invoke("Consumation Error");
			});
		}

		protected override void InternalGetPurchases(Action<PurchasedProduct[]> onSuccessCallback, Action<string> onErrorCallback = null)
		{
			Bridge.payments.GetPurchases((success, purchases) =>
			{
				if(!success)
				{
					onErrorCallback?.Invoke("Get Purchases Error");
					return;
				}
				
				onSuccessCallback?.Invoke(purchases.Select(x => new PurchasedProduct()
				{
					productId = x.GetValueOrDefault("productID"),
					consummationToken = x.GetValueOrDefault("purchaseToken")
				}).ToArray());
			});
		}

		protected override void InternalPurchase(string productId, Action<PurchasedProduct?> onSuccessCallback = null, Action<string> onErrorCallback = null)
		{
			Dictionary<string, object> options = Bridge.platform.id switch
			{
				"yandex" => new Dictionary<string, object> {{"id", productId}},
				"facebook" => new Dictionary<string, object> {{"productID", productId}},
				"playdeck" => new Dictionary<string, object>()
				{
					{"amount", 1},
					{"description", Products.First(x => x.productId == productId).description},
					{"externalId", productId}
				},
				_ => throw new ArgumentOutOfRangeException(nameof(Bridge.platform.id), Bridge.platform.id, null),
			};
			Bridge.payments.Purchase(options);
		}
	}

	public class AdvertisementPlaygama : AdvertisementBase
	{
		public AdvertisementPlaygama(float platformInterstitialCooldown) : base(platformInterstitialCooldown)
		{
			Bridge.advertisement.interstitialStateChanged += OnInterstitialStateChanged;
			Bridge.advertisement.rewardedStateChanged += OnRewardedStateChanged;
		}

		public override void CheckAdBlock(Action<bool> callback) => Bridge.advertisement.CheckAdBlock(callback);

		public override void ShowBanner(Dictionary<string, object> options = null) => Bridge.advertisement.ShowBanner(options);

		public override void HideBanner() => Bridge.advertisement.HideBanner();

		private Action _onInterstitialOpen;
		private Action _onInterstitialClose;
		private Action<string> _onInterstitialError;
		
		protected override void ShowInterstitialInternal(Action onOpen, Action onClose, Action<string> onError)
		{
			_onInterstitialOpen = onOpen;
			_onInterstitialClose = onClose;
			_onInterstitialError = onError;
			Bridge.advertisement.ShowInterstitial();
		}
		
		private void OnInterstitialStateChanged(InterstitialState state)
		{
			switch (state)
			{
				case InterstitialState.Opened:
					_onInterstitialOpen?.Invoke();
					break;
				case InterstitialState.Closed:
					_onInterstitialClose?.Invoke();
					break;
				case InterstitialState.Failed:
					_onInterstitialError?.Invoke("Interstitial error");
					break;
			}
		}

		private Action _onRewardedOpen;
		private Action _onRewardedFinish;
		private Action _onRewardedClose;
		private Action<string> _onRewardedError;

		public override void ShowRewarded(Action onRewarded, Action onOpen = null, Action onClosed = null, Action<string> onError = null)
		{
			_onRewardedFinish = onRewarded;
			_onRewardedOpen = onOpen;
			_onRewardedClose = onClosed;
			_onRewardedError = onError;
			
			Bridge.advertisement.ShowRewarded();
		}
		
		private void OnRewardedStateChanged(RewardedState state)
		{
			switch (state)
			{
				case RewardedState.Opened:
					_onRewardedOpen?.Invoke();
					break;
				case RewardedState.Rewarded:
					_onRewardedFinish?.Invoke();
					break;
				case RewardedState.Closed:
					_onRewardedClose?.Invoke();
					break;
				case RewardedState.Failed:
					_onRewardedError?.Invoke("Rewarded error");
					break;
			}
		}
	}
	
	public class SocialPlaygama : ISocial
	{
		public bool isAddToFavoritesSupported => Bridge.social.isAddToFavoritesSupported;
		public bool isAddToHomeScreenSupported => Bridge.social.isAddToHomeScreenSupported;
		public bool isCreatePostSupported => Bridge.social.isCreatePostSupported;
		public bool isExternalLinksAllowed => Bridge.social.isExternalLinksAllowed;
		public bool isInviteFriendsSupported => Bridge.social.isInviteFriendsSupported;
		public bool isJoinCommunitySupported => Bridge.social.isJoinCommunitySupported;
		public bool isRateSupported => Bridge.social.isRateSupported;
		public bool isShareSupported => Bridge.social.isShareSupported;
		public void AddToFavorites(Action<bool> onComplete = null) => Bridge.social.AddToFavorites(onComplete);

		public void AddToHomeScreen(Action<bool> onComplete = null) => Bridge.social.AddToHomeScreen(onComplete);

		public void CreatePost(Dictionary<string, object> options, Action<bool> onComplete = null) => Bridge.social.CreatePost(options, onComplete);

		public void InviteFriends(string inviteText, Action<bool> onComplete = null, Action<string> onError = null) => 
			Bridge.social.InviteFriends(new Dictionary<string, object>{{"text", inviteText}}, onComplete); 

		public void JoinCommunity(Dictionary<string, object> options, Action<bool> onComplete = null) => Bridge.social.JoinCommunity(options, onComplete);

		public void Rate(Action<bool> onComplete = null) => Bridge.social.Rate(onComplete);

		public void Share(Dictionary<string, object> options, Action<bool> onComplete = null) => Bridge.social.Share(options, onComplete);
	}
	
	public class StoragePlaygama : StorageBase
	{
		public StoragePlaygama(float savePeriod) : base(savePeriod) { }

		public override void LoadData(string key, Action<string> successCallback, Action<string> errorCallback = null)
		{
			Bridge.storage.Get(key, (state, value) =>
			{
				if (!state)
				{
					errorCallback?.Invoke("Storage Load Error");
					return;
				}
				
				successCallback?.Invoke(value);
			});
		}

		public override void SaveData(string key, string value, Action successCallback = null, Action<string> errorCallback = null)
		{
			Bridge.storage.Set(key, value, (state) =>
			{
				if (state)
					successCallback?.Invoke();
				else
					errorCallback?.Invoke("Storage Save Error");
			});
		}
	}
}