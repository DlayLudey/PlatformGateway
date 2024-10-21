using System;
using System.Collections.Generic;

namespace CarrotHood.PlatformGateway
{
	public class DefaultLeaderboard : ILeaderboard
	{
		public bool isGetEntriesSupported => false;

		public bool isGetScoreSupported => false;

		public bool isNativePopupSupported => false;

		public bool isSetScoreSupported => false;

		public bool isSupported => false;

		public void GetEntries(Dictionary<string, object> options, Action<bool, List<Dictionary<string, string>>> onComplete)
		{
			throw new NotImplementedException();
		}

		public void GetScore(Dictionary<string, object> options, Action<bool, int> onComplete)
		{
			throw new NotImplementedException();
		}

		public void SetScore(Dictionary<string, object> options, Action<bool> onComplete)
		{
			throw new NotImplementedException();
		}

		public void ShowNativePopup(Dictionary<string, object> options, Action<bool> onComplete = null)
		{
			throw new NotImplementedException();
		}
	}
}