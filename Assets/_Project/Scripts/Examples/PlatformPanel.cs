using CarrotHood.PlatformGateway;
using UnityEngine;
using UnityEngine.UI;

namespace Examples
{
    public class PlatformPanel : MonoBehaviour
    {
        [SerializeField] private Text _id;
        [SerializeField] private Text _language;
        [SerializeField] private Text _payload;
        [SerializeField] private Text _tld;
        [SerializeField] private Button _sendGameReadyMessageButton;
        [SerializeField] private Button _sendInGameLoadingStartedMessageButton;
        [SerializeField] private Button _sendInGameLoadingStoppedMessageButton;
        [SerializeField] private Button _sendGameplayStartedMessageButton;
        [SerializeField] private Button _sendGameplayStoppedMessageButton;
        [SerializeField] private Button _sendPlayerGotAchievementMessageButton;
        [SerializeField] private Text _serverTimeText;
        [SerializeField] private Button _getServerTimeButton;
        [SerializeField] private GameObject _overlay;

        private void Start()
        {
            _id.text = $"ID: {PlatformGateway.PlatformType}";

            PlatformGateway.CurrentPlatform.GameReady();
            PlatformGateway.CurrentPlatform.OnGameFocusChanged += b => Debug.Log($"Focus: {b}");
        }
    }
}