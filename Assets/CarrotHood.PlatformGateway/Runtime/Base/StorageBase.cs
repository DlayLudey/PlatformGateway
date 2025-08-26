using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace CarrotHood.PlatformGateway
{
	public abstract class StorageBase
	{
		protected Dictionary<string, object> Data;

		protected string LastSavedJson;

		private float savePeriod;

		protected StorageBase(float savePeriod)
		{
			this.savePeriod = savePeriod;
		}

		public IEnumerator Initialize()
		{
			LoadData(nameof(Data),
				(data) =>
				{
					Data = !string.IsNullOrEmpty(data)
						? JsonConvert.DeserializeObject<Dictionary<string, object>>(data)
						: new Dictionary<string, object>();
				}, (error) =>
				{
					Debug.LogError(error);
					Data = new Dictionary<string, object>();
				});

			yield return new WaitUntil(() => Data != null);

			PlatformGateway.Instance.StartCoroutine(SaveTimerCoroutine());
		}

		private IEnumerator SaveTimerCoroutine()
		{
			while (Application.isPlaying)
			{
				yield return new WaitForSecondsRealtime(savePeriod);
				SaveStoredData();
			}
		}

		public T GetValue<T>(string key, T defaultValue)
		{
			if (!Data.TryGetValue(key, out object value)) return defaultValue;

			try
			{
				if (value is T convertedValue) return convertedValue;
				if (value != null && typeof(T).IsAssignableFrom(value.GetType())) return (T)value;
				return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(value));
			}
			catch (Exception exception)
			{
				Debug.LogError(exception);
			}

			return defaultValue;
		}

		public void SetValue<T>(string key, T value)
		{
			Data[key] = value;
		}

		/// <summary>
		/// Do not use this unless necessary, data automatically saves by this class
		/// </summary>
		public void SaveStoredData(Action onComplete = null, Action<string> onError = null)
		{
			string json = JsonConvert.SerializeObject(Data);
			
			if(json == LastSavedJson)
				return;
			
			LastSavedJson = json;
			
			SaveData(nameof(Data), JsonConvert.SerializeObject(Data), onComplete, onError);
		}

		public void ClearData(Action onComplete, Action<string> onError)
		{
			SaveData(nameof(Data), "", onComplete, onError);
		}

		public abstract void LoadData(string key, Action<string> successCallback, Action<string> errorCallback = null);

		public abstract void SaveData(string key, string value, Action successCallback = null, Action<string> errorCallback = null);
	}
}