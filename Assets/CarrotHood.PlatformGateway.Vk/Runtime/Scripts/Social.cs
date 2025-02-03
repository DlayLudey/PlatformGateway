using System;
using System.Runtime.InteropServices;
using AOT;

namespace CarrotHood.PlatformGateway.Vk
{
	public static class Social
	{
		[DllImport("__Internal")]
		private static extern void VkInviteFriends(Action onSuccess, Action<string> onError);

		private static Action onInviteFriendsSuccess;
		private static Action<string> onInviteFriendsError;
		
		public static void InviteFriends(Action onSuccess, Action<string> onError)
		{
			onInviteFriendsSuccess = onSuccess;
			onInviteFriendsError = onError;
			
			#if !UNITY_EDITOR
			VkInviteFriends(OnInviteFriendsSuccess, OnInviteFriendsError);
			#else
			OnInviteFriendsSuccess();
			#endif
		}
		
		[MonoPInvokeCallback(typeof(Action))]
		private static void OnInviteFriendsSuccess()
		{
			onInviteFriendsSuccess?.Invoke();
		}
		
		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnInviteFriendsError(string error)
		{
			onInviteFriendsError?.Invoke(error);
		}
	}
}