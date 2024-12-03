using System.Collections;

namespace CarrotHood.PlatformGateway
{
	public interface IPlatform
	{
		PlatformSettings Settings { get; }
		PlatformType Type { get; }
		string Language { get; }
		IEnumerator Init(PlatformBuilder builder);
	}
}
