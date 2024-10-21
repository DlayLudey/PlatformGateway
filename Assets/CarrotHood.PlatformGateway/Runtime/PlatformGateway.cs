using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CarrotHood.PlatformGateway
{
	public class PlatformGateway : MonoBehaviour
	{
		public static PlatformGateway instance;

		private static PlatformBuilder _platformBuilder;
		private static PlatformBuilder PlatformBuilder
		{
			get
			{
				if (_platformBuilder == null)
				{ 
					_platformBuilder = new PlatformBuilder();
					Debug.LogWarning("Т.к. небыло инициализации установлена платформа по умолчанию!");
					_platformBuilder.Build();
				}
				if (!PlatformBuilder.IsBuilding)
					throw new System.Exception("Вызов функций платформы до её инициализации!");
				return _platformBuilder;
			}
		}

		public static AdvertisementBase Advertisement => PlatformBuilder.Advertisement;
		public static IPayments Payments => PlatformBuilder.Payments;
		public static IStorage Storage => PlatformBuilder.Storage;
		public static ISocial Social => PlatformBuilder.Social;
		public static IPlayer Player => PlatformBuilder.Player;
		public static ILeaderboard Leaderboard => PlatformBuilder.Leaderboard;
		public static IPlatform currentPlatform => PlatformBuilder.Platform;
		public static PlatformType PlatformType { get; private set; } = PlatformType.Default;

		[SerializeField] public PlatformType editorPlatformType;
		[SerializeField] private List<Platform> platforms;

		protected virtual void Awake()
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		public IEnumerator Init()
		{
#if UNITY_EDITOR
			PlatformType = editorPlatformType;
#else
			PlatformType = PlatformGatewayInternal.PlatformType;
#endif
			var platform = platforms.FirstOrDefault(platform => platform.Type == PlatformType);


			if (platform.Type == PlatformType.Default && currentPlatform == default)
			{
				_platformBuilder = new PlatformBuilder();
				Debug.LogError($"Платформа не определена использованы настройки по умолчанию!");
			}
			else if (currentPlatform == default)
			{
				Debug.LogError($"Найстройки для платформы не найдены. Платформа:{PlatformType}");
				_platformBuilder = new PlatformBuilder();
			}
			else
			{
				Debug.Log($"Платформа успешна определена. Платформа:{PlatformType}");
				_platformBuilder = new PlatformBuilder(platform);
			}

			Debug.Log($"Initializing platform: {PlatformType}");
			
			yield return _platformBuilder.Build();

			Debug.Log("Initialize complete!");

			yield return OnInit();
		}

		protected virtual IEnumerator OnInit()
		{
			yield return null;
		}
	}
}