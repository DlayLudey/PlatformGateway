using System.Runtime.InteropServices;

namespace CarrotHood.PlatformGateway
{
	internal static class PlatformGatewayInternal
	{

		public static PlatformType PlatformType
		{
			get
			{
#if !UNITY_EDITOR && UNITY_WEBGL
				
				var platform = GetPlatform();
				switch (platform)
				{
					case "": return PlatformType.Yandex;
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