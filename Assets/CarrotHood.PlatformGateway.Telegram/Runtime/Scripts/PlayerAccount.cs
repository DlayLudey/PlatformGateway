using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace CarrotHood.PlatformGateway.Telegram
{
    public static class PlayerAccount
    {
        [DllImport("__Internal")]
        private static extern void TgGetUserInfo(Action<string> onSuccess);

        public static UserInfo UserInfo;
        
        public static IEnumerator GetUserInfo(Action<UserInfo> successCallback = null)
        {
            if (!TelegramSdk.IsInitialized)
            {
                Debug.LogWarning("Sdk is not initialized!");
                yield break;
            }
			
            #if !UNITY_EDITOR
			TgGetUserInfo(OnGetUserInfo);
            #else
            OnGetUserInfo(JsonConvert.SerializeObject(new UserInfo
            {
                id = "123456",
                firstName = "John",
                lastName = "Doe",
                username = "JohnDoe",
                languageCode = "ru",
                personalMessagesPermission = false,
                photoUrl = ""
            }));
            #endif

            yield return new WaitUntil(() => UserInfo != null);
            successCallback?.Invoke(UserInfo);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetUserInfo(string json)
        {
            UserInfo = JsonConvert.DeserializeObject<UserInfo>(json);
        }
    }

    [Serializable]
    public class UserInfo
    {
        [field: Preserve] 
        public string id;
        [field: Preserve, JsonProperty("first_name")] 
        public string firstName;
        [field: Preserve, JsonProperty("last_name")] 
        public string lastName;
        [field: Preserve] 
        public string username;
        [field: Preserve, JsonProperty("language_code")] 
        public string languageCode;
        [field: Preserve, JsonProperty("allows_write_to_pm")] 
        public bool personalMessagesPermission;
        [field: Preserve, JsonProperty("photo_url")] 
        public string photoUrl;
    }
}
