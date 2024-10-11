using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarrotHood.PlatformDeps.Ok
{
	[CreateAssetMenu(fileName = "OkPlatform", menuName = "Platforms/Ok")]
	public partial class OkPlatform : Platform
	{
		public override PlatformType Type => PlatformType.OK;

		public override bool CheckRelevant()
		{
			return true;
		}

		public override IEnumerator Init(PlatformBuilder builder)
		{
			yield return null;
			builder.AddPurchize(new PaymentsOk());
		}
	}

	//Здесь используем кастомный пакет
	public class PaymentsOk : IPayments
	{
		public bool isSupported => throw new NotImplementedException();

		public void ConsumePurchase(Dictionary<string, object> options, Action<bool> onComplete = null)
		{
			throw new NotImplementedException();
		}

		public void GetCatalog(Action<bool, List<Dictionary<string, string>>> onComplete = null)
		{
			throw new NotImplementedException();
		}

		public void GetPurchases(Action<bool, List<Dictionary<string, string>>> onComplete = null)
		{
			throw new NotImplementedException();
		}

		public void Purchase(Dictionary<string, object> options, Action<bool> onComplete = null)
		{
			throw new NotImplementedException();
		}
	}
}