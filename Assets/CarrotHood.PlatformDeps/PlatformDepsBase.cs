using System;
using System.Collections;
using System.Collections.Generic;
using CarrotHood.PlatformDeps;
using Unity.Services.Analytics;
using UnityEngine;

namespace CarrotHood.PlatformDeps
{
	public class PlatformDepsBase: MonoBehaviour
    {
		public static PlatformDepsBase instance;
		public static IAdvertisement Advertisement;
		public static IPayments Payments;
		public static IStorage Storage;
		public static ISocial Social;

		[SerializeField] private List<Platform> platforms;

		protected virtual void Awake()
		{
			instance = this;
		}

		public IEnumerator Init()
		{
			var builder = new PlatformBuilder();
            foreach (Platform platform in platforms)
            {
                if(platform.CheckRelevant())
                {
                    yield return platform.Init(builder);
                }
            }

			Advertisement = builder.Advertisement ?? new DefaultAdvertisement(0); 

			Payments = builder.Purchases ?? new DefaultPayments();

			Storage = builder.Saves ?? new DefaultStorage();

			Social = builder.Social ?? new DefaultSocial();

			yield return OnInit();
		}

        protected virtual IEnumerator OnInit()
        {
            yield return null;
        }
	}
}