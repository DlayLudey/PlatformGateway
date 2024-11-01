using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CarrotHood.PlatformGateway
{
	public class PlatformGateway : MonoBehaviour
	{
		public static PlatformGateway instance;

		public static PlatformBuilder PlatformBuilder { get; private set; }

		public static AdvertisementBase Advertisement => PlatformBuilder?.Advertisement;
		public static IPayments Payments => PlatformBuilder?.Payments;
		public static IStorage Storage => PlatformBuilder?.Storage;
		public static ISocial Social => PlatformBuilder?.Social;
		public static IPlayer Player => PlatformBuilder?.Player;
		public static ILeaderboard Leaderboard => PlatformBuilder?.Leaderboard;
		public static IPlatform currentPlatform => PlatformBuilder?.Platform;
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


			if (platform == default)
			{
				PlatformBuilder = new PlatformBuilder();
				Debug.LogError($"Платформа не определена использованы настройки по умолчанию!");
			}
			else
			{
				Debug.Log($"Платформа успешна определена. Платформа:{PlatformType}");
				PlatformBuilder = new PlatformBuilder(platform);
			}

			Debug.Log($"Initializing platform: {PlatformType}");
			
			yield return PlatformBuilder.Build();

			Debug.Log("Initialize complete!");

			yield return OnInit();
		}

		protected virtual IEnumerator OnInit()
		{
			yield return null;
		}
	}
}