using UnityEngine;
using UnityEngine.UI;

namespace MultiFPS
{
	public class UserAccount_Lobby : MonoBehaviour
	{
		public Text usernameText;

		void Start()
		{
			if (UserAccountManager.IsLoggedIn)
			{
				usernameText.text = UserAccountManager.PlayerUsername;
			}

		}

		public void LogOut()
		{
			if (UserAccountManager.IsLoggedIn)
			{
				UserAccountManager.instance.LoggedOut();
			}
		}
	}
}
