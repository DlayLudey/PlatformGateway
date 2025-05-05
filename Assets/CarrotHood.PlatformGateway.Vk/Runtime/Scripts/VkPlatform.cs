using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CarrotHood.PlatformGateway.Vk;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace CarrotHood.PlatformGateway.Vk
{
	[CreateAssetMenu(fileName = "VkPlatform", menuName = "Platforms/Vk")]
	public class VkPlatform : PlatformBase
	{
		[SerializeField] private Product[] products;
		[SerializeField] private int appId;
		
		private LaunchParams _launchParams;
		private string authToken;
		
		public override PlatformType Type => PlatformType.Vk;

		public override string Language => _launchParams.language;
		
		private GameFocusManager gameFocusManager;
		
		public override IEnumerator Init(PlatformBuilder builder)
		{
			yield return VkSdk.Initialize();

			yield return GetLaunchParams();

			VkSdk.GetAuthToken(appId, token =>
			{
				authToken = token;
			}, Debug.LogError);
			
			yield return new WaitUntil(() => !string.IsNullOrEmpty(authToken));
			
			builder.Storage = new StorageVk(authToken, _launchParams.userId, saveCooldown);
			
			gameFocusManager = new GameFocusManager();

			yield return builder.Storage.Initialize();
			
			builder.Payments = new PaymentsVk(gameFocusManager, Language, builder.Storage);
			
			builder.Advertisement = new AdvertisementVk(gameFocusManager, interstitialCooldown);
			builder.Social = new SocialVk();

			WebApplication.Initialize(b => gameFocusManager.InBackground = !b);
			gameFocusManager.OnGameFocusChanged += b => OnGameFocusChanged?.Invoke(b);
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
		
		#if UNITY_EDITOR
		[ContextMenu("Export Products")]
		public void ExportProducts()
		{
			if (products == null || !products.Any())
			{
				products = new Product[]{};
			}
			
			string productJson = JsonUtility.ToJson(new ExportJson(products));

			string path = EditorUtility.SaveFilePanel("Products", "", "products", "json");

			if (string.IsNullOrEmpty(path))
				return;

			File.WriteAllText(path, productJson);
		}
		
		[Serializable]
		private struct ExportProduct
		{
			public string productId;
			public string name;
			public string photoUrl;
			public int price;

			public ExportProduct(Product product)
			{
				productId = product.productId;
				name = product.name;
				photoUrl = "";
				price = product.price;
			}
		}
		
		[Serializable]
		private struct ExportJson
		{
			public ExportProduct[] products;

			public ExportJson(Product[] products)
			{
				this.products = products.Select(x => new ExportProduct(x)).ToArray();
			}
		}
		#endif
	}

	public class PaymentsVk : PaymentsBase
	{
		public PaymentsVk(GameFocusManager gameFocusManager, string language, StorageBase storageBase) : base(storageBase)
		{
			CurrencyName = language == "en" ? "Votes" : "Голос";
			this.gameFocusManager = gameFocusManager;
		}

		public override Product[] Products { get; protected set; } = new Product[]{};
		public override string CurrencyName { get; protected set; }
		public override Sprite CurrencySprite { get; protected set; } = Resources.Load<Sprite>("PlatformGateway/CurrencyIcons/Vk");
		public override bool PaymentsSupported => true;
		public override bool ConsummationSupported => false;

		private GameFocusManager gameFocusManager;

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
			gameFocusManager.InPayments = true;
			
			Billing.Purchase(productId, () =>
			{
				gameFocusManager.InPayments = false;
				onSuccessCallback?.Invoke(null);
			}, s =>
			{
				gameFocusManager.InPayments = false;
				onErrorCallback?.Invoke(s);
			});
		}
	}

	public class AdvertisementVk : AdvertisementBase
	{
		public AdvertisementVk(GameFocusManager gameFocusManager, float platformInterstitialCooldown) : base(platformInterstitialCooldown)
		{
			this.gameFocusManager = gameFocusManager;
		}

		protected GameFocusManager gameFocusManager;
		
		public override void CheckAdBlock(Action<bool> callback) => callback?.Invoke(false);

		public override void ShowBanner(Dictionary<string, object> options = null) { }

		public override void HideBanner() { }

		protected override void ShowInterstitialInternal(Action onOpen, Action onClose, Action<string> onError)
		{
			onOpen?.Invoke();
			
			gameFocusManager.InAdvert = true;
			
			Advertisement.ShowInterstitialAd(() =>
			{
				gameFocusManager.InAdvert = false;
				onClose?.Invoke();
			}, s =>
			{
				gameFocusManager.InAdvert = false;
				onError?.Invoke(s);
			});
		}

		public override void ShowRewarded(Action onRewarded, Action onOpen = null, Action onClosed = null, Action<string> onError = null)
		{
			onOpen?.Invoke();
			
			gameFocusManager.InAdvert = true;
			
			Advertisement.ShowRewardedAd(() =>
			{
				gameFocusManager.InAdvert = false;
				onRewarded?.Invoke();
				onClosed?.Invoke();
			}, s =>
			{
				gameFocusManager.InAdvert = false;
				onError?.Invoke(s);
			});
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
		public StorageVk(string token, int userId, float savePeriod) : base(savePeriod)
		{
			PartialStorage.Initialize(token, userId);
		}

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