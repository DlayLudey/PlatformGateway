using CarrotHood.PlatformDeps;

namespace CarrotHood.PlatformDeps
{
	public class PlatformBuilder
	{
		public IAdvertisement Advertisement { get; private set; }
		public IPayments Purchases { get; private set; }
		public IStorage Saves { get; private set; }
		public ISocial Social { get; private set; }

		public void AddAdvertisement(IAdvertisement avirst)
		{
			Advertisement = avirst;
		}

		public void AddPayments(IPayments purchases)
		{
			Purchases = purchases;
		}

		public void AddStorage(IStorage saves)
		{
			Saves = saves;
		}

		public void AddSocial(ISocial social)
		{
			Social = social;
		}
	}
}

