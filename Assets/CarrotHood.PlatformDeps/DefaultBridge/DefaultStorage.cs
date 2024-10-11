using System;
using UnityEngine;
using St = InstantGamesBridge.Modules.Storage.StorageType;

namespace CarrotHood.PlatformDeps
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

