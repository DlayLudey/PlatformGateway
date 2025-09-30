using System;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.UI;

public class GameAnalyticsPanel : MonoBehaviour
{
    [SerializeField] private Text initializedText;
    [SerializeField] private Text rcReadyText;
    [SerializeField] private Text rcStringText;
    [SerializeField] private Text rcJsonText;
    [SerializeField] private Button adEventButton;
    [SerializeField] private Button designEventButton;
    [SerializeField] private Button progressEventButton;

    private void Awake()
    {
        GameAnalytics.Initialize();
        adEventButton.onClick.AddListener(AdEvent);
        designEventButton.onClick.AddListener(DesignEvent);
        progressEventButton.onClick.AddListener(ProgressEvent);
    }

    private void Update()
    {
        initializedText.text = $"Initialized: {GameAnalytics.Initialized}";
        rcReadyText.text = $"RemoteConfigsReady: {GameAnalytics.IsRemoteConfigsReady()}";
        rcStringText.text = $"RemoteConfigsString: {GameAnalytics.GetRemoteConfigsContentAsString()}";
        rcJsonText.text = $"RemoteConfigsJSON: {GameAnalytics.GetRemoteConfigsContentAsJSON()}";
    }

    private void AdEvent()
    {
        GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Interstitial, "adSdkName", "adPlacement");
    }

    private void DesignEvent()
    {
        GameAnalytics.NewDesignEvent("DesignEvent:AAA:BBB");
    }

    private void ProgressEvent()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Chapter", "Level");
    }
}
