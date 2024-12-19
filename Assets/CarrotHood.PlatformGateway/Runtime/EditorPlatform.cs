using System.Collections;
using CarrotHood.PlatformGateway;
using UnityEngine;

namespace CarrotHood.PlatformGateway
{
	[CreateAssetMenu(fileName = "EditorPlatform", menuName = "Platforms/Editor")]
	public class EditorPlatform : PlatformBase
	{
		[SerializeField] private string editorLang;

		public override PlatformType Type => default;
		public override string Language => editorLang;
	
		public override IEnumerator Init(PlatformBuilder builder)
		{
			yield break;
		}
	}	
}
