﻿using System;
using System.Collections;
using UnityEngine;

namespace CarrotHood.PlatformGateway
{
	public abstract class Platform : ScriptableObject
    {
        public PlatformSettings Settings;
        public abstract PlatformType Type { get; }
        public abstract string Language { get; }
        public abstract bool CheckRelevant();
        public virtual IEnumerator Init(PlatformBuilder baseDeps)
        {
            yield return null;
        }
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
		public int StartLevelInter = 0;
		public bool RewardClicker = false;
		public bool InterClicker = false;
		public int interstitialCooldown = 60;
	}
}