using CarrotHood.PlatformGateway;
using UnityEngine;

public class EditorPlatform : Platform
{
	[SerializeField] private string editorLang;

	public override PlatformType Type => default;
	public override string Language => editorLang;
	public override bool CheckRelevant() => true;
}
