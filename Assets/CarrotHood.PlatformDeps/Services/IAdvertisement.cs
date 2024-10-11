using System;
using System.Collections.Generic;

namespace CarrotHood.PlatformDeps
{
	public interface IAdvertisement
	{
		BannerState bannerState { get; }
		InterstitialState interstitialState { get; }
		bool isBannerSupported { get; }
		int minimumDelayBetweenInterstitial { get; }
		RewardedState rewardedState { get; }

		event Action<BannerState> bannerStateChanged;
		event Action<InterstitialState> interstitialStateChanged;
		event Action<RewardedState> rewardedStateChanged;

		void CheckAdBlock(Action<bool> callback);
		void HideBanner();
		void SetMinimumDelayBetweenInterstitial(int seconds);
		void ShowBanner(Dictionary<string, object> options = null);
		void ShowInterstitial();
		void ShowRewarded();
	}

	public enum BannerState
	{
		Loading,
		Shown,
		Hidden,
		Failed
	}

	public enum InterstitialState
	{
		Loading,
		Opened,
		Closed,
		Failed
	}

	public enum RewardedState
	{
		Loading,
		Opened,
		Rewarded,
		Closed,
		Failed
	}
}