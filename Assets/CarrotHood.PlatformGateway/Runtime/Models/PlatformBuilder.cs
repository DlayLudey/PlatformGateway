using CarrotHood.PlatformGateway;

namespace CarrotHood.PlatformGateway
{
	public class PlatformBuilder
	{
		public IAdvertisement Advertisement { get; private set; }
		public IPayments Purchases { get; private set; }
		public IStorage Storage { get; private set; }
		public ISocial Social { get; private set; }

		public void AddAdvertisement(IAdvertisement avirst)
		{
			Advertisement = avirst;
		}

		public void AddPayments(IPayments purchases)
		{
			Purchases = purchases;
		}

		public void AddStorage(IStorage storage)
		{
			Storage = storage;
		}

		public void AddSocial(ISocial social)
		{
			Social = social;
		}
	}
}

