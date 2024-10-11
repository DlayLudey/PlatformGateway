using System;
using System.Collections.Generic;

namespace CarrotHood.PlatformDeps
{
	public interface ISocial
	{
		bool isAddToFavoritesSupported { get; }
		bool isAddToHomeScreenSupported { get; }
		bool isCreatePostSupported { get; }
		bool isExternalLinksAllowed { get; }
		bool isInviteFriendsSupported { get; }
		bool isJoinCommunitySupported { get; }
		bool isRateSupported { get; }
		bool isShareSupported { get; }

		void AddToFavorites(Action<bool> onComplete = null);
		void AddToHomeScreen(Action<bool> onComplete = null);
		void CreatePost(Dictionary<string, object> options, Action<bool> onComplete = null);
		void InviteFriends(Dictionary<string, object> options, Action<bool> onComplete = null);
		void JoinCommunity(Dictionary<string, object> options, Action<bool> onComplete = null);
		void Rate(Action<bool> onComplete = null);
		void Share(Dictionary<string, object> options, Action<bool> onComplete = null);
	}
}