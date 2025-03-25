using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CarrotHood.PlatformGateway
{
	public class PlatformGateway : MonoBehaviour
	{
		private static PlatformGateway cachedInstance;
		public static PlatformGateway Instance
		{
			get
			{
				cachedInstance ??= FindObjectOfType<PlatformGateway>();

				if (cachedInstance == null)
					throw new NullReferenceException("Platform Gateway object was not found!");

				return cachedInstance;
			}
		}

		public static PlatformBuilder PlatformBuilder { get; private set; }

		public static AdvertisementBase Advertisement => PlatformBuilder?.Advertisement;
		public static PaymentsBase Payments => PlatformBuilder?.Payments;
		public static StorageBase Storage => PlatformBuilder?.Storage;
		public static ISocial Social => PlatformBuilder?.Social;
		public static IAccount Account => PlatformBuilder?.Account;
		public static PlatformBase CurrentPlatform => PlatformBuilder?.Platform;
		public static PlatformType PlatformType { get; private set; } = PlatformType.Default;

		[SerializeField] public PlatformType forcedPlatformType;
		[SerializeField] private List<PlatformBase> platforms;

		protected virtual void Awake()
		{
			if(cachedInstance != null && cachedInstance != this)
			{
				Destroy(gameObject);
				return;
			}
			
			
			cachedInstance = this;
			DontDestroyOnLoad(gameObject);
		}

		public IEnumerator Init()
		{
#if UNITY_EDITOR
			PlatformType = forcedPlatformType;
#else
			PlatformType = forcedPlatformType == PlatformType.Default ? PlatformGatewayInternal.PlatformType : forcedPlatformType;
#endif
			PlatformBase platform = platforms.FirstOrDefault(platform => platform.Type == PlatformType);


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

			Debug.Log($"Initializing platform: {platform!.Type}");
			
			yield return PlatformBuilder.Build();

			Debug.Log("Initialize complete!");
		}
	}
}