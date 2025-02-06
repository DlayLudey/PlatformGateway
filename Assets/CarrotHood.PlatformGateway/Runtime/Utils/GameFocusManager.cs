using System;
using UnityEngine;

namespace CarrotHood.PlatformGateway
{
	public class GameFocusManager
	{
		private bool inBackground;
		public bool InBackground
		{
			get => inBackground;
			set
			{
				inBackground = value;
				CheckFocus();
			}
		}

		private bool inAdvert;
		public bool InAdvert
		{
			get => inAdvert;
			set
			{
				inAdvert = value;
				CheckFocus();
			}
		}
		
		private bool inPayments;

		public bool InPayments
		{
			get => inPayments;
			set
			{
				inPayments = value;
				CheckFocus();
			}
		}

		/// <summary>
		/// True => In focus
		/// False => Out of focus
		/// </summary>
		public Action<bool> OnGameFocusChanged;
		
		private void CheckFocus()
		{
			OnGameFocusChanged?.Invoke(!inBackground && !inAdvert && !inPayments);
		}
	}
}