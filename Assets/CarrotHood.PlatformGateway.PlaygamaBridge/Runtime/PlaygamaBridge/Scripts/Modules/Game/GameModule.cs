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
using UnityEngine;
using System;
#if !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Playgama.Modules.Game
{
    public class GameModule : MonoBehaviour
    {
        public event Action<VisibilityState> visibilityStateChanged;

#if !UNITY_EDITOR
        public VisibilityState visibilityState 
        { 
            get
            {
                var state = PlaygamaBridgeGetVisibilityState();

                if (Enum.TryParse<VisibilityState>(state, true, out var value)) {
                    return value;
                }

                return VisibilityState.Visible;
            }
        }

        [DllImport("__Internal")]
        private static extern string PlaygamaBridgeGetVisibilityState();
#else
        public VisibilityState visibilityState => VisibilityState.Visible;
#endif

        // Called from JS
        private void OnVisibilityStateChanged(string value)
        {
            if (Enum.TryParse<VisibilityState>(value, true, out var state))
            {
                visibilityStateChanged?.Invoke(visibilityState);
            }
        }
    }
}
#endif