using CarrotHood.PlatformGateway;
using UnityEngine;

namespace CarrotHood.PlatformGateway
{
	[CreateAssetMenu(fileName = "EditorPlatform", menuName = "Platforms/Editor")]
	public class EditorPlatform : Platform
	{
		[SerializeField] private string editorLang;

		public override PlatformType Type => default;
		public override string Language => editorLang;
		public override bool CheckRelevant() => true;
	}	
}
