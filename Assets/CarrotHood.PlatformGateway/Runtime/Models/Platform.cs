using System;
using System.Collections;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Linq;
#endif

namespace CarrotHood.PlatformGateway
{
	public abstract class Platform : ScriptableObject, IPlatform
	{
		[SerializeField] private PlatformSettings settings;
		public PlatformSettings Settings => settings;
		public abstract PlatformType Type { get; }
		public abstract string Language { get; }

		public virtual IEnumerator Init(PlatformBuilder baseDeps)
		{
			yield return null;
		}

#if UNITY_EDITOR
		[ContextMenu("ImportProduct")]
		public void ImportProgucts()
		{
			var productJson = JsonUtility.ToJson(settings.products);
			if (!settings.products.Any())
			{
				Debug.Log("Сначала необходимо заполнить объект продуктами");
				return;
			}

			var path = EditorUtility.SaveFilePanel("Products", "", "product", "json");
			
			if (string.IsNullOrEmpty(path))
				return;

			File.WriteAllText(path, productJson);
		}
#endif
	}

	public enum PlatformType
	{
		Default,
		Yandex,
		Telegram,
		OK,
		VK,
		Playgama,
		CrazyGames,
		Playdeck,
		Wortal,
		GameDistribution,
		AbsoluteGames,
		VKPlay,
	}

	[Serializable]
	public partial class PlatformSettings
	{
		[Header("Advertisement")]
		public int startLevelInter = 0;
		public bool rewardClicker = false;
		public bool interClicker = false;
		public int interstitialCooldown = 60;

		[Header("Payments")]
		public IPayments.Product[] products;
	}
}