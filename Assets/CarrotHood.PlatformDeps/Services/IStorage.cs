using System;
using System.Collections.Generic;

namespace CarrotHood.PlatformDeps
{
	public interface IStorage
	{
		StorageType defaultType { get; }

		void Delete(List<string> keys, Action<bool> onComplete = null, StorageType? storageType = null);
		void Delete(string key, Action<bool> onComplete = null, StorageType? storageType = null);
		void Get(List<string> keys, Action<bool, List<string>> onComplete, StorageType? storageType = null);
		void Get(string key, Action<bool, string> onComplete, StorageType? storageType = null);
		bool IsAvailable(StorageType storageType);
		bool IsSupported(StorageType storageType);
		void Set(List<string> keys, List<object> values, Action<bool> onComplete = null, StorageType? storageType = null);
		void Set(string key, bool value, Action<bool> onComplete = null, StorageType? storageType = null);
		void Set(string key, int value, Action<bool> onComplete = null, StorageType? storageType = null);
		void Set(string key, string value, Action<bool> onComplete = null, StorageType? storageType = null);
	}

	public enum StorageType
	{
		LocalStorage,
		PlatformInternal
	}
}