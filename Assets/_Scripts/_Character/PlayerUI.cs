using UnityEngine;
using UnityEngine.UI;

namespace MultiFPS
{
	public class PlayerUI : MonoBehaviour
	{
		[SerializeField] private GameObject pauseMenu;
		[SerializeField] private GameObject scoreBoard;
		[SerializeField] private RectTransform healthFill;
		[SerializeField] private Text ammoText;

		private PlayerInputController controller;
		private PlayerManager player;
		private WeaponManager weaponManager;

		public void SetPlayer(PlayerManager _player)
		{
			player = _player;
			controller = player.GetComponent<PlayerInputController>();
			weaponManager = player.GetComponent<WeaponManager>();
		}

		void Start()
		{
			PauseMenu.IsOn = false;
		}

		void Update()
		{
			SetHealthAmount(player.GetHealthPct());
			SetAmmoAmount(weaponManager.GetCurrentWeapon().Bullets, weaponManager.GetCurrentWeapon().MaxBullets);

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

		void SetHealthAmount(float _amount)
		{
			healthFill.localScale = new Vector3(_amount, 1f, 1f);
		}

		void SetAmmoAmount(int _currentAmount, int _maxAmount)
		{
			ammoText.text = _currentAmount.ToString() + " / " + _maxAmount.ToString();
		}

		public void TogglePauseMenu()
		{
			pauseMenu.SetActive(!pauseMenu.activeSelf);
			PauseMenu.IsOn = pauseMenu.activeSelf;
		}
	}
}
