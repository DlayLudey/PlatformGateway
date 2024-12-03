using System.Collections;

namespace CarrotHood.PlatformGateway
{
	public class DefaultPlatform : Platform
	{
		public override PlatformType Type => PlatformType.Default;

		public override string Language => "ru";

		public override IEnumerator Init(PlatformBuilder builder)
		{
			yield return base.Init(builder);
		}
	}
}