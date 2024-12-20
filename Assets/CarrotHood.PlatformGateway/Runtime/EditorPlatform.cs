using System.Collections;
using CarrotHood.PlatformGateway;
using UnityEngine;

namespace CarrotHood.PlatformGateway
{
	[CreateAssetMenu(fileName = "EditorPlatform", menuName = "Platforms/Editor")]
	public class EditorPlatform : PlatformBase
	{
		[SerializeField] private string editorLang;
		[SerializeField] private Product[] products;

		public override PlatformType Type => default;
		public override string Language => editorLang;
	
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
