using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiFPS
{
	[RequireComponent(typeof(PlayerManager))]
	public class PlayerScore : MonoBehaviour
	{
		PlayerManager player;

		void Start()
		{
			player = GetComponent<PlayerManager>();
			StartCoroutine(SyncScoreLoop());
		}

		void OnDestroy()
		{
			if (player != null)
			{
				SyncNow();
			}
		}

		IEnumerator SyncScoreLoop()
		{
			while (true)
			{
				yield return new WaitForSeconds(20f);

				SyncNow();
			}
		}

		void SyncNow()
		{
			if (UserAccountManager.IsLoggedIn)
			{
				UserAccountManager.instance.GetUserData(OnDataRecieved);
			}
		}

		void OnDataRecieved(string data)
		{
			if (player.kills == 0 & player.deaths == 0) { return; }

			int _kills = UserAccountDataTranslator.DataToKills(data);
			int _deaths = UserAccountDataTranslator.DataToDeaths(data);

			int _newKills = player.kills + _kills;
			int _newDeaths = player.deaths + _deaths;

			string _newData = UserAccountDataTranslator.ValuesToData(_newKills, _newDeaths);

			player.kills = 0;
			player.deaths = 0;

			UserAccountManager.instance.LoggedIn_SaveDataButtonPressed(_newData);
		}
	}
}
