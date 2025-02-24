using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;

namespace Qt.OkSdk
{
    public static class Storage
    {
#region GetStorage
        [DllImport("__Internal")]
        private static extern void OkGetStorage(string key, Action<string> onSuccess, Action<string> onError);

        private static Action<string> s_onGetStorageSuccess;
        private static Action<string> s_onGetStorageError;
        
        public static void GetStorageValue(string key, Action<string> onSuccess, Action<string> onError = null)
        {
            s_onGetStorageSuccess = onSuccess;
            s_onGetStorageError = onError;

            #if !UNITY_EDITOR
            OkGetStorage(key, OnGetStorageSuccess, OnGetStorageError);
            #else
            OnGetStorageSuccess(PlayerPrefs.GetString(key));
            #endif
        }
        [MonoPInvokeCallback(typeof(Action))]
        private static void OnGetStorageSuccess(string data)
        {
            s_onGetStorageSuccess?.Invoke(data);
        }
        
        [MonoPInvokeCallback(typeof(Action))]
        private static void OnGetStorageError(string error)
        {
            s_onGetStorageError?.Invoke(error);
        }
#endregion

#region SetStorage
        [DllImport("__Internal")]
        private static extern void OkSetStorage(string key, string value, Action onSuccess, Action<string> onError);

        private static Action s_onSetStorageSuccess;
        private static Action<string> s_onSetStorageError;
        
        public static void SetStorageValue(string key, string value, Action onSuccess = null, Action<string> onError = null)
        {
            s_onSetStorageSuccess = onSuccess;
            s_onSetStorageError = onError;

            #if !UNITY_EDITOR
            OkSetStorage(key, value, OnSetStorageSuccess, OnSetStorageError);
            #else
            PlayerPrefs.SetString(key, value);
            OnSetStorageSuccess();
            #endif
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnSetStorageSuccess()
        {
            s_onSetStorageSuccess?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnSetStorageError(string error)
        {
            s_onSetStorageError?.Invoke(error);
        }
#endregion
    }
}