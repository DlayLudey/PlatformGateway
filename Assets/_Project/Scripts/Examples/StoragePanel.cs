﻿using System;
using System.Collections.Generic;
using CarrotHood.PlatformGateway;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Examples
{
	public class StoragePanel : MonoBehaviour
	{
		[SerializeField] private Text _defaultTypeText;
		[SerializeField] private Text _isLocalStorageSupportedText;
		[SerializeField] private Text _isLocalStorageAvailableText;
		[SerializeField] private Text _isPlatformInternalSupportedText;
		[SerializeField] private Text _isPlatformInternalAvailableText;
		[SerializeField] private InputField _coinsInput;
		[SerializeField] private InputField _levelInput;
		[SerializeField] private Button _setStorageDataButton;
		[SerializeField] private Button _getStorageDataButton;
		[SerializeField] private Button _deleteStorageDataButton;
		[SerializeField] private GameObject _overlay;

		private const string _coinsKey = "coins";
		private const string _levelKey = "level";
		private IStorage Storage => PlatformGateway.Storage;

		private void Start()
		{
			//_isLocalStorageSupportedText.text = $"Is Local Storage Supported: { Storage.IsSupported(StorageType.LocalStorage) }";
			//_isPlatformInternalSupportedText.text = $"Is Platform Internal Supported: { Storage.IsSupported(StorageType.PlatformInternal) }";
			//_isLocalStorageAvailableText.text = $"Is Local Storage Available: { Storage.IsAvailable(StorageType.LocalStorage) }";
			//_isPlatformInternalAvailableText.text = $"Is Platform Internal Available: { Storage.IsAvailable(StorageType.PlatformInternal) }";

			_setStorageDataButton.onClick.AddListener(OnSetStorageDataButtonClicked);
			_getStorageDataButton.onClick.AddListener(OnGetStorageDataButtonClicked);
			//_deleteStorageDataButton.onClick.AddListener(OnDeleteStorageDataButtonClicked);
		}

		private void OnSetStorageDataButtonClicked()
		{
			_overlay.SetActive(true);

			Test test = new Test()
			{
				_coinsKey = _coinsInput.text,
				_levelKey = _levelInput.text

			};
			Storage.SetValue("save", JsonUtility.ToJson(test), () => _overlay.SetActive(false), (x) =>
			{
				Debug.Log($"Error: {x}");
				_overlay.SetActive(false);
			});
		}
		[Serializable]
		public struct Test
		{
			public string _coinsKey;
			public string _levelKey;
		}
		private void OnGetStorageDataButtonClicked()
		{
			_overlay.SetActive(true);

			var keys = new List<string> { _coinsKey, _levelKey };

			Storage.GetValue("save", (x) =>
			{
				Debug.Log(x);
				var test = JsonUtility.FromJson<Test>(x);
				_coinsInput.text = test._coinsKey;
				_levelInput.text = test._levelKey;
				_overlay.SetActive(false);
			}, (x) =>
			{
				Debug.Log($"Error: {x}");
				_overlay.SetActive(false);
			});
		}

		//private void OnDeleteStorageDataButtonClicked()
		//{
		//    _overlay.SetActive(true);

		//    var keys = new List<string> { _coinsKey, _levelKey };
		//    Storage.Delete(keys, success =>
		//    {
		//        _coinsInput.text = string.Empty;
		//        _levelInput.text = string.Empty;
		//        _overlay.SetActive(false);
		//    });
		//}
	}
}