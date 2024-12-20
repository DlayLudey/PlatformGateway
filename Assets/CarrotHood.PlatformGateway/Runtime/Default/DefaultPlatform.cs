using System.Collections;
using UnityEngine;

namespace CarrotHood.PlatformGateway
{
	public class DefaultPlatform : PlatformBase
	{
		public override PlatformType Type => PlatformType.Default;

		public override string Language => "ru";

		public override IEnumerator Init(PlatformBuilder builder)
		{
			yield return null;
		}
	}
}