using System;
using System.Collections.Generic;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Storage;
using St = InstantGamesBridge.Modules.Storage.StorageType;

namespace CarrotHood.PlatformDeps
{
	public class DefaultStorage : IStorage
	{
		private StorageModule Storage => Bridge.storage;
		public StorageType defaultType => (StorageType)Storage.defaultType;

		public void Delete(List<string> keys, Action<bool> onComplete = null, StorageType? storageType = null)
			=> Storage.Delete(keys, onComplete, (St)storageType);

		public void Delete(string key, Action<bool> onComplete = null, StorageType? storageType = null)
			=> Storage.Delete(key, onComplete, (St)storageType);

		public void Get(List<string> keys, Action<bool, List<string>> onComplete, StorageType? storageType = null)
			=> Storage.Get(keys, onComplete, (St)storageType);

		public void Get(string key, Action<bool, string> onComplete, StorageType? storageType = null)
			=> Storage.Get(key, onComplete, (St)storageType);

		public bool IsAvailable(StorageType storageType)
			=> Storage.IsAvailable((St)storageType);

		public bool IsSupported(StorageType storageType)
			=> Storage.IsSupported((St)storageType);

		public void Set(List<string> keys, List<object> values, Action<bool> onComplete = null, StorageType? storageType = null)
			=> Storage.Set(keys, values, onComplete, (St)storageType);

		public void Set(string key, bool value, Action<bool> onComplete = null, StorageType? storageType = null)
			=> Storage.Set(key, value, onComplete, (St)storageType);

		public void Set(string key, int value, Action<bool> onComplete = null, StorageType? storageType = null)
			=> Storage.Set(key, value, onComplete, (St)storageType);

		public void Set(string key, string value, Action<bool> onComplete = null, StorageType? storageType = null)
			=> Storage.Set(key, value, onComplete);
	}
}

