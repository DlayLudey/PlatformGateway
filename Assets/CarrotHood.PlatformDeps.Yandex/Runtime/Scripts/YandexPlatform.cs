using System;
using System.Collections;
using Kimicu.YandexGames;
using UnityEngine;

namespace CarrotHood.PlatformDeps.Yandex
{
	[CreateAssetMenu(fileName = "YandexPlatform", menuName = "Platforms/Yandex")]
	public partial class YandexPlatform : Platform
	{
		public override PlatformType Type => PlatformType.Yandex;

		public override bool CheckRelevant()
		{
			return true;
		}

		public override IEnumerator Init(PlatformBuilder builder)
#if !YandexOverride
		{
			yield return null;
		}
#else
;
#endif
	}
}