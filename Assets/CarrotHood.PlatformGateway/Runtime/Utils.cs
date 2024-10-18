using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace CarrotHood.PlatformGateway
{

	public static class Utils
	{
		public static IEnumerator DownloadSprite(string url, Action<Texture2D> onSuccessCallback)
		{
			UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
			yield return www.SendWebRequest();
			if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
			{
				Debug.Log($"Error downloading texture: {www.error}");
			}
			else
			{
				Texture2D texture2D = DownloadHandlerTexture.GetContent(www);
				onSuccessCallback?.Invoke(texture2D);
			}
		}

		public static Sprite TextureToSprite(Texture2D texture)
		{
			return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		}

		public static void MockSpriteAssetTexture(TMP_SpriteAsset spriteAsset, Texture2D texture)
		{
			texture.wrapMode = TextureWrapMode.Clamp;
			
			spriteAsset.material.mainTexture = texture;

			float aspectRatio = texture.width / (float)texture.height;

			Vector2 textureScale = new (aspectRatio > 1 ? 1 : 1 / aspectRatio, aspectRatio < 1 ? 1 : aspectRatio);
			
			spriteAsset.material.mainTextureScale = textureScale;
			spriteAsset.material.mainTextureOffset = new Vector2(1 - textureScale.x, 1 - textureScale.y);
		}
	}
}
