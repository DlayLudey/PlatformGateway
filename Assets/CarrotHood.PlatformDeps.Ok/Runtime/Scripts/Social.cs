using System;
using System.Runtime.InteropServices;
using AOT;

namespace Qt.OkSdk
{
	public static class Social
	{
#region InviteFriends
		[DllImport("__Internal")]
		private static extern void ShowInvite(string inviteText, Action<int> onSuccess, Action<string> onError);

		private static Action<int> s_onInviteFriendsSuccess;
		private static Action<string> s_onInviteFriendsError;
		
		public static void InviteFriends(string inviteText, Action<int> onSuccess, Action<string> onError)
		{
			s_onInviteFriendsSuccess = onSuccess;
			s_onInviteFriendsError = onError;
			
			#if !UNITY_EDITOR
			ShowInvite(inviteText, OnShowInviteSuccess, OnShowInviteError);
			#else
			OnShowInviteSuccess(1);
			#endif
		}

		[MonoPInvokeCallback(typeof(Action<int>))]
		private static void OnShowInviteSuccess(int inviteAmount)
		{
			s_onInviteFriendsSuccess?.Invoke(inviteAmount);
		}
		
		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnShowInviteError(string error)
		{
			s_onInviteFriendsError?.Invoke(error);
		}
#endregion
	}
}
