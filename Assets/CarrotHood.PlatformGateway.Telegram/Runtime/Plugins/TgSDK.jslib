const library = {
    $tgSDK: {
        isInitializeCalled: false,

        initializeSdk: function (successCallbackPtr) {
            dynCall('v', successCallbackPtr, []);
        },
        
        // Advertisment
        showInterstitial: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
            console.warn("Вызов интера(нереализовано)");

			dynCall('v', closeCallbackPtr, []);
			dynCall('v', errorCallbackPtr, []);
        },

        showRewarded: function(rewardedCallbackPtr, errorCallbackPtr) {
			console.warn("Вызов реварда(нереализовано)");
			dynCall('v', errorCallbackPtr, []);
		},
      
        // Storage

        getStorage: function(keyPtr, scopePtr, successCallbackPtr, errorCallbackPtr){
            //dynCall('vi', tgSDK.getStorageSuccessCallbackPtr, [successDataUnmanagedStringPtr]);
			//dynCall('vi', okSdk.interstitialErrorCallbackPtr, [errorDataUnmanagedStringPtr]);

        },
        
        setStorage: function(keyPtr, valuePtr, successCallbackPtr, errorCallbackPtr){

            //dynCall('v', tgSDK.setStorageSuccessCallbackPtr, []);
        },
               
        // Utils

        allocateUnmanagedString: function (string) {
            const stringBufferSize = lengthBytesUTF8(string) + 1;
            const stringBufferPtr = _malloc(stringBufferSize);
            stringToUTF8(string, stringBufferPtr, stringBufferSize);
            return stringBufferPtr;
        }
    },


    tgSDKInitialize: function (successCallbackPtr) {
        tgSDK.initializeSdk(successCallbackPtr);
    },
    
    ShowInterstitial: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
        tgSDK.showInterstitial(openCallbackPtr, closeCallbackPtr, errorCallbackPtr);
    },
    
    ShowRewarded: function (rewardedCallbackPtr, errorCallbackPtr) {
        tgSDK.showRewarded(rewardedCallbackPtr, errorCallbackPtr);
    },
    
    GetStorage: function(keyPtr, scopePtr, successCallbackPtr, errorCallbackPtr){
        tgSDK.getStorage(keyPtr, scopePtr, successCallbackPtr, errorCallbackPtr);
    },
    
    SetStorage: function(keyPtr, valuePtr, successCallbackPtr, errorCallbackPtr){
        tgSDK.setStorage(keyPtr, valuePtr, successCallbackPtr, errorCallbackPtr);
    },
}

autoAddDeps(library, '$tgSDK');
mergeInto(LibraryManager.library, library);