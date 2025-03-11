using System;
using System.Collections;
using System.Collections.Generic;

namespace CarrotHood.PlatformGateway
{
	public interface IAccount
	{
		public class PlayerData
		{
			public string name;
			public string id;
			public string profilePictureUrl;
		}
		
		public PlayerData Player { get; }

		public IEnumerator GetPlayerData();
	}
}