using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiFPS
{
	public class Scoreboard : MonoBehaviour
	{
		void OnEnable()
		{
			PlayerManager[] players = GameManager.GetAllPlayers();

			foreach (PlayerManager player in players)
			{

			}
		}

		void OnDisable()
		{

		}
	}
}
