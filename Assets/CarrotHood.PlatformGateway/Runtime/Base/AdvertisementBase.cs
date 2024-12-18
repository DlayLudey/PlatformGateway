using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AdvertisementBase
{
	public float interstitialCooldown { get; protected set; }
	
	private float _lastInterstitialTime;
	public bool isInterstitialAvailable => Time.realtimeSinceStartup - _lastInterstitialTime > interstitialCooldown;

	public AdvertisementBase(float platformInterstitialCooldown)
	{
		interstitialCooldown = platformInterstitialCooldown;
		_lastInterstitialTime = -interstitialCooldown;
	}

	public abstract void CheckAdBlock(Action<bool> callback);

	public abstract void ShowBanner(Dictionary<string, object> options = null);

	public abstract void HideBanner();

	public void ShowInterstitial(Action onOpen = null, Action onClose = null, Action<string> onError = null)
	{
		if (!isInterstitialAvailable)
		{
			onError?.Invoke($"Interstitial is not available yet, wait for {interstitialCooldown - (Time.realtimeSinceStartup - _lastInterstitialTime)}");
			return;
		}
		
		ShowInterstitialInternal(onOpen, () =>
		{
			_lastInterstitialTime = Time.realtimeSinceStartup;
			onClose?.Invoke();
		}, onError);
	}

	protected abstract void ShowInterstitialInternal(Action onOpen, Action onClose, Action<string> onError);

	public virtual bool NeedToPreloadRewarded => false;
	public virtual void PreloadRewarded(Action onSuccess, Action<string> onError = null) => onSuccess?.Invoke();
	public abstract void ShowRewarded(Action onRewarded, Action onOpened = null, Action onClosed = null, Action<string> onError = null);
}
