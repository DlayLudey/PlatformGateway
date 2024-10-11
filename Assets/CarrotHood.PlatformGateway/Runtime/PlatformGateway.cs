using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CarrotHood.PlatformGateway
{
	public class PlatformGateway : MonoBehaviour
	{
		public static PlatformGateway instance;
		public static IAdvertisement Advertisement;
		public static IPayments Payments;
		public static IStorage Storage;
		public static ISocial Social;
		public static PlatformType PlatformType { get; } = PlatformType.Default;
		
		[SerializeField] public PlatformType editorPlatformType;
		[SerializeField] private List<Platform> platforms;
		protected virtual void Awake()
		{
			instance = this;
		}

		public IEnumerator Init()
		{
#if UNITY_EDITOR
			var platformType = editorPlatformType;
#else
			var platformType = PlatformGatewayInternal.PlatformType;

#endif
			var platform = platforms.FirstOrDefault();
			var builder = new PlatformBuilder();

			if (platform != default)
				yield return platform.Init(builder);
			else
				Debug.LogWarning("The platform was not found, the default platform is used!");

			Advertisement = builder.Advertisement ?? new DefaultAdvertisement(0);

			Payments = builder.Purchases ?? new DefaultPayments();

			Storage = builder.Saves ?? new DefaultStorage();

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