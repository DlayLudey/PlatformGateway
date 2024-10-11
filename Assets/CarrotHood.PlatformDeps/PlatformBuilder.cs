using CarrotHood.PlatformDeps;

namespace CarrotHood.PlatformDeps
{
	public class PlatformBuilder
	{
		public IAdvertisement Advertisement { get; private set; }
		public IPayments Purchases { get; private set; }
		public IStorage Saves { get; private set; }
		public ISocial Social { get; private set; }

		public void AddAdvirst(IAdvertisement avirst)
		{
			Advertisement = avirst;
		}

		public void AddPurchize(IPayments purchases)
		{
			Purchases = purchases;
		}

		public void AddSaves(IStorage saves)
		{
			Saves = saves;
		}

		public void AddSocial(ISocial social)
		{
			Social = social;
		}
	}
}

