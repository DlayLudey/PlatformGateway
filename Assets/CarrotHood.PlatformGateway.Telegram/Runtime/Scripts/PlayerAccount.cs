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

        public static UserInfo userInfo;
        
        public static IEnumerator Initialize()
        {
            #if !UNITY_EDITOR
			TgGetUserInfo(OnGetUserInfo);
            #else
            OnGetUserInfo(JsonConvert.SerializeObject(new UserInfo
            {
                id = "123456",
                isBot = true,
                firstName = "John",
                lastName = "Doe",
                username = "JohnDoe",
                languageCode = "ru",
                isPremium = false,
                addedToAttachmentMenu = false,
                personalMessagesPermission = false,
                photoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/68/TechCrunch_Disrupt_Europe_Berlin_2013_%2810536888854%29_%28cropped%29.jpg/620px-TechCrunch_Disrupt_Europe_Berlin_2013_%2810536888854%29_%28cropped%29.jpg?20210611095614"
            }));
            #endif

            yield return new WaitUntil(() => userInfo != null);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetUserInfo(string json)
        {
            userInfo = JsonConvert.DeserializeObject<UserInfo>(json);
        }
    }

    [Serializable]
    public class UserInfo
    {
        [field: Preserve] 
        public string id;
        [field: Preserve, JsonProperty("is_bot")]
        public bool isBot;
        [field: Preserve, JsonProperty("first_name")] 
        public string firstName;
        [field: Preserve, JsonProperty("last_name")] 
        public string lastName;
        [field: Preserve] 
        public string username;
        [field: Preserve, JsonProperty("language_code")] 
        public string languageCode;
        [field: Preserve, JsonProperty("is_premium")] 
        public bool isPremium;
        [field: Preserve, JsonProperty("added_to_attachment_menu")] 
        public bool addedToAttachmentMenu;
        [field: Preserve, JsonProperty("allows_write_to_pm")] 
        public bool personalMessagesPermission;
        [field: Preserve, JsonProperty("photo_url")] 
        public string photoUrl;
    }
}
