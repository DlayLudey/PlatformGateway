using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarrotHood.PlatformGateway.Telegram
{
    [CreateAssetMenu(fileName = "TelegramPlatform", menuName = "Platforms/Telegram")]
    public class TelegramPlatform : PlatformBase
    {
        [SerializeField] protected Product[] products;
        
        public override PlatformType Type => PlatformType.Telegram;
        public override string Language => PlayerAccount.userInfo == null ? "ru" : PlayerAccount.userInfo.languageCode;

        public override IEnumerator Init(PlatformBuilder builder)
        {
            yield return TelegramSdk.Initialize();

            builder.Account = new AccountTelegram();
            
            builder.Advertisement = new AdvertisementTelegram(interstitialCooldown);

            builder.Storage = new StorageTelegram(saveCooldown);
            
            yield return builder.Storage.Initialize();

            builder.Payments = new PaymentsTelegram(products, builder.Storage);
            builder.Advertisement = new AdvertisementTelegram(interstitialCooldown);

            builder.Social = new SocialTelegram();
        }

        public override void GameReady()
        {
            TelegramSdk.GameReady();
        }
    }
    
    public class AccountTelegram : IAccount
    {
        public IAccount.PlayerData Player { get; private set; }
        
        public IEnumerator GetPlayerData()
        {
            yield return PlayerAccount.Initialize();

            Player = new IAccount.PlayerData
            {
                name = $"{PlayerAccount.userInfo.firstName} {PlayerAccount.userInfo.lastName}",
                id = PlayerAccount.userInfo.id,
                profilePictureUrl = PlayerAccount.userInfo.photoUrl
            };
        }
    }

    public class AdvertisementTelegram : AdvertisementBase
    {
        public AdvertisementTelegram(float platformInterstitialCooldown) : base(platformInterstitialCooldown) { }

        public override void CheckAdBlock(Action<bool> callback) => callback?.Invoke(false);

        public override void ShowBanner(Dictionary<string, object> options = null) { }
        
        public override void HideBanner() { }

        protected override void ShowInterstitialInternal(Action onOpen, Action onClose, Action<string> onError)
        {
            Advertisement.ShowInterstitialAd(onOpen, onClose, onError);
        }

        public override void ShowRewarded(Action onRewarded, Action onOpen = null, Action onClose = null, Action<string> onError = null)
        {
            onOpen?.Invoke();
            Advertisement.ShowRewardedAd(onOpen, onRewarded, onClose, onError);
        }
    }

    public class StorageTelegram : StorageBase
    {
        public StorageTelegram(float savePeriod) : base(savePeriod) { }

        public override void LoadData(string key, Action<string> successCallback, Action<string> errorCallback = null)
        {
            Storage.GetCloudData(key, successCallback, errorCallback);
        }

        public override void SaveData(string key, string value, Action successCallback = null, Action<string> errorCallback = null)
        {
            Storage.SetCloudData(key, value, successCallback, errorCallback);
        }
    }

    public class SocialTelegram : ISocial
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

    public class PaymentsTelegram : PaymentsBase
    {
        public PaymentsTelegram(Product[] products, StorageBase storageBase) : base(storageBase)
        {
            Products = products;
        }
        
        public sealed override Product[] Products { get; protected set; }

        public override string CurrencyName { get; protected set; } = "TON";

        public override Sprite CurrencySprite { get; protected set; } = Resources.Load<Sprite>("PlatformGateway/CurrencyIcons/Telegram");

        public override bool PaymentsSupported { get; } = false;
        public override bool ConsummationSupported { get; } = false;
        
        protected override void InternalConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null) { }

        protected override void InternalGetPurchases(Action<PurchasedProduct[]> onSuccessCallback, Action<string> onErrorCallback = null) { }

        protected override void InternalPurchase(string productId, Action<PurchasedProduct?> onSuccessCallback = null, Action<string> onErrorCallback = null, string payload = null) { }
    }
}