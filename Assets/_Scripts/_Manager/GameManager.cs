using UnityEngine;
using System.Collections.Generic;

namespace MultiFPS
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public MatchSettings matchSettings;

        void Awake()
        {
            if(instance != null)
            {
                Debug.LogError("More than one GameManager in scene");
            }
            else
            {
                instance = this;
            }
        }

        #region Player tracking

        private const string PLAYER_ID_PREFIX = "Player";

        // create a dictionary to store playerIDs in
        private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();

        // registers a player into the dictionary
        public static void RegisterPlayer(string _netID, PlayerManager _player)
        {
            string _playerID = PLAYER_ID_PREFIX + _netID;           // writes the player name and his id into _playerID
            players.Add(_playerID, _player);                        // adds the created name and the PlayerManager into the Dictionary
            _player.transform.name = _playerID;                     // overrides the name of the Player with the newly created one;
        }

        // removes the Player from the Dictionary
        public static void UnRegisterPlayer(string _playerID)
        {
            players.Remove(_playerID);
        }

        public static PlayerManager GetPlayer(string _playerID)
        {
            return players[_playerID];
        }

        //void OnGUI()
        //{
        //    GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        //    GUILayout.BeginVertical();

        //    foreach (string _playerID in players.Keys)
        //    {
        //        GUILayout.Label(_playerID + "  -  " + players[_playerID].transform.name);
        //    }

        //    GUILayout.EndVertical();
        //    GUILayout.EndArea();
        //}

        #endregion
    }

}
