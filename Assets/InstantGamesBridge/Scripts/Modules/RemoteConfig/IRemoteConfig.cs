using System;
using System.Collections.Generic;

namespace InstantGamesBridge.Modules.RemoteConfig
{
	public interface IRemoteConfig
	{
		bool isSupported { get; }

		void Get(Dictionary<string, object> options, Action<bool, Dictionary<string, string>> onComplete);
	}
}