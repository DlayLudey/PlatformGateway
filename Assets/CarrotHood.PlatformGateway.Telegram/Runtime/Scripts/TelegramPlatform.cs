using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarrotHood.PlatformGateway.Telegram
{
    [CreateAssetMenu(fileName = "TelegramPlatform", menuName = "Platforms/Telegram")]
    public class TelegramPlatform : Platform
    {
        public override PlatformType Type => PlatformType.Telegram;
        public override string Language => PlayerAccount.UserInfo == null ? "ru" : PlayerAccount.UserInfo.languageCode;

        public override IEnumerator Init(PlatformBuilder builder)
        {
            yield return base.Init(builder);

            yield return TelegramSdk.Initialize();
            yield return PlayerAccount.GetUserInfo();

            builder.AddAdvertisement(new AdvertisementTelegram(Settings.interstitialCooldown));
            builder.AddStorage(new StorageTelegram());
            
            builder.AddSocial(new SocialTelegram());
            builder.AddPayments(new PaymentsTelegram(Settings.products));
        }
    }

    public class AdvertisementTelegram : AdvertisementBase
    {
        public AdvertisementTelegram(int platformInterstitialCooldown) : base(platformInterstitialCooldown) { }

        public override void CheckAdBlock(Action<bool> callback) => callback?.Invoke(false);

        public override void ShowBanner(Dictionary<string, object> options = null) { }
        
        public override void HideBanner() { }

        protected override void ShowInterstitialInternal(Action onOpen, Action onClose, Action<string> onError)
        {
            Advertisement.ShowInterstitialAd(onOpen, onClose, onError);
        }

        public override void ShowRewarded(Action onRewarded, Action onOpened = null, Action<string> onError = null)
        {
            Advertisement.ShowRewardedAd(onRewarded, onOpened, onError);
        }
    }

    public class StorageTelegram : IStorage
    {
        public void GetValue(string key, Action<string> onSuccess, Action<string> onError = null)
        {
            Storage.GetStorageValue(key, onSuccess, onError);
        }

        public void SetValue(string key, string value, Action onSuccess = null, Action<string> onError = null)
        {
            Storage.SetStorageValue(key, value, onSuccess, onError);
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

        public void InviteFriends(string inviteText, Action<int> onComplete = null, Action<string> onError = null) { }

        public void JoinCommunity(Dictionary<string, object> options, Action<bool> onComplete = null) { }

        public void Rate(Action<bool> onComplete = null) { }

        public void Share(Dictionary<string, object> options, Action<bool> onComplete = null) { }
    }

    public class PaymentsTelegram : IPayments
    {
        public Product[] Products { get; }
        public string CurrencyName => "TON";
        public Sprite CurrencySprite { get; }
        public bool paymentsSupported => false;
        public bool consummationSupported => false;

        public PaymentsTelegram(Product[] products)
        {
            Products = products;
            CurrencySprite = Resources.Load<Sprite>("PlatformGateway/CurrencyIcons/Telegram");
        }

        public void ConsumePurchase(string productToken, Action onSuccessCallback = null, Action<string> onErrorCallback = null) { }

        public void GetPurchases(Action<PurchasedProduct[]> onSuccessCallback, Action<string> onErrorCallback = null) { }

        public void Purchase(string productId, Action<PurchasedProduct?> onSuccessCallback = null, Action<string> onErrorCallback = null) { }
    }
}