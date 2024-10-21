using System.Collections;

namespace CarrotHood.PlatformGateway
{
	public class DefaultPlatform : IPlatform
	{
		public PlatformSettings Settings => new PlatformSettings();

		public PlatformType Type => PlatformType.Default;

		public string Language => "ru";

		public IEnumerator Init(PlatformBuilder baseDeps)
		{
			yield return baseDeps;
		}
	}
}