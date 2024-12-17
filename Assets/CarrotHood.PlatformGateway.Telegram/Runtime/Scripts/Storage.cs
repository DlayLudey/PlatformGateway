using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace CarrotHood.PlatformGateway.Telegram
{
    public static class Storage
    {
#region SetStorage
        [DllImport("__Internal")]
        private static extern void TgSaveCloudData(string key, string value, Action onSuccess, Action<string> onError);

        private static Action onSetStorageSuccess;
        private static Action<string> onSetStorageError;
        
        public static void SetCloudData(string key, string value, Action onSuccess, Action<string> onError = null)
        {
            onSetStorageSuccess = onSuccess;
            onSetStorageError = onError;
            
            #if !UNITY_EDITOR
            TgSaveCloudData(key, value, OnSetStorageSuccess, OnSetStorageError);
            #else
            PlayerPrefs.SetString(key, value);
            OnSetStorageSuccess();
            #endif
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnSetStorageSuccess()
        {
            onSetStorageSuccess?.Invoke();
        }
        
        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnSetStorageError(string error)
        {
            onSetStorageError?.Invoke(error);
        }
#endregion
        
#region GetStorage
        [DllImport("__Internal")]
        private static extern void TgGetCloudData(string key, Action<string> onSuccess, Action<string> onError);

        private static Action<string> onGetStorageSuccess;
        private static Action<string> onGetStorageError;
        
        public static void GetCloudData(string key, Action<string> onSuccess, Action<string> onError = null)
        {
            onGetStorageSuccess = onSuccess;
            onGetStorageError = onError;
            
            #if !UNITY_EDITOR
            TgGetCloudData(key, OnGetStorageSuccess, OnGetStorageError);
            #else
            OnGetStorageSuccess(PlayerPrefs.GetString(key));
            #endif
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetStorageSuccess(string value)
        {
            onGetStorageSuccess?.Invoke(value);
        }
        
        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetStorageError(string error)
        {
            onGetStorageError?.Invoke(error);
        }
#endregion
    }
}
