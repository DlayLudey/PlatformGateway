using System;
using System.Collections;
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
	}
}
