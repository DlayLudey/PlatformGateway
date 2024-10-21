using System;
using System.Collections.Generic;

namespace CarrotHood.PlatformGateway
{
	public interface IPlayer
	{
		string id { get; }
		bool isAuthorizationSupported { get; }
		bool isAuthorized { get; }
		string name { get; }
		List<string> photos { get; }

		void Authorize(Dictionary<string, object> options = null, Action<bool> onComplete = null);
	}
}