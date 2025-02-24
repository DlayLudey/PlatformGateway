using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Qt.OkSdk;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif
using UnityEngine;
using Social = Qt.OkSdk.Social;

namespace CarrotHood.PlatformGateway.Ok
{
	[CreateAssetMenu(fileName = "OkPlatform", menuName = "Platforms/Ok")]
	public class OkPlatform : PlatformBase
	{
		[SerializeField] private Product[] products;
		[SerializeField] private StorageType storageType;
		
		private enum StorageType
		{
			Default,
			Partial
		}
		
		public override PlatformType Type => PlatformType.Ok;
		public override string Language => "ru";

		public override IEnumerator Init(PlatformBuilder builder)
		{
			yield return OkSdk.Initialize();

			if(storageType == StorageType.Default)
				builder.Storage = new StorageOk(saveCooldown);
			else
				builder.Storage = new PartialStorageOk(saveCooldown);
			
			yield return builder.Storage.Initialize();
			
			builder.Payments = new PaymentsOk(products, builder.Storage);

			builder.Advertisement = new AdvertisementOk(interstitialCooldown);
			builder.Social = new SocialOk();
		}
		
		#if UNITY_EDITOR
		[ContextMenu("Export Products")]
		public void ExportProducts()
		{
			if (products == null || !products.Any())
			{
				products = new Product[]{};
			}
			
			string productJson = JsonUtility.ToJson(new ExportProduct(products));

			string path = EditorUtility.SaveFilePanel("Products", "", "product", "json");

			if (string.IsNullOrEmpty(path))
				return;

			File.WriteAllText(path, productJson);
		}

		[Serializable]
		private struct ExportProduct
		{
			public Product[] products;

			public ExportProduct(Product[] products)
			{
				this.products = products;
			}
		}
		#endif
	}

	public class PaymentsOk : PaymentsBase
	{
		public PaymentsOk(Product[] products, StorageBase storageBase) : base(storageBase)
		{
			Products = products;
		}

		public override Product[] Products { get; protected set; }

		public override string CurrencyName { get; protected set; } = "OK";
		public override Sprite CurrencySprite { get; protected set; } = Resources.Load<Sprite>("PlatformGateway/CurrencyIcons/Ok");
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
			if (Products.All(x => x.productId != productId))
				throw new KeyNotFoundException($"There is no product with id: {productId}");
			
			Product product = Products.FirstOrDefault(x => x.productId == productId);
			
			Billing.Purchase(
				product.name, 
				product.description, 
				product.productId, 
				product.price, 
				() => onSuccessCallback?.Invoke(null), 
				onErrorCallback);
		}
	}

	public class AdvertisementOk : AdvertisementBase
	{
		public AdvertisementOk(float platformInterstitialCooldown) : base(platformInterstitialCooldown) { }

		public override void CheckAdBlock(Action<bool> callback) => callback?.Invoke(false);

		public override void ShowBanner(Dictionary<string, object> options = null) { }

		public override void HideBanner() { }

		protected override void ShowInterstitialInternal(Action onOpen, Action onClose, Action<string> onError)
		{
			Advertisement.ShowInterstitialAd(onOpen, onClose, onError);
		}

		public override bool NeedToPreloadRewarded => true;

		public override void PreloadRewarded(Action onSuccess, Action<string> onError = null)
		{
			Advertisement.LoadRewardedAd(onSuccess, onError);
		}
		
		public override void ShowRewarded(Action onRewarded, Action onOpen = null, Action onClose = null, Action<string> onError = null)
		{
			Advertisement.ShowRewardedAd(() =>
			{
				onOpen?.Invoke();
				onRewarded?.Invoke();
				onClose?.Invoke();
				
				if(isInterstitialAvailable)
					lastInterstitialTime = Time.realtimeSinceStartup - (interstitialCooldown / 4 * 3);
			}, onError);
		}
	}

	public class SocialOk : ISocial
	{
		public bool isAddToFavoritesSupported { get; } = false;
		public bool isAddToHomeScreenSupported { get; } = false;
		public bool isCreatePostSupported { get; } = false;
		public bool isExternalLinksAllowed { get; } = false;
		public bool isInviteFriendsSupported { get; } = true;
		public bool isJoinCommunitySupported { get; } = false;
		public bool isRateSupported { get; } = false;
		public bool isShareSupported { get; } = false;
		public void AddToFavorites(Action<bool> onComplete = null) { }

		public void AddToHomeScreen(Action<bool> onComplete = null) { }

		public void CreatePost(Dictionary<string, object> options, Action<bool> onComplete = null) { }
		public void InviteFriends(string inviteText, Action<bool> onComplete = null, Action<string> onError = null)
		{
			Social.InviteFriends(inviteText, (i) => onComplete?.Invoke(i > 0), onError);
		}

		public void JoinCommunity(Dictionary<string, object> options, Action<bool> onComplete = null) { }

		public void Rate(Action<bool> onComplete = null) { }

		public void Share(Dictionary<string, object> options, Action<bool> onComplete = null) { }
	}

	public class StorageOk : StorageBase
	{
		public StorageOk(float savePeriod) : base(savePeriod) { }

		public override void LoadData(string key, Action<string> successCallback, Action<string> errorCallback = null)
		{
			Storage.GetStorageValue(key, successCallback, errorCallback);
		}

		public override void SaveData(string key, string value, Action successCallback = null, Action<string> errorCallback = null)
		{
			Storage.SetStorageValue(key, value, successCallback, errorCallback);
		}
	}

	public class PartialStorageOk : StorageBase
	{
		public PartialStorageOk(float savePeriod) : base(savePeriod) { }
		
		public override void LoadData(string key, Action<string> successCallback, Action<string> errorCallback = null)
		{
			PartialStorage.GetPartialStorage(key, successCallback, errorCallback);
		}

		public override void SaveData(string key, string value, Action successCallback = null, Action<string> errorCallback = null)
		{
			PartialStorage.SetPartialStorage(key, value, successCallback, errorCallback);
		}
	}
}