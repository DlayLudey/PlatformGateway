using System;
using System.Collections.Generic;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Advertisement;

namespace CarrotHood.PlatformDeps
{
	public class DefaultAdvertisement : IAdvertisement
	{
		private AdvertisementModule Ads => Bridge.advertisement;

		public event Action<BannerState> bannerStateChanged;
		public event Action<InterstitialState> interstitialStateChanged;
		public event Action<RewardedState> rewardedStateChanged;

		public BannerState bannerState => (BannerState)Ads.bannerState;
		public InterstitialState interstitialState => (InterstitialState)Ads.interstitialState;
		public RewardedState rewardedState => (RewardedState)Ads.rewardedState;

		public int minimumDelayBetweenInterstitial => Ads.minimumDelayBetweenInterstitial;
		public bool isBannerSupported => Ads.isBannerSupported;

		public DefaultAdvertisement()
		{
			Ads.bannerStateChanged += Ads_bannerStateChanged;
			Ads.interstitialStateChanged += Ads_interstitialStateChanged;
			Ads.rewardedStateChanged += Ads_rewardedStateChanged; 
		}

		private void Ads_rewardedStateChanged(InstantGamesBridge.Modules.Advertisement.RewardedState obj)
					 => rewardedStateChanged?.Invoke((RewardedState)obj);
		
		private void Ads_interstitialStateChanged(InstantGamesBridge.Modules.Advertisement.InterstitialState obj)
					 =>	interstitialStateChanged?.Invoke((InterstitialState)obj);
		
		private void Ads_bannerStateChanged(InstantGamesBridge.Modules.Advertisement.BannerState obj)
					 => bannerStateChanged?.Invoke((BannerState)obj);
		
		public void CheckAdBlock(Action<bool> callback) => Ads.CheckAdBlock(callback);

		public void HideBanner() => Ads.HideBanner();

		public void SetMinimumDelayBetweenInterstitial(int seconds) => Ads.SetMinimumDelayBetweenInterstitial(seconds);

		public void ShowBanner(Dictionary<string, object> options = null) => Ads.ShowBanner(options);

		public void ShowInterstitial() => Ads.ShowInterstitial();

		public void ShowRewarded() => Ads.ShowRewarded();
	}
}

