﻿using System.Collections.Generic;
using CarrotHood.PlatformGateway;
using UnityEngine;
using UnityEngine.UI;


namespace Examples
{
    public class AdvertisementPanel : MonoBehaviour
    {
        [SerializeField] private Text _bannerState;
        [SerializeField] private Text _interstitialState;
        [SerializeField] private Text _rewardedState;
        [SerializeField] private Text _bannerSupported;
        [SerializeField] private InputField _minimumDelayBetweenInterstitial;
        [SerializeField] private Button _setMinimumDelayBetweenInterstitialButton;
        [SerializeField] private Button _showBannerButton;
        [SerializeField] private Button _hideBannerButton;
        [SerializeField] private Button _showInterstitialButton;
        [SerializeField] private Button _showRewardedButton;
        [SerializeField] private Text _adBlockDetectedText;
        [SerializeField] private Button _checkAdBlockButton;
        [SerializeField] private GameObject _overlay;

        public AdvertisementBase Ads => PlatformGateway.Advertisement;

        private void Start()
        {
            _setMinimumDelayBetweenInterstitialButton.onClick.AddListener(OnSetMinimumDelayBetweenInterstitialButtonClicked);
            _showBannerButton.onClick.AddListener(OnShowBannerButtonClicked);
            _hideBannerButton.onClick.AddListener(OnHideBannerButtonClicked);
            _showInterstitialButton.onClick.AddListener(OnShowInterstitialButtonClicked);
            _showRewardedButton.onClick.AddListener(OnShowRewardedButtonClicked);
            _checkAdBlockButton.onClick.AddListener(OnCheckAdBlockButtonClicked);

           // _bannerSupported.text = $"Is Banner Supported: {Ads.isBannerSupported }";
        }


        private void OnSetMinimumDelayBetweenInterstitialButtonClicked()
        {
            int.TryParse(_minimumDelayBetweenInterstitial.text, out var seconds);
        }
        
        private void OnShowBannerButtonClicked()
        {
            var options = new Dictionary<string, object>();
            
            Ads.ShowBanner(options);
        }

        private void OnHideBannerButtonClicked()
        {
            Ads.HideBanner();
        }

        private void OnShowInterstitialButtonClicked()
        {
            Ads.ShowInterstitial(() =>
            {
                Debug.Log("Open");
            }, () =>
            {
                Debug.Log("Closed");
            }, Debug.LogError);
        }

        private void OnShowRewardedButtonClicked()
        {
            if(!Ads.NeedToPreloadRewarded)
                ShowRewarded();
            else
                Ads.PreloadRewarded(ShowRewarded, Debug.LogError);
        }

        private void ShowRewarded()
        {
            Ads.ShowRewarded(() =>
            {
                Debug.Log("Rewarded");
            }, () =>
            {
                Debug.Log("Open");
            }, () =>
            {
                Debug.Log("Closed");
            }, Debug.LogError);
        }

        private void OnCheckAdBlockButtonClicked()
        {
            _overlay.SetActive(true);
            Ads.CheckAdBlock(isAdBlockDetected =>
            {
                _adBlockDetectedText.text = $"AdBlock Detected: {isAdBlockDetected}";
                _overlay.SetActive(false);
            });
        }
    }
}