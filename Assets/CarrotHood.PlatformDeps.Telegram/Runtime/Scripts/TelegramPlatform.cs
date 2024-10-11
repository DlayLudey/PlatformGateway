using UnityEngine;

namespace CarrotHood.PlatformDeps.Telegram
{
	[CreateAssetMenu(fileName = "TelegramPlatform" ,menuName = "Platforms/Telegram")]
	public class TelegramPlatform : Platform
	{
		public override PlatformType Type => PlatformType.Telegram;

		public override bool CheckRelevant()
		{
			return true;
		}
	}
}
