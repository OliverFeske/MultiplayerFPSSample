using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiFPS
{
	[RequireComponent(typeof(PlayerManager))]
	public class PlayerScore : MonoBehaviour
	{
		private int lastKills = 0;
		private int lastDeaths = 0;

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
			if(player.Kills <= lastKills && player.Deaths <= lastDeaths) { return; }

			int _killsSinceLast = player.Kills - lastKills;
			int _deathsSinceLast = player.Deaths - lastDeaths;

			if (_killsSinceLast == 0 && _deathsSinceLast == 0) { return; }

			int _kills = UserAccountDataTranslator.DataToKills(data);
			int _deaths = UserAccountDataTranslator.DataToDeaths(data);

			int _newKills = _killsSinceLast + _kills;
			int _newDeaths = _deathsSinceLast + _deaths;

			string _newData = UserAccountDataTranslator.ValuesToData(_newKills, _newDeaths);

			lastKills = player.Kills;
			lastKills = player.Deaths;

			UserAccountManager.instance.LoggedIn_SaveDataButtonPressed(_newData);
		}
	}
}
