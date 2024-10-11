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
					case "vk": return PlatformType.VK;
					case "vk_play": return PlatformType.VKPlay;
					case "ok": return PlatformType.OK;
					case "yandex": return PlatformType.Yandex;
					case "crazy_games": return PlatformType.CrazyGames;
					case "absolute_games": return PlatformType.AbsoluteGames;
					case "game_distribution": return PlatformType.GameDistribution;
					case "playgama": return PlatformType.Playgama;
					case "wortal": return PlatformType.Wortal;
					case "playdeck": return PlatformType.Playdeck;
					case "telegram": return PlatformType.Telegram;
					case "mock": return PlatformType.Default;
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