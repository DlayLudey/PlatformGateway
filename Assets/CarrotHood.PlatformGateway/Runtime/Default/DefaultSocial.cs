using System;
using System.Collections.Generic;

namespace CarrotHood.PlatformGateway
{
	public class DefaultSocial : ISocial
	{
		public bool isAddToFavoritesSupported { get; } = true;
		public bool isAddToHomeScreenSupported { get; } = true;
		public bool isCreatePostSupported { get; } = true;
		public bool isExternalLinksAllowed { get; } = true;
		public bool isInviteFriendsSupported { get; } = true;
		public bool isJoinCommunitySupported { get; } = true;
		public bool isRateSupported { get; } = true;
		public bool isShareSupported { get; } = true;
		public void AddToFavorites(Action<bool> onComplete = null) { }
		public void AddToHomeScreen(Action<bool> onComplete = null) { }
		public void CreatePost(Dictionary<string, object> options, Action<bool> onComplete = null) { }
		public void InviteFriends(string inviteText, Action<bool> onComplete = null, Action<string> onError = null) { }
		public void JoinCommunity(Dictionary<string, object> options, Action<bool> onComplete = null) { }
		public void Rate(Action<bool> onComplete = null) { }
		public void Share(Dictionary<string, object> options, Action<bool> onComplete = null) { }
	}
}
