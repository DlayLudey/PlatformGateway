using System;
using System.Collections.Generic;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Social;

namespace CarrotHood.PlatformDeps
{
	public class DefaultSocial : ISocial
	{
		SocialModule Social => Bridge.social;
		public bool isAddToFavoritesSupported => Social.isAddToFavoritesSupported;

		public bool isAddToHomeScreenSupported => Social.isAddToHomeScreenSupported;

		public bool isCreatePostSupported => Social.isCreatePostSupported;

		public bool isExternalLinksAllowed => Social.isExternalLinksAllowed;

		public bool isInviteFriendsSupported => Social.isInviteFriendsSupported;

		public bool isJoinCommunitySupported => Social.isJoinCommunitySupported;

		public bool isRateSupported => Social.isRateSupported;

		public bool isShareSupported => Social.isShareSupported;

		public void AddToFavorites(Action<bool> onComplete = null) 
			=> Social.AddToFavorites(onComplete);

		public void AddToHomeScreen(Action<bool> onComplete = null) 
			=> Social.AddToHomeScreen(onComplete);

		public void CreatePost(Dictionary<string, object> options, Action<bool> onComplete = null)
			=> Social.CreatePost(options, onComplete);

		public void InviteFriends(Dictionary<string, object> options, Action<bool> onComplete = null)
			=> Social.InviteFriends(options, onComplete);

		public void JoinCommunity(Dictionary<string, object> options, Action<bool> onComplete = null) 
			=> Social.JoinCommunity(options, onComplete);

		public void Rate(Action<bool> onComplete = null) 
			=> Social.Rate(onComplete);

		public void Share(Dictionary<string, object> options, Action<bool> onComplete = null)
			=> Social.Share(options, onComplete);
	}
}
