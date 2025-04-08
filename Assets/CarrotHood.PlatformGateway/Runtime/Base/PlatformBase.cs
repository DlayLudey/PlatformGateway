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
		[SerializeField] protected float interstitialCooldown = 60;
		[SerializeField] protected float saveCooldown = 4;
		
		public abstract PlatformType Type { get; }
		public abstract string Language { get; }
		public virtual DateTime CurrentTime => DateTime.Now;

		/// <summary>
		/// True => In focus
		/// False => Out of focus
		/// </summary>
		public Action<bool> OnGameFocusChanged;
		
		public abstract IEnumerator Init(PlatformBuilder builder);
		public virtual void GameReady(){}
	}

	public enum PlatformType
	{
		Default,
		Yandex,
		Telegram,
		Ok,
		Vk,
		Playgama
	}
}