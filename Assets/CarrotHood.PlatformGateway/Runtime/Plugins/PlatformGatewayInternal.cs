using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AOT;
using Newtonsoft.Json;

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
					case "ok": return PlatformType.Ok;
					case "yandex": return PlatformType.Yandex;
					case "telegram": return PlatformType.Telegram;
					case "mock": return PlatformType.Default;
					case "vk": return PlatformType.Vk;
					default: return PlatformType.Default;
				}
#else
				return PlatformType.Default;
#endif
			}
		}


		[DllImport("__Internal")]
		private static extern string GetPlatform();

		[DllImport("__Internal")]
		private static extern void InitPlatformCommands(string commandsJson, Action<string> commandCallback);

		private static Dictionary<string, Action> customCommands;
		
		public static void InitCustomPlatformCommands(PlatformCommand[] commands)
		{
			#if UNITY_EDITOR
			return;
			#endif
			
			customCommands = commands.ToDictionary(x => x.methodName, x => x.Callback);
			InitPlatformCommands(JsonConvert.SerializeObject(commands.Select(x => x.methodName).ToArray()), OnCommandInvoke);
		}

		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnCommandInvoke(string methodName)
		{
			customCommands[methodName].Invoke();
		}

		[Serializable]
		public struct PlatformCommand
		{
			public string methodName;
			public Action Callback;

			public PlatformCommand(Action method)
			{
				methodName = method.Method.Name;
				Callback = method;
			}
		}
	}
}