const library = {
    $platformGateway: {

        initPlatformCommands: function (commands, commandCallbackPtr) {
            const data = JSON.parse(commands);
            
            window.PlatformGateway = {}
            
            data.forEach(entry => {
                const methodName = entry;
            
                window.PlatformGateway[methodName] = function () {
                    const buffer = platformGateway.allocateUnmanagedString(methodName);
                    
                    {{{ makeDynCall('vi', 'commandCallbackPtr') }}}(buffer);
                }
            })
        },
        
        get PLATFORM_ID() {
            return {
                VK: 'vk',
                VK_PLAY: 'vk_play',
                OK: 'ok',
                YANDEX: 'yandex',
                CRAZY_GAMES: 'crazy_games',
                ABSOLUTE_GAMES: 'absolute_games',
                GAME_DISTRIBUTION: 'game_distribution',
                PLAYGAMA: 'playgama',
                WORTAL: 'wortal',
                PLAYDECK: 'playdeck',
                TELEGRAM: 'telegram',
                MOCK: 'mock'
            }
        },


        getPlatform: function () {

            let platformId = platformGateway.PLATFORM_ID["MOCK"];

            if (this._options && this._options.forciblySetPlatformId) {
                platformId = this.getPlatformId(this._options.forciblySetPlatformId.toLowerCase());
            } else {
                const url = new URL(window.location.href);
                const yandexUrl = ['y', 'a', 'n', 'd', 'e', 'x', '.', 'n', 'e', 't'].join('');
                if (url.searchParams.has('platform_id')) {
                    platformId = this.getPlatformId(url.searchParams.get('platform_id').toLowerCase());
                } else if (url.hostname.includes(yandexUrl) || url.hash.includes('yandex')) {
                    platformId = platformGateway.PLATFORM_ID["YANDEX"];
                } else if (url.hostname.includes('crazygames.') || url.hostname.includes('1001juegos.com')) {
                    platformId = platformGateway.PLATFORM_ID["CRAZY_GAMES"];
                } else if (url.hostname.includes('gamedistribution.com')) {
                    platformId = platformGateway.PLATFORM_ID["GAME_DISTRIBUTION"];
                } else if (url.hostname.includes('wortal.ai')) {
                    platformId = platformGateway.PLATFORM_ID["WORTAL"];
                } else if (url.searchParams.has('api_id') && url.searchParams.has('viewer_id') && url.searchParams.has('auth_key')) {
                    platformId = platformGateway.PLATFORM_ID["VK"];
                } else if (url.searchParams.has('app_id') && url.searchParams.has('player_id') && url.searchParams.has('game_sid') && url.searchParams.has('auth_key')) {
                    platformId = platformGateway.PLATFORM_ID["ABSOLUTE_GAMES"];
                } else if (url.searchParams.has('playdeck')) {
                    platformId = platformGateway.PLATFORM_ID["PLAYDECK"];
                } else if (url.hash.includes('tgWebAppData')) {
                    platformId = platformGateway.PLATFORM_ID["TELEGRAM"];
                }
            }
            return platformGateway.allocateUnmanagedString(platformId);
        },


        getPlatformId : function (value) {
            const platformIds = Object.values(platformGateway.PLATFORM_ID);
            for (let i = 0; i < platformIds.length; i++) {
                if (value === platformIds[i]) {
                    return value;
                }
            }

            return platformGateway.PLATFORM_ID["MOCK"];
        },
        
        svg2Png: function(svgUrl, width, height, successCallbackPtr){
            const img = new Image();
            
            img.crossOrigin = "anonymous";
            img.onload = () => {
                const canvas = document.createElement('canvas');

                canvas.width = width;
                canvas.height = height;

                const ctx = canvas.getContext('2d');

                ctx.clearRect(0, 0, width, height);
                ctx.drawImage(img, 0, 0, width, height);

                const pngBase64 = canvas.toDataURL("image/png");
                
                const buffer = platformGateway.allocateUnmanagedString(pngBase64);
                
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(buffer);

                _free(buffer);

                URL.revokeObjectURL(svgUrl);
            };

            img.src = svgUrl;
        },

        allocateUnmanagedString: function (string) {
            const stringBufferSize = lengthBytesUTF8(string) + 1;
            const stringBufferPtr = _malloc(stringBufferSize);
            stringToUTF8(string, stringBufferPtr, stringBufferSize);
            return stringBufferPtr;
        }
    },
    
    InitPlatformCommands: function(commandsPtr, commandCallbackPtr){
        platformGateway.initPlatformCommands(UTF8ToString(commandsPtr), commandCallbackPtr);
    },

    GetPlatform: function(){
        return  platformGateway.getPlatform();
    },
    
    Svg2Png: function(svgUrlPtr, width, height, successCallbackPtr){
        platformGateway.svg2Png(UTF8ToString(svgUrlPtr), width, height, successCallbackPtr);
    }
}

autoAddDeps(library, '$platformGateway');
mergeInto(LibraryManager.library, library);