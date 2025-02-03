using System;
using System.Collections;
using System.Collections.Generic;
using CarrotHood.PlatformGateway.Vk.Runtime.Scripts;
using UnityEngine;

namespace CarrotHood.PlatformGateway.Vk
{
	[CreateAssetMenu(fileName = "VkPlatform", menuName = "Platforms/Vk")]
	public class VkPlatform : PlatformBase
	{
		[SerializeField] private Product[] products;

		private LaunchParams _launchParams;

		public override PlatformType Type => PlatformType.Vk;

		public override string Language => _launchParams.language;
		
		public override IEnumerator Init(PlatformBuilder builder)
		{
			yield return VkSdk.Initialize();

			yield return GetLaunchParams();
			
			builder.Storage = new StorageVk(saveCooldown);

			yield return builder.Storage.Initialize();
			
			builder.Payments = new PaymentsVk(Language, products, builder.Storage);
			
			builder.Advertisement = new AdvertisementVk(interstitialCooldown);
			builder.Social = new SocialVk();
		}

		private IEnumerator GetLaunchParams()
		{
			bool gotParams = false;
			
			VkSdk.GetLaunchParams(launchParams =>
			{
				_launchParams = launchParams;
				gotParams = true;
			}, error =>
			{
				Debug.LogError(error);
				gotParams = true;
			});

			yield return new WaitUntil(() => gotParams);
		}
	}

	public class PaymentsVk : PaymentsBase
	{
		public PaymentsVk(string language, Product[] products, StorageBase storageBase) : base(storageBase)
		{
			Products = products;
			
			CurrencyName = language == "en" ? "Votes" : "Голос";
		}
		
		public override Product[] Products { get; protected set; }
		public override string CurrencyName { get; protected set; }
		public override Sprite CurrencySprite { get; protected set; } = Resources.Load<Sprite>("PlatformGateway/CurrencyIcons/Vk");
		public override bool PaymentsSupported => true;
		public override bool ConsummationSupported => false;

		protected override void InternalConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null)
		{
			onErrorCallback?.Invoke("Consummation is not supported.");
		}

		protected override void InternalGetPurchases(Action<PurchasedProduct[]> onSuccessCallback, Action<string> onErrorCallback = null)
		{
			onErrorCallback?.Invoke("Consummation is not supported.");
		}

		protected override void InternalPurchase(string productId, Action<PurchasedProduct?> onSuccessCallback = null, Action<string> onErrorCallback = null)
		{
			Billing.Purchase(productId, () => onSuccessCallback?.Invoke(null), onErrorCallback);
		}
	}

	public class AdvertisementVk : AdvertisementBase
	{
		public AdvertisementVk(float platformInterstitialCooldown) : base(platformInterstitialCooldown) { }
		public override void CheckAdBlock(Action<bool> callback) => callback?.Invoke(false);

		public override void ShowBanner(Dictionary<string, object> options = null) { }

		public override void HideBanner() { }

		protected override void ShowInterstitialInternal(Action onOpen, Action onClose, Action<string> onError)
		{
			onOpen?.Invoke();
			Advertisement.ShowInterstitialAd(onClose, onError);
		}

		public override void ShowRewarded(Action onRewarded, Action onOpen = null, Action onClosed = null, Action<string> onError = null)
		{
			onOpen?.Invoke();
			
			Advertisement.ShowRewardedAd(() =>
			{
				onRewarded?.Invoke();
				onClosed?.Invoke();
			}, onError);
		}
	}

	public class SocialVk : ISocial
	{
		public bool isAddToFavoritesSupported => false;
		public bool isAddToHomeScreenSupported => false;
		public bool isCreatePostSupported => false;
		public bool isExternalLinksAllowed => false;
		public bool isInviteFriendsSupported => true;
		public bool isJoinCommunitySupported => false;
		public bool isRateSupported => false;
		public bool isShareSupported => false;
		public void AddToFavorites(Action<bool> onComplete = null) {}

		public void AddToHomeScreen(Action<bool> onComplete = null) {}

		public void CreatePost(Dictionary<string, object> options, Action<bool> onComplete = null) {}

		public void InviteFriends(string inviteText, Action<bool> onComplete = null, Action<string> onError = null)
		{
			Social.InviteFriends(() => onComplete?.Invoke(true), onError);
		}

		public void JoinCommunity(Dictionary<string, object> options, Action<bool> onComplete = null) {}

		public void Rate(Action<bool> onComplete = null) {}

		public void Share(Dictionary<string, object> options, Action<bool> onComplete = null) {}
	}

	public class StorageVk : StorageBase
	{
		public StorageVk(float savePeriod) : base(savePeriod) { }

		public override void LoadData(string key, Action<string> successCallback, Action<string> errorCallback = null)
		{
			Storage.GetStorage(key, successCallback, errorCallback);
		}

		public override void SaveData(string key, string value, Action successCallback = null, Action<string> errorCallback = null)
		{
			Storage.SetStorage(key, value, successCallback, errorCallback);
		}
	}
}