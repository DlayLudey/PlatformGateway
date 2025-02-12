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
			Debug.Log($"DataLoad: {key}:{PlayerPrefs.GetString(key)}");
			
			successCallback?.Invoke(PlayerPrefs.GetString(key));
		}

		public override void SaveData(string key, string value, Action successCallback = null, Action<string> errorCallback = null)
		{
			Debug.Log($"DataSave: {key}:{value}");

			PlayerPrefs.SetString(key, value);
			successCallback?.Invoke();
		}
	}
}

