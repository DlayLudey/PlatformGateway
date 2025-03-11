using System.Collections;
using static CarrotHood.PlatformGateway.IAccount;

namespace CarrotHood.PlatformGateway
{
	public class DefaultAccount : IAccount
	{
		public PlayerData Player { get; } = new()
		{
			id = "123456789",
			name = "John Doe",
			profilePictureUrl = "https://loremflickr.com/128/128"
		};

		public IEnumerator GetPlayerData()
		{
			yield return null;
		}
	}
}