using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiFPS
{
	public class Scoreboard : MonoBehaviour
	{
		[SerializeField] private GameObject playerScoreBoardItemPrefab;
		[SerializeField] private Transform playerScoreboardList;

		void OnEnable()
		{
			PlayerManager[] players = GameManager.GetAllPlayers();

			foreach (PlayerManager player in players)
			{
				GameObject itemGO = Instantiate(playerScoreBoardItemPrefab, playerScoreboardList);
				PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();
				if(item != null)
				{
					item.Setup(player.Username, player.Kills, player.Deaths);
				}
			}
		}

		void OnDisable()
		{
			foreach (Transform child in playerScoreboardList)
			{
				Destroy(child.gameObject);
			}
		}
	}
}
