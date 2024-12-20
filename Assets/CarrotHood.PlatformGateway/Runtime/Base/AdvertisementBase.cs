using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AdvertisementBase
{
	public float interstitialCooldown { get; protected set; }
	
	protected float lastInterstitialTime;
	public bool isInterstitialAvailable => Time.realtimeSinceStartup - lastInterstitialTime > interstitialCooldown;

	public AdvertisementBase(float platformInterstitialCooldown)
	{
		interstitialCooldown = platformInterstitialCooldown;
		lastInterstitialTime = -interstitialCooldown;
	}

	public abstract void CheckAdBlock(Action<bool> callback);

	public abstract void ShowBanner(Dictionary<string, object> options = null);

	public abstract void HideBanner();

	public void ShowInterstitial(Action onOpen = null, Action onClose = null, Action<string> onError = null)
	{
		if (!isInterstitialAvailable)
		{
			onError?.Invoke($"Interstitial is not available yet, wait for {interstitialCooldown - (Time.realtimeSinceStartup - lastInterstitialTime)}");
			return;
		}
		
		ShowInterstitialInternal(onOpen, () =>
		{
			lastInterstitialTime = Time.realtimeSinceStartup;
			onClose?.Invoke();
		}, s =>
		{
			lastInterstitialTime = Time.realtimeSinceStartup;
			onError?.Invoke(s);
		});
	}

	protected abstract void ShowInterstitialInternal(Action onOpen, Action onClose, Action<string> onError);

	public virtual bool NeedToPreloadRewarded => false;
	public virtual void PreloadRewarded(Action onSuccess, Action<string> onError = null) => onSuccess?.Invoke();
	public abstract void ShowRewarded(Action onRewarded, Action onOpened = null, Action onClosed = null, Action<string> onError = null);
}
