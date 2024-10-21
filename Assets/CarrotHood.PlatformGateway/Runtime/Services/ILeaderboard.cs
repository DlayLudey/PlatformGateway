using System;
using System.Collections.Generic;

namespace CarrotHood.PlatformGateway
{
	public interface ILeaderboard
	{
		bool isGetEntriesSupported { get; }
		bool isGetScoreSupported { get; }
		bool isNativePopupSupported { get; }
		bool isSetScoreSupported { get; }
		bool isSupported { get; }

		void GetEntries(Dictionary<string, object> options, Action<bool, List<Dictionary<string, string>>> onComplete);
		void GetScore(Dictionary<string, object> options, Action<bool, int> onComplete);
		void SetScore(Dictionary<string, object> options, Action<bool> onComplete);
		void ShowNativePopup(Dictionary<string, object> options, Action<bool> onComplete = null);
	}
}