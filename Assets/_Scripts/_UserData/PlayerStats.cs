using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MultiFPS
{
	public class PlayerStats : MonoBehaviour
	{
		public Text killCount;
		public Text deathCount;

		void Start()
		{
			if (UserAccountManager.IsLoggedIn)
			{
				UserAccountManager.instance.GetUserData(OnReceivedData);
			}
		}

		void OnReceivedData(string data)
		{
			killCount.text = UserAccountDataTranslator.DataToKills(data).ToString() + " Kills";
			deathCount.text = UserAccountDataTranslator.DataToDeaths(data).ToString() + " Deaths";
		}
	}
}
