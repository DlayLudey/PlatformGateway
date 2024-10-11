const library = {
    $platformGateway: {

        export const PLATFORM_ID = {
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
            MOCK: 'mock',
        }


        function GetPlatform() {

            let platformId = PLATFORM_ID.MOCK

            if (this._options && this._options.forciblySetPlatformId) {
                platformId = this.#getPlatformId(this._options.forciblySetPlatformId.toLowerCase())
            } else {
                const url = new URL(window.location.href)
                const yandexUrl = ['y', 'a', 'n', 'd', 'e', 'x', '.', 'n', 'e', 't'].join('')
                if (url.searchParams.has('platform_id')) {
                    platformId = this.#getPlatformId(url.searchParams.get('platform_id').toLowerCase())
                } else if (url.hostname.includes(yandexUrl) || url.hash.includes('yandex')) {
                    platformId = PLATFORM_ID.YANDEX
                } else if (url.hostname.includes('crazygames.') || url.hostname.includes('1001juegos.com')) {
                    platformId = PLATFORM_ID.CRAZY_GAMES
                } else if (url.hostname.includes('gamedistribution.com')) {
                    platformId = PLATFORM_ID.GAME_DISTRIBUTION
                } else if (url.hostname.includes('wortal.ai')) {
                    platformId = PLATFORM_ID.WORTAL
                } else if (url.searchParams.has('api_id') && url.searchParams.has('viewer_id') && url.searchParams.has('auth_key')) {
                    platformId = PLATFORM_ID.VK
                } else if (url.searchParams.has('app_id') && url.searchParams.has('player_id') && url.searchParams.has('game_sid') && url.searchParams.has('auth_key')) {
                    platformId = PLATFORM_ID.ABSOLUTE_GAMES
                } else if (url.searchParams.has('playdeck')) {
                    platformId = PLATFORM_ID.PLAYDECK
                } else if (url.hash.includes('tgWebAppData')) {
                    platformId = PLATFORM_ID.TELEGRAM
                }
            }
            return platformId;
        }


    #getPlatformId(value) {
        const platformIds = Object.values(PLATFORM_ID)
        for (let i = 0; i < platformIds.length; i++) {
            if (value === platformIds[i]) {
                return value
            }
        }

        return PLATFORM_ID.MOCK
    }
}

autoAddDeps(library, '$okSdk');
mergeInto(LibraryManager.library, library);