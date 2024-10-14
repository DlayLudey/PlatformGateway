using System;
using System.Collections.Generic;

namespace CarrotHood.PlatformGateway
{
	public interface IStorage
	{
		public void GetValue(string key, Action<string> onSuccess, Action<string> onError = null);
		public void SetValue(string key, string value, Action onSuccess = null, Action<string> onError = null);
	}
}