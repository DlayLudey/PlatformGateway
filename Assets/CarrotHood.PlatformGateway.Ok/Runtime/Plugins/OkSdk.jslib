const library = {
    $okSdk: {
        isInitializeCalled: false,
        
        FAPI: undefined,
        okCallbacks: undefined,
        
        get apiCallbacks() {
            return {
                showAd: (result, data) => okSdk.showAdCallback(result, data),
                loadAd: (result, data) => okSdk.loadAdCallback(result, data),
                showLoadedAd: (result, data) => okSdk.showLoadedAdCallback(result, data),
                showPayment: (result, data) => okSdk.showPaymentCallback(result, data),
                showInvite: (result, data) => okSdk.showInviteCallback(result, data),


                showPermissions: (_, __) => okSdk.dummy(_, __),
                requestBannerAds: (_, __) => okSdk.dummy(_, __),
                showBannerAds: (_, __) => okSdk.dummy(_, __),
                hideBannerAds: (_, __) => okSdk.dummy(_, __),
                setBannerFormat: (_, __) => okSdk.dummy(_, __),
                showRatingDialog: (_, __) => okSdk.dummy(_, __),
                joinGroup: (_, __) => okSdk.dummy(_, __),
                showLoginSuggestion: (_, __) => okSdk.dummy(_, __),
                postMediatopic: (_, __) => okSdk.dummy(_, __),
                isAdBlockEnabled: (_, __) => okSdk.dummy(_, __),
            }
        },
        
        dummy: function (_, __){
            
        },
        
        //Initialization
        okSdkInitialize: function (successCallbackPtr) {
            if(okSdk.isInitializeCalled)
                return;

            okSdk.isInitializeCalled = true;
            
            const scriptSrc = "//api.ok.ru/js/fapi5.js";
            
            console.log("Adding SDK Script...");
            
            const sdkScript = document.createElement("Script");
            sdkScript.src = scriptSrc;
            sdkScript.defer = "defer";

            sdkScript.onload = () => {
                console.log('SDK script loaded.');
                okSdk.initializeSdk(successCallbackPtr);
            };
            sdkScript.onerror = () => {
                console.error('Failed to load SDK script.');
            };
            
            document.head.appendChild(sdkScript);
        },

        initializeSdk: function (successCallbackPtr) {
            okSdk.FAPI = window["FAPI"];
            
            const rParams = okSdk.FAPI.Util.getRequestParameters();

            okSdk.FAPI.init(rParams["api_server"], rParams["apiconnection"],
                () => {
                    console.log("Sdk initialized successfully.");
                    window["API_callback"] = (method, result, data) => okSdk.apiCallbacks[method](result, data);
                    {{{ makeDynCall('v', 'successCallbackPtr') }}}();
                },
                () => {
                    console.warn("Sdk failed to initialize.");
                }
            );
        },
        
        // Advertisment
        interstitialOpenCallbackPtr: undefined,
        interstitialClosedCallbackPtr: undefined,
        interstitialErrorCallbackPtr: undefined,
        showInterstitial: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
            okSdk.interstitialClosedCallbackPtr = openCallbackPtr;
            okSdk.interstitialClosedCallbackPtr = closeCallbackPtr;
            okSdk.interstitialErrorCallbackPtr = errorCallbackPtr;

            okSdk.FAPI.UI.showAd();
        },
        
        showAdCallback: function (result, data) {
            if(result !== "ok")
            {
                const buffer = okSdk.allocateUnmanagedString(data);
                {{{ makeDynCall('vi', 'okSdk.interstitialErrorCallbackPtr') }}}(buffer);
                _free(buffer);
                return;
            }
            
            switch(data){
                case "ad_prepared":
                    {{{ makeDynCall('v', 'okSdk.interstitialOpenCallbackPtr') }}}();
                    break;
                case "ad_shown":
                    {{{ makeDynCall('v', 'okSdk.interstitialClosedCallbackPtr') }}}();
                    break;
            }
        },
        
        loadAdSuccessCallbackPtr: undefined,
        loadAdErrorCallbackPtr: undefined,
        loadRewardedAd: function(successCallbackPtr, errorCallbackPtr){
            okSdk.loadAdSuccessCallbackPtr = successCallbackPtr;
            okSdk.loadAdErrorCallbackPtr = errorCallbackPtr;
            
            okSdk.FAPI.UI.loadAd();
        },

        loadAdCallback: function(result, data){
            if(result !== "ok")
            {
                const buffer = okSdk.allocateUnmanagedString(data);
                {{{ makeDynCall('vi', 'okSdk.loadAdErrorCallbackPtr') }}}(buffer);
                _free(buffer);
                return;
            }

            {{{ makeDynCall('v', 'okSdk.loadAdSuccessCallbackPtr') }}}(buffer);
        },

        rewardedShownCallbackPtr: undefined,
        rewardedErrorCallbackPtr: undefined,
        showRewarded: function(rewardedCallbackPtr, errorCallbackPtr) {
            okSdk.rewardedShownCallbackPtr = rewardedCallbackPtr;
            okSdk.rewardedErrorCallbackPtr = errorCallbackPtr;

            okSdk.FAPI.UI.showLoadedAd();
        },
        
        showLoadedAdCallback: function(result, data) {
            if(result !== "ok")
            {
                const buffer = okSdk.allocateUnmanagedString(data);
                {{{ makeDynCall('vi', 'okSdk.rewardedErrorCallbackPtr') }}}(buffer);
                 _free(buffer);
                return;
            }
            
            switch(data){
                case "complete":
                case "ad_shown":
                    {{{ makeDynCall('v', 'okSdk.rewardedShownCallbackPtr') }}}();
                    break
            }
        },
        
        // Billing
        
        paymentSuccessCallbackPtr: undefined,
        paymentErrorCallbackPtr: undefined,
        showPayment: function(namePtr, descriptionPtr, codePtr, price, successCallbackPtr, errorCallbackPtr) {
            okSdk.paymentSuccessCallbackPtr = successCallbackPtr;
            okSdk.paymentErrorCallbackPtr = errorCallbackPtr;
            
            okSdk.FAPI.UI.showPayment(
                UTF8ToString(namePtr), 
                UTF8ToString(descriptionPtr), 
                UTF8ToString(codePtr), 
                price, 
                undefined, 
                undefined, 
                undefined, 
                "true");
        },
        
        showPaymentCallback: function(result, data){
            if(result !== "ok"){
                const buffer = okSdk.allocateUnmanagedString(data);
                {{{ makeDynCall('vi', 'okSdk.paymentErrorCallbackPtr') }}}(buffer);
                _free(buffer);
                return;
            }

            {{{ makeDynCall('v', 'okSdk.paymentSuccessCallbackPtr') }}}(buffer);
        },
        
        // Storage
        getStorageSuccessCallbackPtr: undefined,
        getStorageErrorCallbackPtr: undefined,
        getStorageKey: undefined,
        getStorage: function(keyPtr, scopePtr, successCallbackPtr, errorCallbackPtr){
            okSdk.getStorageSuccessCallbackPtr = successCallbackPtr;
            okSdk.getStorageErrorCallbackPtr = errorCallbackPtr;
            okSdk.getStorageKey = UTF8ToString(keyPtr);
            
            okSdk.FAPI.Client.call(
                {
                    "method": "storage.get",
                    "keys":[okSdk.getStorageKey],
                    "scope":UTF8ToString(scopePtr)
                }, okSdk.getStorageCallback);
        },
        
        getStorageCallback: function(status, data, error){
            if(status !== "ok"){
                const buffer = okSdk.allocateUnmanagedString(JSON.stringify(error));
                {{{ makeDynCall('vi', 'okSdk.getStorageErrorCallbackPtr') }}}(buffer);
                _free(buffer);
                return;
            }
            
            var value = "";
            
            if(data === undefined || data["data"] === undefined)
                value = ""
            else
                value = data["data"][okSdk.getStorageKey];
            
            const buffer = okSdk.allocateUnmanagedString(value);
            {{{ makeDynCall('vi', 'okSdk.getStorageSuccessCallbackPtr') }}}(buffer);
            _free(buffer);
        },
        
        setStorageSuccessCallbackPtr: undefined,
        setStorageErrorCallbackPtr: undefined,
        setStorage: function(keyPtr, valuePtr, successCallbackPtr, errorCallbackPtr){
            okSdk.setStorageSuccessCallbackPtr = successCallbackPtr;
            okSdk.setStorageErrorCallbackPtr = errorCallbackPtr;
            
            okSdk.FAPI.Client.call(
                {
                    "method": "storage.set",
                    "key":UTF8ToString(keyPtr),
                    "value":UTF8ToString(valuePtr)
                }, okSdk.setStorageCallback);
        },
        
        setStorageCallback: function(status, data, error){
            if(status !== "ok"){
                const buffer = okSdk.allocateUnmanagedString(JSON.stringify(error));
                {{{ makeDynCall('vi', 'okSdk.setStorageErrorCallbackPtr') }}}(buffer);
                _free(buffer);
                return;
            }

            {{{ makeDynCall('v', 'okSdk.setStorageSuccessCallbackPtr') }}}();
        },
        
        // Social
        
        showInviteSuccessCallbackPtr: undefined,
        showInviteErrorCallbackPtr: undefined,
        showInvite: function(textPtr, successCallbackPtr, errorCallbackPtr){
            okSdk.showInviteSuccessCallbackPtr = successCallbackPtr;
            okSdk.showInviteErrorCallbackPtr = errorCallbackPtr;

            okSdk.FAPI.UI.showInvite(UTF8ToString(textPtr));
        },

        showInviteCallback: function(result, data){
            if(result !== "ok"){
                const buffer = okSdk.allocateUnmanagedString(JSON.stringify(data));
                {{{ makeDynCall('vi', 'okSdk.showInviteErrorCallbackPtr') }}}(buffer);
                _free(buffer);
                return;
            }

            {{{ makeDynCall('vi', 'okSdk.showInviteSuccessCallbackPtr') }}}(data.split(",").length);
        },
        
        // Utils

        allocateUnmanagedString: function (string) {
            const stringBufferSize = lengthBytesUTF8(string) + 1;
            const stringBufferPtr = _malloc(stringBufferSize);
            stringToUTF8(string, stringBufferPtr, stringBufferSize);
            return stringBufferPtr;
        }
    },


    OkSdkInitialize: function (successCallbackPtr) {
        okSdk.okSdkInitialize(successCallbackPtr);
    },
    
    OkShowInterstitial: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
        okSdk.showInterstitial(openCallbackPtr, closeCallbackPtr, errorCallbackPtr);
    },
    
    OkLoadRewardedAd: function(successCallbackPtr, errorCallbackPtr){
        return okSdk.loadRewardedAd(successCallbackPtr, errorCallbackPtr);
    },
    
    OkShowRewarded: function (rewardedCallbackPtr, errorCallbackPtr) {
        okSdk.showRewarded(rewardedCallbackPtr, errorCallbackPtr);
    },
    
    OkShowPayment: function (namePtr, descriptionPtr, codePtr, price, paymentSuccessCallbackPtr, paymentErrorCallbackPtr){
        okSdk.showPayment(namePtr, descriptionPtr, codePtr, price, paymentSuccessCallbackPtr, paymentErrorCallbackPtr);
    },
    
    OkGetStorage: function(keyPtr, scopePtr, successCallbackPtr, errorCallbackPtr){
        okSdk.getStorage(keyPtr, scopePtr, successCallbackPtr, errorCallbackPtr);
    },
    
    OkSetStorage: function(keyPtr, valuePtr, successCallbackPtr, errorCallbackPtr){
        okSdk.setStorage(keyPtr, valuePtr, successCallbackPtr, errorCallbackPtr);
    },
    
    OkShowInvite: function(textPtr, successCallbackPtr, errorCallbackPtr){
        okSdk.showInvite(textPtr, successCallbackPtr, errorCallbackPtr);
    }
}

autoAddDeps(library, '$okSdk');
mergeInto(LibraryManager.library, library);