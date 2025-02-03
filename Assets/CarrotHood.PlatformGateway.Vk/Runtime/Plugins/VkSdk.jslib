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
                    vkSdk.invokeViCallback(errorCallbackPtr, error);
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
                    vkSdk.invokeViCallback(errorCallbackPtr, error);
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
                    vkSdk.invokeViCallback(errorCallbackPtr, error);
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
                    vkSdk.invokeViCallback(errorCallbackPtr, error)
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
                    vkSdk.invokeViCallback(errorCallbackPtr, error)
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
                    vkSdk.invokeViCallback(errorCallbackPtr, error);
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
                    vkSdk.invokeViCallback(errorCallbackPtr, error);
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
    }
}

autoAddDeps(library, '$vkSdk');
mergeInto(LibraryManager.library, library);