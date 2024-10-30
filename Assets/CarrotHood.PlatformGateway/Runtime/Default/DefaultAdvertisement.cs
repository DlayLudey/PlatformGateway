using System;
using System.Collections.Generic;

namespace CarrotHood.PlatformGateway
{
	public class DefaultAdvertisement : AdvertisementBase
	{
		public DefaultAdvertisement(int platformInterstitialCooldown) : base(platformInterstitialCooldown) { }

		public override void CheckAdBlock(Action<bool> callback) => callback?.Invoke(false);

		public override void ShowBanner(Dictionary<string, object> options = null) { }

		public override void HideBanner() { }

		protected override void ShowInterstitialInternal(Action onOpen, Action onClose, Action<string> onError)
		{
			onOpen?.Invoke();
			onClose?.Invoke();
		}

		public override void ShowRewarded(Action onRewarded, Action onOpen = null, Action<string> onError = null)
		{
			onOpen?.Invoke();
			onRewarded?.Invoke();
		}
	}
}

