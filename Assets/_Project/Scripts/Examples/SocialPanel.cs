using System.Collections.Generic;
using CarrotHood.PlatformGateway;
using UnityEngine;
using UnityEngine.UI;

namespace Examples
{
    public class SocialPanel : MonoBehaviour
    {
        [SerializeField] private Text _isShareSupported;
        [SerializeField] private Text _isInviteFriendsSupported;
        [SerializeField] private Text _isJoinCommunitySupported;
        [SerializeField] private Text _isAddToFavoritesSupported;
        [SerializeField] private Text _isAddToHomeScreenSupported;
        [SerializeField] private Text _isCreatePostSupported;
        [SerializeField] private Text _isRateSupported;
        [SerializeField] private Text _isExternalLinksAllowed;
        [SerializeField] private Button _shareButton;
        [SerializeField] private Button _inviteFriendsButton;
        [SerializeField] private Button _joinCommunityButton;
        [SerializeField] private Button _addToFavoritesButton;
        [SerializeField] private Button _addToHomeScreenButton;
        [SerializeField] private Button _createPostButton;
        [SerializeField] private Button _rateButton;
        [SerializeField] private GameObject _overlay;

        private ISocial Social => PlatformGateway.Social;

        private void Start()
        {
            _isShareSupported.text = $"Is Share Supported: { Social.isShareSupported }";
            _isInviteFriendsSupported.text = $"Is Invite Friends Supported: {Social.isInviteFriendsSupported }";
            _isJoinCommunitySupported.text = $"Is Join Community Supported: {Social.isJoinCommunitySupported }";
            _isAddToFavoritesSupported.text = $"Is Add To Favorites Supported: {Social.isAddToFavoritesSupported }";
            _isAddToHomeScreenSupported.text = $"Is Add To Home Screen Supported: {Social.isAddToHomeScreenSupported }";
            _isCreatePostSupported.text = $"Is Create Post Supported: {Social.isCreatePostSupported }";
            _isRateSupported.text = $"Is Rate Supported: {Social.isRateSupported }";
            _isExternalLinksAllowed.text = $"Is External Links Allowed: {Social.isExternalLinksAllowed }";

            _shareButton.onClick.AddListener(OnShareButtonClicked);
            _inviteFriendsButton.onClick.AddListener(OnInviteFriendsButtonClicked);
            _joinCommunityButton.onClick.AddListener(OnJoinCommunityButtonClicked);
            _addToFavoritesButton.onClick.AddListener(OnAddToFavoritesButtonClicked);
            _addToHomeScreenButton.onClick.AddListener(OnAddToHomeScreenButtonClicked);
            _createPostButton.onClick.AddListener(OnCreatePostButtonClicked);
            _rateButton.onClick.AddListener(OnRateButtonClicked);
        }

        private void OnShareButtonClicked()
        {
            _overlay.SetActive(true);

            var options = new Dictionary<string, object>();
            
            //switch (Bridge.platform.id)
            //{
            //    case "vk":
            //        options.Add("link", "https://vk.com/mewton.games");
            //        break;
            //}

			Social.Share(options, _ => { _overlay.SetActive(false); });
        }

        private void OnInviteFriendsButtonClicked()
        {
            _overlay.SetActive(true);
            
            var options = new Dictionary<string, object>();
            
            //switch (Bridge.platform.id)
            //{
            //    case "ok":
            //        options.Add("text", "Hello World!");
            //        break;
            //}

			Social.InviteFriends("Hello World!", _ => { _overlay.SetActive(false); });
        }

        private void OnJoinCommunityButtonClicked()
        {
            _overlay.SetActive(true);

            var options = new Dictionary<string, object>();
            
            //switch (Bridge.platform.id)
            //{
            //    case "vk":
            //        options.Add("groupId", 199747461);
            //        break;
            //    case "ok":
            //        options.Add("groupId", 62984239710374);
            //        break;
            //}

			Social.JoinCommunity(options, _ => { _overlay.SetActive(false); });
        }

        private void OnAddToFavoritesButtonClicked()
        {
            _overlay.SetActive(true);
			Social.AddToFavorites(_ => { _overlay.SetActive(false); });
        }

        private void OnAddToHomeScreenButtonClicked()
        {
            _overlay.SetActive(true);
			Social.AddToHomeScreen(_ => { _overlay.SetActive(false); });
        }

        private void OnCreatePostButtonClicked()
        {
            _overlay.SetActive(true);
            
            var options = new Dictionary<string, object>();
            switch (PlatformGateway.PlatformType)
            {
                case PlatformType.Vk:
                    options.Add("message", "Hello World!");
                    options.Add("attachments", "photo-199747461_457239629");
                    break;

                case PlatformType.Ok:
                    var media = new object[]
                    {
                        new Dictionary<string, object>
                        {
                            { "type", "text" },
                            { "text", "Hello World!" },
                        },
                        new Dictionary<string, object>
                        {
                            { "type", "link" },
                            { "url", "https://apiok.ru" },
                        },
                        new Dictionary<string, object>
                        {
                            { "type", "poll" },
                            { "question", "Do you like our API?" },
                            {
                                "answers",
                                new object[]
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "text", "Yes" },
                                    },
                                    new Dictionary<string, object>
                                    {
                                        { "text", "No" },
                                    }
                                }
                            },
                            { "options", "SingleChoice,AnonymousVoting" },
                        },
                    };

                    options.Add("media", media);
                    break;
            }

            Social.CreatePost(options, _ => { _overlay.SetActive(false); });
        }

        private void OnRateButtonClicked()
        {
            _overlay.SetActive(true);
			Social.Rate(_ => { _overlay.SetActive(false); });
        }
    }
}