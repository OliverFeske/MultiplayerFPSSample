using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiFPS
{
	public class PlayerUI : MonoBehaviour
	{
		[SerializeField] private GameObject pauseMenu;
		[SerializeField] private GameObject scoreBoard;

		void Start()
		{
			PauseMenu.IsOn = false;
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				TogglePauseMenu();
			}

			if (Input.GetKeyDown(KeyCode.Tab))
			{
				scoreBoard.SetActive(true);
			}
			else if (Input.GetKeyUp(KeyCode.Tab))
			{
				scoreBoard.SetActive(false);
			}
		}

		public void TogglePauseMenu()
		{
			pauseMenu.SetActive(!pauseMenu.activeSelf);
			PauseMenu.IsOn = pauseMenu.activeSelf;
		}
	}
}
