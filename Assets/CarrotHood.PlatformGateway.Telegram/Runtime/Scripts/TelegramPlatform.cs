using UnityEngine;

namespace CarrotHood.PlatformGateway.Telegram
{
	[CreateAssetMenu(fileName = "TelegramPlatform" ,menuName = "Platforms/Telegram")]
	public class TelegramPlatform : Platform
	{
		public override PlatformType Type => PlatformType.Telegram;
		public override string Language { get; }

	}
}
