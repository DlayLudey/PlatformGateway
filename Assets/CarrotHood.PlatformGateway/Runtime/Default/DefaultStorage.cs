using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace CarrotHood.PlatformGateway
{
	public class DefaultStorage : StorageBase
	{
		public DefaultStorage(float savePeriod) : base(savePeriod) { }

		public override void LoadData(string key, Action<string> successCallback, Action<string> errorCallback = null)
		{
			successCallback?.Invoke(PlayerPrefs.GetString(key));
		}

		public override void SaveData(string key, string value, Action successCallback = null, Action<string> errorCallback = null)
		{
			PlayerPrefs.SetString(key, value);
			successCallback?.Invoke();
		}
	}
}

