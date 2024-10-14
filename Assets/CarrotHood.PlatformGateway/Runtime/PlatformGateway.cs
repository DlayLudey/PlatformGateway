using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using InstantGamesBridge.Modules.Player;
using InstantGamesBridge.Modules.Leaderboard;

namespace CarrotHood.PlatformGateway
{
	public class PlatformGateway : MonoBehaviour
	{
		public static PlatformGateway instance;
		public static IAdvertisement Advertisement;
		public static IPayments Payments;
		public static IStorage Storage;
		public static ISocial Social;
		public static IPlayer Player;
		public static ILeaderboard Leaderboard;
		public static PlatformType PlatformType { get; } = PlatformType.Default;
		
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
			var platformType = editorPlatformType;
#else
			var platformType = PlatformGatewayInternal.PlatformType;

#endif
			var platform = platforms.FirstOrDefault(platform => platform.Type == platformType);
			var builder = new PlatformBuilder();

			if (platform != default)
				yield return platform.Init(builder);
			else
				Debug.LogWarning("The platform was not found, the default platform is used!");

			Advertisement = builder.Advertisement ?? new DefaultAdvertisement(0);

			Payments = builder.Purchases ?? new DefaultPayments();

			Storage = builder.Storage ?? new DefaultStorage();

			Social = builder.Social ?? new DefaultSocial();

			Debug.Log("Initialize complete!");

			yield return OnInit();
		}

		protected virtual IEnumerator OnInit()
		{
			yield return null;
		}
	}
}