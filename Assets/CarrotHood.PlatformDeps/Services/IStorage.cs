using System;
using System.Collections.Generic;

namespace CarrotHood.PlatformDeps
{
	public interface IStorage
	{
		public void GetValue(string key, Action<string> onSuccess, Action<string> onError);
		public void SetValue(string key, string value, Action onSuccess, Action<string> onError);
	}
}