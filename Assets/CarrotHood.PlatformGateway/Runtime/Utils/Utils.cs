using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;
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
				onSuccessCallback?.Invoke(null);
			}
			else
			{
				Texture2D texture2D = DownloadHandlerTexture.GetContent(www);
				onSuccessCallback?.Invoke(texture2D);
			}
		}

		[DllImport("__Internal")]
		private static extern void Svg2Png(string svgUrl, int width, int height, Action<string> base64);

		private static Action<Texture2D> onDownloadSvgSuccessCallback;
		
		public static void DownloadSvg(string url, Action<Texture2D> onSuccessCallback, int width = 256, int height = 256)
		{
			onDownloadSvgSuccessCallback = onSuccessCallback;
			
			#if UNITY_WEBGL && !UNITY_EDITOR
			Svg2Png(url, 256, 256, OnDownloadSvg);
			#else
			Debug.LogError("Not in WebGl");
			#endif
		}

		[MonoPInvokeCallback(typeof(Action<string>))]
		private static void OnDownloadSvg(string base64)
		{
			byte[] pngData = Convert.FromBase64String(base64.Substring(base64.IndexOf(",") + 1));

			Texture2D tex = new Texture2D(2, 2);
			tex.LoadImage(pngData);

			onDownloadSvgSuccessCallback?.Invoke(tex);
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
