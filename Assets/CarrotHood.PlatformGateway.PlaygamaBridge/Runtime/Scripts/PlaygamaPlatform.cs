using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Playgama;
using Playgama.Modules.Advertisement;
using UnityEngine;

namespace CarrotHood.PlatformGateway.PlaygamaBridge
{
	public class PlaygamaPlatform : Platform
	{
		public override PlatformType Type => PlatformType.Playgama;
		public override string Language => Bridge.platform.language;

		public override IEnumerator Init(PlatformBuilder builder)
		{
			builder.AddPayments(new PaymentsPlaygama(Settings.products));
			
			builder.AddAdvertisement(new AdvertisementPlaygama(Settings.interstitialCooldown));
			
			builder.AddSocial(new SocialPlaygama());
			
			builder.AddStorage(new StoragePlaygama());
			
			yield break;
		}
	}

	public class PaymentsPlaygama : IPayments
	{
		public Product[] Products { get; }

		public PaymentsPlaygama(Product[] products)
		{
			Products = products;
		}

		public string CurrencyName { get; private set; }
		public Sprite CurrencySprite { get; private set; }
		public bool isSupported => Bridge.payments.isSupported;
		
		
		public void ConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null)
		{
			Bridge.payments.ConsumePurchase(new Dictionary<string, object>{{"purchaseToken", productToken}}, success =>
			{
				if (!success)
				{
					onErrorCallback?.Invoke($"Failed to consume purchase by token: {productToken}");
					return;
				}
				
				onSuccessCallback?.Invoke();
			});
		}

		public void GetPurchases(Action<object> onSuccessCallback, Action<string> onErrorCallback = null)
		{
			Bridge.payments.GetPurchases((success, data) =>
			{
				if (!success)
				{
					onErrorCallback?.Invoke("Failed to get purchases");
					return;
				}

				onSuccessCallback.Invoke(data.Select(purchase =>
				{
					GetPurchasesCallback parsedPurchase = new GetPurchasesCallback
					{
						ProductId = purchase["productID"],
						PurchaseToken = purchase["purchaseToken"],
					};

					return parsedPurchase;
				}).ToArray());
			});
		}

		public struct GetPurchasesCallback
		{
			public string ProductId;
			public string PurchaseToken;
		}

		public void Purchase(string productId, Action<object> onSuccessCallback = null, Action<string> onErrorCallback = null)
		{
			Bridge.payments.Purchase(new Dictionary<string, object>{{"id", productId}}, (success) =>
			{
				if (!success)
				{
					onErrorCallback?.Invoke($"Failed to purchase product by product id: {productId}");
					return;
				}

				onSuccessCallback?.Invoke(null);
			});
		}
	}

	public class AdvertisementPlaygama : AdvertisementBase
	{
		public AdvertisementPlaygama(int platformInterstitialCooldown) : base(platformInterstitialCooldown)
		{
			Bridge.advertisement.interstitialStateChanged += OnInterstitialStateChanged;
			Bridge.advertisement.rewardedStateChanged += OnRewardedStateChanged;
		}

		private Action onInterstitialOpened;
		private Action onInterstitialClosed;
		private Action<string> onInterstitialError;
		private void OnInterstitialStateChanged(InterstitialState interstitialState)
		{
			switch (interstitialState)
			{
				case InterstitialState.Loading:
					break;
				case InterstitialState.Opened:
					onInterstitialOpened?.Invoke();
					break;
				case InterstitialState.Closed:
					onInterstitialClosed?.Invoke();
					break;
				case InterstitialState.Failed:
					onInterstitialError?.Invoke("Failed to show interstitial");
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(interstitialState), interstitialState, null);
			}
		}

		private Action onRewardedOpened;
		private Action onRewardedCompleted;
		private Action<string> onRewardedError;
		private void OnRewardedStateChanged(RewardedState rewardedState)
		{
			switch (rewardedState)
			{
				case RewardedState.Loading:
					break;
				case RewardedState.Opened:
					onRewardedOpened?.Invoke();
					break;
				case RewardedState.Rewarded:
					onRewardedCompleted?.Invoke();
					break;
				case RewardedState.Closed:
					break;
				case RewardedState.Failed:
					onRewardedError?.Invoke("Failed to show rewarded");
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(rewardedState), rewardedState, null);
			}
		}

		public override void CheckAdBlock(Action<bool> callback)
		{
			Bridge.advertisement.CheckAdBlock(callback);
		}

		public override void ShowBanner(Dictionary<string, object> options = null)
		{
			Bridge.advertisement.ShowBanner(options);
		}

		public override void HideBanner()
		{
			Bridge.advertisement.HideBanner();
		}

		protected override void ShowInterstitialInternal(Action onOpen, Action onClose, Action<string> onError)
		{
			onInterstitialOpened = onOpen;
			onInterstitialClosed = onClose;
			onInterstitialError = onError;
			
			Bridge.advertisement.ShowInterstitial();
		}

		public override void ShowRewarded(Action onRewarded, Action onOpened = null, Action<string> onError = null)
		{
			onRewardedCompleted = onRewarded;
			onRewardedOpened = onOpened;
			onRewardedError = onError;
			
			Bridge.advertisement.ShowRewarded();
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
		
		public void AddToFavorites(Action<bool> onComplete = null)
		{
			Bridge.social.AddToFavorites(onComplete);
		}

		public void AddToHomeScreen(Action<bool> onComplete = null)
		{
			Bridge.social.AddToHomeScreen(onComplete);
		}

		public void CreatePost(Dictionary<string, object> options, Action<bool> onComplete = null)
		{
			Bridge.social.CreatePost(options, onComplete);
		}

		public void InviteFriends(string inviteText, Action<int> onComplete = null, Action<string> onError = null)
		{
			Bridge.social.InviteFriends(new Dictionary<string, object>{{"text", inviteText}}, success =>
			{
				if (!success)
				{
					onError?.Invoke("Failed to invite friends");
					return;
				}
				
				onComplete?.Invoke(1);
			});
		}

		public void JoinCommunity(Dictionary<string, object> options, Action<bool> onComplete = null)
		{
			Bridge.social.JoinCommunity(options, onComplete);
		}

		public void Rate(Action<bool> onComplete = null)
		{
			Bridge.social.Rate(onComplete);
		}

		public void Share(Dictionary<string, object> options, Action<bool> onComplete = null)
		{
			Bridge.social.Share(options, onComplete);
		}
	}

	public class StoragePlaygama : IStorage
	{
		public void GetValue(string key, Action<string> onSuccess, Action<string> onError = null)
		{
			Bridge.storage.Get(key, (success, value) =>
			{
				if (!success)
				{
					onError?.Invoke($"Failed to get storage value by key: {key}");
					return;
				}
				
				onSuccess?.Invoke(value);
			});
		}

		public void SetValue(string key, string value, Action onSuccess = null, Action<string> onError = null)
		{
			Bridge.storage.Set(key, value, success =>
			{
				if (!success)
				{
					onError?.Invoke($"Failed to set storage value: {value}\n" +
					                $"By key: {key}");
				}
			});
		}
	}
}

