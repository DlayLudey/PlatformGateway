using System;

namespace InstantGamesBridge.Modules.Platform
{
	public interface IPlatform
	{
		string language { get; }
		string payload { get; }
		string tld { get; }

		void GetServerTime(Action<DateTime?> callback);
		void SendMessage(PlatformMessage message);
	}
}