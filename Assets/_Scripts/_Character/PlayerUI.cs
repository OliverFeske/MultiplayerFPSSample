using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiFPS
{
	public class PlayerUI : MonoBehaviour
	{
		[SerializeField] GameObject pauseMenu;

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
		}

		void TogglePauseMenu()
		{
			pauseMenu.SetActive(!pauseMenu.activeSelf);
			PauseMenu.IsOn = pauseMenu.activeSelf;
		}
	}
}
