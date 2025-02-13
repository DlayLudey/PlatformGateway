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
		protected const string SaveLengthKey = "SaveLength";
		protected const string SaveKey = "Save";
		
		protected virtual int MaxSaveLength => 0;

		protected Dictionary<string, object> Data;

		protected string LastSavedJson;

		private float savePeriod;

		protected StorageBase(float savePeriod)
		{
			this.savePeriod = savePeriod;
		}

		public IEnumerator Initialize()
		{
			yield return GetPartialSave();
			
			PlatformGateway.Instance.StartCoroutine(SaveTimerCoroutine());
		}

		private IEnumerator GetPartialSave()
		{
			int saveLength = 0;
			
			LoadData(SaveLengthKey, s =>
			{
				if (!int.TryParse(s, out saveLength))
					saveLength = -1;
			}, s =>
			{
				saveLength = -1;
				Debug.LogError($"Load Save Error: {s}");
			});

			yield return new WaitUntil(() => saveLength != 0);

			if (saveLength == -1)
			{
				Data = new Dictionary<string, object>();
				yield break;
			}

			string[] jsonParts = new string[saveLength];
			
			for (int i = 0; i < saveLength; i++)
			{
				bool gotError = false;
				bool gotSave = false;
				int saveIndex = i;
				
				LoadData(SaveKey + i, s =>
				{
					jsonParts[saveIndex] = s;
					gotSave = true;
				}, s =>
				{
					Debug.LogError(s);
					gotError = true;
				});
				
				yield return new WaitUntil(() => gotSave || gotError);

				if (!gotError) continue;
				
				Data = new Dictionary<string, object>();
				yield break;
			}
			
			string json = string.Join("", jsonParts);
			
			if(string.IsNullOrEmpty(json))
			{
				Data = new Dictionary<string, object>();
				yield break;
			}
			
			Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
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
		public void SaveStoredData()
		{
			string json = JsonConvert.SerializeObject(Data);
			
			if(json == LastSavedJson)
				return;
			
			LastSavedJson = json;
			
			List<string> saveParts = new List<string>();

			if (MaxSaveLength == 0)
				saveParts.Add(json);
			else
			{
				for (int i = 0; i < json.Length / MaxSaveLength + 1; i++)
				{
					int startIndex = i * MaxSaveLength;
					saveParts.Add(
						MaxSaveLength <= json.Length - startIndex
							? json.Substring(startIndex, MaxSaveLength)
							: json.Substring(startIndex));
				}
			}

			SaveData(SaveLengthKey, saveParts.Count.ToString());
			
			for (int i = 0; i < saveParts.Count; i++)
			{
				SaveData(SaveKey + i, saveParts[i], errorCallback: s => Debug.LogError($"SaveError: {s}"));
			}
		}

		public IEnumerator ClearData()
		{
			int saveLength = 0;
			
			LoadData(SaveLengthKey, s =>
			{
				if (!int.TryParse(s, out saveLength))
					saveLength = -1;
			}, s =>
			{
				saveLength = -1;
				Debug.LogError($"Load Save Error: {s}");
			});

			yield return new WaitUntil(() => saveLength != 0);
			
			for (int i = 0; i < saveLength; i++)
			{
				SaveData(SaveKey + i, "");
			}
			
			SaveData(SaveLengthKey, "");
			
			Data = new Dictionary<string, object>();
		}

		public abstract void LoadData(string key, Action<string> successCallback, Action<string> errorCallback = null);

		public abstract void SaveData(string key, string value, Action successCallback = null, Action<string> errorCallback = null);
	}
}