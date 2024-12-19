using System;
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

		private StorageBase Storage => PlatformGateway.Storage;
		private PlayerData _playerData = new ();

		private void Start()
		{
			_setStorageDataButton.onClick.AddListener(OnSetStorageDataButtonClicked);
			_getStorageDataButton.onClick.AddListener(OnGetStorageDataButtonClicked);
		}

		private void OnSetStorageDataButtonClicked()
		{
			_playerData.coins = _coinsInput.text;
			_playerData.level = _levelInput.text;
			Storage.SetValue("Save", _playerData);
		}
		
		private void OnGetStorageDataButtonClicked()
		{
			_overlay.SetActive(true);
			
			_playerData = Storage.GetValue("Save", new PlayerData());

			_coinsInput.text = _playerData.coins;
			_levelInput.text = _playerData.level;
			
			_overlay.SetActive(false);
		}

		private class PlayerData
		{
			public string coins;
			public string level;
		}
	}
}