using System;
using UnityEngine;

namespace CarrotHood.PlatformGateway
{
	public class DefaultStorage : IStorage
	{
		public void GetValue(string key, Action<string> onSuccess, Action<string> onError)
		{
			onSuccess?.Invoke(PlayerPrefs.GetString(key));
		}

		public void SetValue(string key, string value, Action onSuccess, Action<string> onError)
		{
			PlayerPrefs.SetString(key, value);
			onSuccess?.Invoke();
		}
	}
}

