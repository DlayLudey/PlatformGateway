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
		public static AdvertisementBase Advertisement;
		public static IPayments Payments;
		public static IStorage Storage;
		public static ISocial Social;
		public static IPlayer Player;
		public static ILeaderboard Leaderboard;
		public static PlatformType PlatformType { get; } = PlatformType.Default;
		public static Platform currentPlatform;
		
		[SerializeField] public PlatformType editorPlatformType;
		[SerializeField] private EditorPlatform editorPlatform;
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
			currentPlatform = platforms.FirstOrDefault(platform => platform.Type == platformType);
			var builder = new PlatformBuilder();

			if (currentPlatform == default)
				currentPlatform = editorPlatform;
			
			yield return currentPlatform.Init(builder);
			
			Debug.Log($"Initializing platform: {platformType}");
			
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