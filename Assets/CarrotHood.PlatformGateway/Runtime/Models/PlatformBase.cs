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
	public abstract class PlatformBase : ScriptableObject
	{
		[SerializeField] protected float interstitialCooldown;
		[SerializeField] protected float saveCooldown;
		
		public abstract PlatformType Type { get; }
		public abstract string Language { get; }
		
		public abstract IEnumerator Init(PlatformBuilder builder);
	}

	public enum PlatformType
	{
		Default,
		Yandex,
		Telegram,
		Ok,
		Vk,
	}
}