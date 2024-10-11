using System;
using System.Collections.Generic;

namespace CarrotHood.PlatformGateway
{
	public interface IAdvertisement
	{
		int interstitialCooldown { get; }

		void CheckAdBlock(Action<bool> callback);
		void ShowBanner(Dictionary<string, object> options = null);
		void HideBanner();
		void ShowInterstitial(Action onOpen = null, Action onClose = null, Action<string> onError = null);
		void ShowRewarded(Action onRewarded, Action<string> onError = null);
	}
}