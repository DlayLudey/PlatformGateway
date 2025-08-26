using UnityEngine;

namespace CarrotHood.PlatformGateway
{
	public static class CustomCommands
	{
		public static void Init()
		{
			PlatformGatewayInternal.InitCustomPlatformCommands(new []
			{
				new PlatformGatewayInternal.PlatformCommand(ClearSaves),
			});
		}

		private static void ClearSaves()
		{
			PlatformGateway.Storage.ClearData(() =>
			{
				Debug.Log("Saves are cleared!");
				Application.Quit();
			}, Debug.LogError);
		}
	}
}