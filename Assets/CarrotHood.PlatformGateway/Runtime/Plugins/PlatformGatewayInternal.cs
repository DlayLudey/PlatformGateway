using System.Runtime.InteropServices;

namespace CarrotHood.PlatformGateway
{
	internal static class PlatformGatewayInternal
	{
//		   VK: 'vk',
//         VK_PLAY: 'vk_play',
//         OK: 'ok',
//         YANDEX: 'yandex',
//         CRAZY_GAMES: 'crazy_games',
//         ABSOLUTE_GAMES: 'absolute_games',
//         GAME_DISTRIBUTION: 'game_distribution',
//         PLAYGAMA: 'playgama',
//         WORTAL: 'wortal',
//         PLAYDECK: 'playdeck',
//         TELEGRAM: 'telegram',
//         MOCK: 'mock',
		public static PlatformType PlatformType
		{
			get
			{
#if !UNITY_EDITOR && UNITY_WEBGL

				var platform = GetPlatform();
				switch (platform)
				{
					case "ok": return PlatformType.Ok;
					case "yandex": return PlatformType.Yandex;
					case "telegram": return PlatformType.Telegram;
					case "mock": return PlatformType.Default;
					case "vk": return PlatformType.Vk;
					default: return PlatformType.Default;
				}
#else
				return PlatformType.Default;
#endif
			}
		}


		[DllImport("__Internal")]
		private static extern string GetPlatform();
	}
}