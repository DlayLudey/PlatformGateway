using System.Collections;
using UnityEngine;

namespace CarrotHood.PlatformGateway
{
	public class DefaultPlatform : PlatformBase
	{
		[SerializeField] private Product[] products;
		
		public override PlatformType Type => PlatformType.Default;

		public override string Language => "ru";

		public override IEnumerator Init(PlatformBuilder builder)
		{
			builder.Advertisement = new DefaultAdvertisement(interstitialCooldown);
			builder.Storage = new DefaultStorage(saveCooldown);
			yield return builder.Storage.Initialize();
			builder.Payments = new DefaultPayments(products, builder.Storage);
			builder.Social = new DefaultSocial();
		}
	}
}