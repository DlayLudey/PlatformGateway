const library = {
    $tgSdk: {
        isInitializeCalled: false,
        telegramApi: undefined,
        adsgramApi: undefined,

        telegramSdkInitialize: function (successCallbackPtr) {
            if(tgSdk.isInitializeCalled)
                return;
            
            tgSdk.isInitializeCalled = true;

            const scriptSrc = 'https://tgbot.dev-carrothood.ru/iframe-api.js';

            const sdkScript = document.createElement('script');
            sdkScript.src = scriptSrc;
            sdkScript.onload = () => {
                tgSdk.telegramApi = window.IframeTelegramApi;
                tgSdk.adsgramApi = window.IframeAdsGramApi;

                {{{ makeDynCall('v', 'successCallbackPtr') }}}();
            };
            sdkScript.onerror = () => {
                console.error('Failed to load SDK script.');
            };
            document.head.appendChild(sdkScript);
        },

        gameReady: function(){
            tgSdk.telegramApi.GameReady();
        },

        getUserInfo: function (successCallbackPtr) {
            tgSdk.telegramApi.GetUserData((data) => {
                const buffer = tgSdk.allocateUnmanagedString(JSON.stringify(data));
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(buffer);
                _free(buffer);
            });
        },

        saveCloudData: function(key, value, successCallbackPtr, errorCallbackPtr){
            tgSdk.telegramApi.SaveCloudData(key, value, () => {
                {{{ makeDynCall('v', 'successCallbackPtr') }}}();
            }, (error) => {
                const buffer = tgSdk.allocateUnmanagedString(JSON.stringify(error));
                {{{ makeDynCall('vi', 'errorCallbackPtr') }}}(buffer);
                _free(buffer);
            });
        },

        getCloudData: function(key, successCallbackPtr, errorCallbackPtr){
            tgSdk.telegramApi.GetCloudData(key, (data) => {
                const buffer = tgSdk.allocateUnmanagedString(data);
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(buffer);
                _free(buffer);
            }, (error) => {
                const buffer = tgSdk.allocateUnmanagedString(JSON.stringify(error));
                {{{ makeDynCall('vi', 'errorCallbackPtr') }}}(buffer);
                _free(buffer);
            });
        },

        showInterstitial: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
            try{
                tgSdk.adsgramApi.ShowInterstitial(() => {
                    {{{ makeDynCall('v', 'openCallbackPtr') }}}();
                }, () => {
                    {{{ makeDynCall('v', 'closeCallbackPtr') }}}();
                }, () => {
                    const buffer = tgSdk.allocateUnmanagedString("Error showing rewarded ad");
                    {{{ makeDynCall('vi', 'errorCallbackPtr') }}}(buffer);
                    _free(buffer);
                });   
            }
            catch (e){
                const buffer = tgSdk.allocateUnmanagedString(e);
                {{{ makeDynCall('vi', 'errorCallbackPtr') }}}(buffer);
                _free(buffer);
            }
        },

        showRewarded: function(openCallbackPtr, rewardedCallbackPtr, closedCallbackPtr, errorCallbackPtr) {
            try{
                tgSdk.adsgramApi.ShowRewarded(() => {
                    {{{ makeDynCall('v', 'openCallbackPtr') }}}();
                }, () => {
                    {{{ makeDynCall('v', 'rewardedCallbackPtr') }}}();
                }, () => {
                    {{{ makeDynCall('v', 'closedCallbackPtr') }}}();
                }, () => {
                    const buffer = tgSdk.allocateUnmanagedString("Error showing rewarded ad");
                    {{{ makeDynCall('vi', 'errorCallbackPtr') }}}(buffer);
                    _free(buffer);
                });    
            }
            catch (e){
                const buffer = tgSdk.allocateUnmanagedString(e);
                {{{ makeDynCall('vi', 'errorCallbackPtr') }}}(buffer);
                _free(buffer);
            }
            
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
    
    TgGameReady: function (){
        tgSdk.gameReady();
    },
    
    TgShowInterstitial: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
        tgSdk.showInterstitial(openCallbackPtr, closeCallbackPtr, errorCallbackPtr);
    },

    TgShowRewarded: function (openCallbackPtr, rewardedCallbackPtr, closedCallbackPtr, errorCallbackPtr) {
        tgSdk.showRewarded(openCallbackPtr, rewardedCallbackPtr, closedCallbackPtr, errorCallbackPtr);
    },

    TgSaveCloudData: function(keyPtr, valuePtr, successCallbackPtr, errorCallbackPtr){
        tgSdk.saveCloudData(UTF8ToString(keyPtr), UTF8ToString(valuePtr), successCallbackPtr, errorCallbackPtr);
    },

    TgGetCloudData: function(keyPtr, successCallbackPtr, errorCallbackPtr){
        tgSdk.getCloudData(UTF8ToString(keyPtr), successCallbackPtr, errorCallbackPtr);
    },

    TgGetUserInfo: function(successCallbackPtr){
        tgSdk.getUserInfo(successCallbackPtr);
    },
}

autoAddDeps(library, '$tgSdk');
mergeInto(LibraryManager.library, library);