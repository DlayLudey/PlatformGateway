

using System;
using System.Collections.Generic;

namespace CarrotHood.PlatformGateway
{
	internal class DefaultPlayer : IPlayer
	{
		public string id => throw new NotImplementedException();

		public bool isAuthorizationSupported => throw new NotImplementedException();

		public bool isAuthorized => throw new NotImplementedException();

		public string name => throw new NotImplementedException();

		public List<string> photos => throw new NotImplementedException();

		public void Authorize(Dictionary<string, object> options = null, Action<bool> onComplete = null)
		{
			throw new NotImplementedException();
		}
	}
}