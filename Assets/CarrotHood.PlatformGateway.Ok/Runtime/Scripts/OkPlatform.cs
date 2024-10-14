﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Qt.OkSdk;
using UnityEngine;
using Social = Qt.OkSdk.Social;

namespace CarrotHood.PlatformGateway.Ok
{
	[CreateAssetMenu(fileName = "OkPlatform", menuName = "Platforms/Ok")]
	public partial class OkPlatform : Platform
	{
		[SerializeField] private PaymentsOk.Product[] products;

		public override PlatformType Type => PlatformType.OK;
		public override string Language => "ru";

		public override bool CheckRelevant()
		{
			return true;
		}

		public override IEnumerator Init(PlatformBuilder builder)
		{
			yield return OkSdk.Initialize();

			builder.AddPayments(new PaymentsOk(products));
			
			builder.AddAdvertisement(new AdvertisementOk(Settings.interstitialCooldown));
			
			builder.AddSocial(new SocialOk());
			
			builder.AddStorage(new StorageOk());
		}
	}

	//Здесь используем кастомный пакет
	public class PaymentsOk : IPayments
	{
		[Serializable]
		public struct Product
		{
			public string productId;
			public string name;
			public string description;
			public int price;
		}

		private readonly Product[] _products;
		
		public PaymentsOk(Product[] products)
		{
			_products = products;
		}
		
		public bool isSupported => true;

		public void ConsumePurchase(string productToken, Action onSuccessCallback = null,
			Action<string> onErrorCallback = null)
		{
			onSuccessCallback?.Invoke();
		}

		public void GetPurchases(Action<object> onSuccessCallback, Action<string> onErrorCallback = null)
		{
			onSuccessCallback?.Invoke(null);
		}

		public void Purchase(string productId, Action<object> onSuccessCallback = null, Action<string> onErrorCallback = null)
		{
			if (_products.All(x => x.productId != productId))
				throw new KeyNotFoundException($"There is no product with id: {productId}");
			
			Product product = _products.FirstOrDefault(x => x.productId == productId);
			
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
		public AdvertisementOk(int platformInterstitialCooldown) : base(platformInterstitialCooldown) { }

		public override void CheckAdBlock(Action<bool> callback) => callback?.Invoke(false);

		public override void ShowBanner(Dictionary<string, object> options = null) { }

		public override void HideBanner() { }

		protected override void ShowInterstitialInternal(Action onOpen, Action onClose, Action<string> onError)
		{
			Advertisement.ShowInterstitialAd(onOpen, onClose, onError);
		}

		public override void ShowRewarded(Action onRewarded, Action<string> onError)
		{
			Advertisement.ShowRewardedAd(onRewarded, onError);
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

		public void InviteFriends(string inviteText, Action<int> onComplete = null, Action<string> onError = null)
		{
			Social.InviteFriends(inviteText, onComplete, onError);
		}

		public void JoinCommunity(Dictionary<string, object> options, Action<bool> onComplete = null) { }

		public void Rate(Action<bool> onComplete = null) { }

		public void Share(Dictionary<string, object> options, Action<bool> onComplete = null) { }
	}

	public class StorageOk : IStorage
	{
		public void GetValue(string key, Action<string> onSuccess, Action<string> onError)
		{
			Storage.GetStorageValue(key, onSuccess, onError);
		}

		public void SetValue(string key, string value, Action onSuccess, Action<string> onError)
		{
			Storage.SetStorageValue(key, value, onSuccess, onError);
		}
	}
}