/*
 * This file is part of Playgama Bridge.
 *
 * Playgama Bridge is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * any later version.
 *
 * Playgama Bridge is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with Playgama Bridge. If not, see <https://www.gnu.org/licenses/>.
*/

#if UNITY_WEBGL
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
#if !UNITY_EDITOR
using Playgama.Common;
using System.Runtime.InteropServices;
#endif

namespace Playgama.Modules.RemoteConfig
{
    public class RemoteConfigModule : MonoBehaviour
    {
        public bool isSupported
        {
            get
            {
#if !UNITY_EDITOR
                return PlaygamaBridgeIsRemoteConfigSupported() == "true";
#else
                return false;
#endif
            }
        }
        
#if !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string PlaygamaBridgeIsRemoteConfigSupported();

        [DllImport("__Internal")]
        private static extern void PlaygamaBridgeRemoteConfigGet(string options);
#endif
        private Action<bool, Dictionary<string, string>> _getCallback;

        
        public void Get(Action<bool, Dictionary<string, string>> onComplete)
        {
            _getCallback = onComplete;

#if !UNITY_EDITOR
            PlaygamaBridgeRemoteConfigGet(string.Empty);
#else
            OnRemoteConfigGetFailed();
#endif
        }
        
        public void Get(Dictionary<string, object> options, Action<bool, Dictionary<string, string>> onComplete)
        {
            _getCallback = onComplete;

#if !UNITY_EDITOR
            PlaygamaBridgeRemoteConfigGet(options.ToJson());
#else
            OnRemoteConfigGetFailed();
#endif
        }


        // Called from JS
        private void OnRemoteConfigGetSuccess(string result)
        {
            var values = new Dictionary<string, string>();
            
            try
            {
                values = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            
            _getCallback?.Invoke(true, values);
        }

        private void OnRemoteConfigGetFailed()
        {
            _getCallback?.Invoke(false, null);
        }
    }
}
#endif