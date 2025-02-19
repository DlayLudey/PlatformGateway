const library = {
    $vkSdk: {
        isInitializeCalled: false,
        vkBridge: undefined,

        // Initialization
        vkSdkInitialize: function (successCallbackPtr) {
            if(vkSdk.isInitializeCalled)
                return;

            vkSdk.isInitializeCalled = true;

            const scriptSrc = "https://unpkg.com/@vkontakte/vk-bridge/dist/browser.min.js";

            console.log("Adding SDK Script...");

            const sdkScript = document.createElement("Script");
            sdkScript.src = scriptSrc;
            sdkScript.defer = "defer";

            sdkScript.onload = () => {
                console.log('SDK script loaded.');
                
                vkSdk.vkBridge = window["vkBridge"];
                vkSdk.vkBridge.send("VKWebAppInit", {});
                {{{ makeDynCall('v', 'successCallbackPtr') }}}();
            };
            sdkScript.onerror = () => {
                console.error('Failed to load SDK script.');
            };

            document.head.appendChild(sdkScript);
        },
        
        webApplicationInitialize: function (onGameFocusChangeCallbackPtr) {

            vkSdk.vkBridge.subscribe((e) => {
                const { type, data } = e.detail;
            
                switch(type) {
                    case 'VKWebAppViewHide':
                        const reason = data.reason;
                        
                        if(reason === 'pip')
                            return;
                    
                        {{{ makeDynCall('vi', 'onGameFocusChangeCallbackPtr') }}}(false);
                        break;
                    case 'VKWebAppViewRestore':
                        {{{ makeDynCall('vi', 'onGameFocusChangeCallbackPtr') }}}(true);
                        break;
                }
            });

            document.addEventListener('visibilitychange', () => {
                if (document.hidden) {
                    {{{ makeDynCall('vi', 'onGameFocusChangeCallbackPtr') }}}(false);
                } else {
                    {{{ makeDynCall('vi', 'onGameFocusChangeCallbackPtr') }}}(true);
                }
            });
        },
        
        getLaunchParams: function (successCallbackPtr, errorCallbackPtr) {
            vkSdk.vkBridge.send('VKWebAppGetLaunchParams')
                .then((data) => {
                    if (data.vk_app_id) {
                        vkSdk.invokeViCallback(successCallbackPtr, JSON.stringify(data));
                        return;
                    }
                    
                    vkSdk.invokeViCallback(errorCallbackPtr, "GetLaunchParams data error");
                })
                .catch((error) => {
                    vkSdk.invokeViCallback(errorCallbackPtr, error.message);
                });
        },
        
        // Billing
        showPayment: function (key, successCallbackPtr, errorCallbackPtr) {
            vkSdk.vkBridge.send('VKWebAppShowOrderBox', {
                type: 'item',
                item: key
            })
                .then((data) => {
                    if (data.success) {
                        {{{ makeDynCall('v', 'successCallbackPtr') }}}();
                        return;
                    }
                    
                    vkSdk.invokeViCallback(errorCallbackPtr, "ShowPayment data error");
                })
                .catch((error) => {
                    vkSdk.invokeViCallback(errorCallbackPtr, error.message);
                });
        },
        
        // Social
        inviteFriends: function (successCallbackPtr, errorCallbackPtr) {
            vkSdk.vkBridge.send('VKWebAppShowInviteBox', {
            })
                .then((data) => {
                    if(data.success) {
                        {{{ makeDynCall('v', 'successCallbackPtr') }}}();
                        return;
                    }
                    
                    vkSdk.invokeViCallback(errorCallbackPtr, "InviteFriends data error");
                })
                .catch((error) => {
                    vkSdk.invokeViCallback(errorCallbackPtr, error.message);
                });
        },
        
        // Advertisement
        showInterstitial: function (closeCallbackPtr, errorCallbackPtr){
            vkSdk.vkBridge.send('VKWebAppShowNativeAds', {
                ad_format: 'interstitial'
            })
                .then( (data) => {
                    if (data.result) {
                        {{{ makeDynCall('v', 'closeCallbackPtr') }}}();
                        return;
                    }
                    
                    vkSdk.invokeViCallback(errorCallbackPtr, "Interstitial error");
                })
                .catch((error) => { 
                    vkSdk.invokeViCallback(errorCallbackPtr, error.message)
                });
        },

        showRewarded: function (closeCallbackPtr, errorCallbackPtr){
            vkSdk.vkBridge.send('VKWebAppShowNativeAds', {
                ad_format: 'reward'
            })
                .then( (data) => {
                    if (data.result) {
                        {{{ makeDynCall('v', 'closeCallbackPtr') }}}();
                        return;
                    }

                    vkSdk.invokeViCallback(errorCallbackPtr, "Interstitial error");
                })
                .catch((error) => {
                    vkSdk.invokeViCallback(errorCallbackPtr, error.message)
                });
        },
        
        // Storage
        getStorage: function (key, successCallbackPtr, errorCallbackPtr) {
            vkSdk.vkBridge.send('VKWebAppStorageGet', {
                keys: [
                    key
                ]})
                .then((data) => {
                    if (data.keys) {
                        vkSdk.invokeViCallback(successCallbackPtr, data.keys[0].value);
                        return;
                    }
                    
                    vkSdk.invokeViCallback(errorCallbackPtr, "GetStorage data error")
                })
                .catch((error) => {
                    vkSdk.invokeViCallback(errorCallbackPtr, error.message);
                });
        },
        
        setStorage: function (key, value, successCallbackPtr, errorCallbackPtr) {
            vkSdk.vkBridge.send('VKWebAppStorageSet', {
                key: key,
                value: value
            })
                .then((data) => {
                    if (data.result) {
                        {{{ makeDynCall('v', 'successCallbackPtr') }}}();
                        return;
                    }
                    
                    vkSdk.invokeViCallback(errorCallbackPtr, "SetStorage error");
                })
                .catch((error) => {
                    vkSdk.invokeViCallback(errorCallbackPtr, error.message);
                });
        },
        
        // PartialStorage
        
        getPartialStorage: function (key, token, userId, successCallbackPtr, errorCallbackPtr) {
            vkSdk.vkBridge.send("VKWebAppCallAPIMethod", {
                method: "execute.getPartialData",
                params: {
                    Key: key,
                    UserId: userId,
                    access_token: token,
                    v: "5.199"
                }
            }).then((data) => {
                vkSdk.invokeViCallback(successCallbackPtr, data.response);
            }).catch((error) => {
                vkSdk.invokeViCallback(errorCallbackPtr, error.message);
            });
        },
        
        setPartialStorage: function (key, value, token, userId, successCallbackPtr, errorCallbackPtr) {
            vkSdk.vkBridge.send("VKWebAppCallAPIMethod", {
                method: "execute.setPartialData",
                params: {
                    Key: key,
                    Save: value,
                    UserId: userId,
                    access_token: token,
                    v: "5.199"
                }
            }).then((data) => {
                {{{ makeDynCall('v', 'successCallbackPtr') }}}();
            }).catch((error) => {
                vkSdk.invokeViCallback(errorCallbackPtr, error.message);
            });
        },
        
        // Utils
        invokeViCallback: function (callbackPtr, data){
            const buffer = vkSdk.allocateUnmanagedString(data);

            {{{ makeDynCall('vi', 'callbackPtr') }}}(buffer);
            
            _free(buffer);
        },

        allocateUnmanagedString: function (string) {
            const stringBufferSize = lengthBytesUTF8(string) + 1;
            const stringBufferPtr = _malloc(stringBufferSize);
            stringToUTF8(string, stringBufferPtr, stringBufferSize);
            return stringBufferPtr;
        }
    },

    VkSdkInitialize: function (successCallbackPtr) {
        vkSdk.vkSdkInitialize(successCallbackPtr);
    },
    
    VkWebApplicationInitialize: function (onGameFocusChangeCallbackPtr){
        vkSdk.webApplicationInitialize(onGameFocusChangeCallbackPtr);
    },
    
    VkGetLaunchParams: function (successCallbackPtr, errorCallbackPtr) {
        vkSdk.getLaunchParams(successCallbackPtr, errorCallbackPtr);
    },
    
    VkShowPayment: function (keyPtr, successCallbackPtr, errorCallbackPtr) {
        vkSdk.showPayment(UTF8ToString(keyPtr), successCallbackPtr, errorCallbackPtr);
    },
    
    VkInviteFriends: function (successCallbackPtr, errorCallbackPtr) {
        vkSdk.inviteFriends(successCallbackPtr, errorCallbackPtr);
    },
    
    VkShowInterstitial: function (closeCallbackPtr, errorCallbackPtr){
        vkSdk.showInterstitial(closeCallbackPtr, errorCallbackPtr)
    },
    
    VkShowRewarded: function (closeCallbackPtr, errorCallbackPtr){
        vkSdk.showRewarded(closeCallbackPtr, errorCallbackPtr)
    },
    
    VkGetStorage: function (keyPtr, successCallbackPtr, errorCallbackPtr) {
        vkSdk.getStorage(UTF8ToString(keyPtr), successCallbackPtr, errorCallbackPtr);
    },
    
    VkSetStorage: function (keyPtr, valuePtr, successCallbackPtr, errorCallbackPtr) {
        vkSdk.setStorage(UTF8ToString(keyPtr), UTF8ToString(valuePtr), successCallbackPtr, errorCallbackPtr);
    },
    
    VkGetPartialStorage: function (keyPtr, tokenPtr, userId, successCallbackPtr, errorCallbackPtr) {
        vkSdk.getPartialStorage(UTF8ToString(keyPtr), UTF8ToString(tokenPtr), userId, successCallbackPtr, errorCallbackPtr);
    },
    
    VkSetPartialStorage: function (keyPtr, valuePtr, tokenPtr, userId, successCallbackPtr, errorCallbackPtr) {
        vkSdk.setPartialStorage(UTF8ToString(keyPtr), UTF8ToString(valuePtr), UTF8ToString(tokenPtr), userId, successCallbackPtr, errorCallbackPtr);
    },
}

autoAddDeps(library, '$vkSdk');
mergeInto(LibraryManager.library, library);