using System.Collections;

namespace CarrotHood.PlatformGateway
{
	public class PlatformBuilder
	{
		public PlatformBuilder() { }
		public PlatformBuilder(IPlatform platform)
		{
			Platform = platform;
		}

		public bool IsBuilding { get; private set; }
		public IPlatform Platform { get; private set; }
		public AdvertisementBase Advertisement { get; private set; }
		public IPayments Payments { get; private set; }
		public IStorage Storage { get; private set; }
		public ISocial Social { get; private set; }
		public ILeaderboard Leaderboard { get; private set; }
		public IPlayer Player { get; private set; }

		public void AddAdvertisement(AdvertisementBase advertisement) => Advertisement = advertisement;

		public void AddPayments(IPayments payments) => Payments = payments;

		public void AddStorage(IStorage storage) => Storage = storage;

		public void AddSocial(ISocial social) => Social = social;

		public void AddLeaderboard(ILeaderboard leaderboard) => Leaderboard = leaderboard;

		public void AddPlayer(IPlayer player) => Player = player;

		public IEnumerator Build()
		{
			if (Platform != null)
			{ 
				yield return Platform.Init(this); 
			}

			Advertisement ??= new DefaultAdvertisement(0);
			Payments ??= new DefaultPayments(Platform?.Settings);
			Storage ??= new DefaultStorage();
			Social ??= new DefaultSocial();
			Leaderboard ??= new DefaultLeaderboard();
			Player ??= new DefaultPlayer();

			yield return null;

			IsBuilding = true;
		}
	}
}

