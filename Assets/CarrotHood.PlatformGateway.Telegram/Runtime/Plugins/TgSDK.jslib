const library = {
    $tgSdk: {
        isInitializeCalled: false,

        telegramSdkInitialize: function (successCallbackPtr) {
            if(tgSdk.isInitializeCalled)
                return;
            
            tgSdk.isInitializeCalled = true;

            window.addEventListener('message', tgSdk.windowMessageCallback);

            dynCall('v', successCallbackPtr, []);
        },
        
        windowMessageCallback: function (event) {
            console.log(event);
            const message = event.data;
            const payload = message.payload;

            switch (message.type) {
                case "user-info":
                    dynCall('vi', tgSdk.getUserInfoSuccessCallbackPtr, [tgSdk.allocateUnmanagedString(JSON.stringify(payload.user))]);

                    break;
                case "cloudStorageSaved":
                    break;
                case "cloudStorageData":
                    break;
                case "adStatus":
                    switch (payload.status){
                        case "watched":
                            dynCall('v', tgSdk.rewardedSuccessCallbackPtr, []);
                            break;
                        case "not_watched":
                            dynCall('v', tgSdk.rewardedClosedCallbackPtr, []);
                            break;
                        case "error":
                            dynCall('vi', tgSdk.rewardedErrorCallbackPtr, [tgSdk.allocateUnmanagedString(JSON.stringify(payload.message))]);
                            break;
                    }
            }
        },
        
        //Player Account
        getUserInfoSuccessCallbackPtr: undefined,
        getUserInfo: function (successCallbackPtr) {
            tgSdk.getUserInfoSuccessCallbackPtr = successCallbackPtr;
            
            parent.postMessage({type: "getUserInfo"}, '*');
        },
        
        // Advertisment
        interstitialOpenCallbackPtr: undefined,
        interstitialClosedCallbackPtr: undefined,
        interstitialErrorCallbackPtr: undefined,
        showInterstitial: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
            tgSdk.interstitialOpenCallbackPtr = openCallbackPtr;
            tgSdk.interstitialClosedCallbackPtr = closeCallbackPtr;
            tgSdk.interstitialErrorCallbackPtr = errorCallbackPtr;

            console.log("Interstitials are not invented yet, sadge");
            dynCall('vi', errorCallbackPtr, [tgSdk.allocateUnmanagedString("Interstitials are not invented, what can do")]);
            // parent.postMessage({type: "showAd"}, '*');
        },

        rewardedSuccessCallbackPtr: undefined,
        rewardedClosedCallbackPtr: undefined,
        rewardedErrorCallbackPtr: undefined,
        showRewarded: function(rewardedSuccessCallbackPtr, rewardedClosedCallbackPtr, rewardedErrorCallbackPtr) {
            tgSdk.rewardedSuccessCallbackPtr = rewardedSuccessCallbackPtr;
            tgSdk.rewardedClosedCallbackPtr = rewardedClosedCallbackPtr;
            tgSdk.rewardedErrorCallbackPtr = rewardedErrorCallbackPtr;
            
            console.log(2);

            parent.postMessage({type: "showAd"}, '*');
        },
      
        // Storage
        getStorageSuccessCallbackPtr: undefined,
        getStorageErrorCallbackPtr: undefined,
        getStorage: function(keyPtr, successCallbackPtr, errorCallbackPtr){
            tgSdk.getStorageSuccessCallbackPtr = successCallbackPtr;
            tgSdk.getStorageErrorCallbackPtr = errorCallbackPtr;

            console.log("Getting storage, but data is not settable so im gonna return error :p");
            dynCall('vi', errorCallbackPtr, [tgSdk.allocateUnmanagedString("Data is not settable, what can do")]);
            // parent.postMessage({type: "getFromCloudStorage"}, '*');
        },
        
        setStorageSuccessCallbackPtr: undefined,
        setStorageErrorCallbackPtr: undefined,
        setStorage: function(keyPtr, valuePtr, successCallbackPtr, errorCallbackPtr){
            tgSdk.setStorageSuccessCallbackPtr = successCallbackPtr;
            tgSdk.setStorageErrorCallbackPtr = errorCallbackPtr;

            console.log("Setting storage, but data is not settable so im gonna return error :p");
            dynCall('vi', errorCallbackPtr, [tgSdk.allocateUnmanagedString("Data is not settable, what can do")]);
            // parent.postMessage({type: "saveToCloudStorage"}, '*');
        },
               
        // Utils

        allocateUnmanagedString: function (string) {
            const stringBufferSize = lengthBytesUTF8(string) + 1;
            const stringBufferPtr = _malloc(stringBufferSize);
            stringToUTF8(string, stringBufferPtr, stringBufferSize);
            return stringBufferPtr;
        }
    },


    TelegramSdkInitialize: function (successCallbackPtr) {
        tgSdk.telegramSdkInitialize(successCallbackPtr);
    },
    
    TgShowInterstitial: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
        tgSdk.showInterstitial(openCallbackPtr, closeCallbackPtr, errorCallbackPtr);
    },

    TgShowRewarded: function (rewardedSuccessCallbackPtr, rewardedClosedCallbackPtr, rewardedErrorCallbackPtr) {
        console.log(1);
        tgSdk.showRewarded(rewardedSuccessCallbackPtr, rewardedClosedCallbackPtr, rewardedErrorCallbackPtr);
    },
    
    TgGetStorage: function(keyPtr, successCallbackPtr, errorCallbackPtr){
        tgSdk.getStorage(keyPtr, successCallbackPtr, errorCallbackPtr);
    },
    
    TgSetStorage: function(keyPtr, valuePtr, successCallbackPtr, errorCallbackPtr){
        tgSdk.setStorage(keyPtr, valuePtr, successCallbackPtr, errorCallbackPtr);
    },

    TgGetUserInfo: function(successCallbackPtr){
        tgSdk.getUserInfo(successCallbackPtr);
    },
}

autoAddDeps(library, '$tgSdk');
mergeInto(LibraryManager.library, library);