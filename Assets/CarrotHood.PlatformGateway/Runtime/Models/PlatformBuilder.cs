using System.Collections;
using UnityEngine;

namespace CarrotHood.PlatformGateway
{
	public class PlatformBuilder
	{
		public PlatformBuilder() { }
		public PlatformBuilder(PlatformBase platform)
		{
			Platform = platform;
		}

		public PlatformBase Platform;
		public AdvertisementBase Advertisement;
		public PaymentsBase Payments;
		public StorageBase Storage;
		public ISocial Social;
		public ILeaderboard Leaderboard;
		public IPlayer Player;
		
		public IEnumerator Build()
		{
			if (Platform != null)
			{ 
				yield return Platform.Init(this); 
			}

			Advertisement ??= new DefaultAdvertisement(0);
			
			if (Storage == null)
			{
				Storage = new DefaultStorage(1);
				yield return Storage.Initialize();
			}
			
			Payments ??= new DefaultPayments(Storage);
			Social ??= new DefaultSocial();
			Leaderboard ??= new DefaultLeaderboard();
			Player ??= new DefaultPlayer();
		}
	}
}

