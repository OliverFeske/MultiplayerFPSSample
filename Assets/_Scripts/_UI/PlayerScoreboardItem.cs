using UnityEngine;
using UnityEngine.UI;

namespace MultiFPS
{
	public class PlayerScoreboardItem : MonoBehaviour
	{
		[SerializeField] private Text usernameText;
		[SerializeField] private Text killsText;
		[SerializeField] private Text deathsText;

		public void Setup(string _username, int _kills, int _deaths)
		{
			usernameText.text = _username;
			killsText.text = "Kills: " + _kills;
			deathsText.text = "Deaths: " + _deaths;
		}
	}
}
