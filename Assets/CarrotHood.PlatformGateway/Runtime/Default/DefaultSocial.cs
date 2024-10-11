using System;
using System.Collections.Generic;

namespace CarrotHood.PlatformGateway
{
	public class DefaultSocial : ISocial
	{
		public bool isAddToFavoritesSupported { get; } = false;
		public bool isAddToHomeScreenSupported { get; } = false;
		public bool isCreatePostSupported { get; } = false;
		public bool isExternalLinksAllowed { get; } = false;
		public bool isInviteFriendsSupported { get; } = false;
		public bool isJoinCommunitySupported { get; } = false;
		public bool isRateSupported { get; } = false;
		public bool isShareSupported { get; } = false;
		public void AddToFavorites(Action<bool> onComplete = null) { }
		public void AddToHomeScreen(Action<bool> onComplete = null) { }
		public void CreatePost(Dictionary<string, object> options, Action<bool> onComplete = null) { }
		public void InviteFriends(string inviteText, Action<int> onComplete = null, Action<string> onError = null) { }
		public void JoinCommunity(Dictionary<string, object> options, Action<bool> onComplete = null) { }
		public void Rate(Action<bool> onComplete = null) { }
		public void Share(Dictionary<string, object> options, Action<bool> onComplete = null) { }
	}
}
