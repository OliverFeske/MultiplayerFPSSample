using UnityEngine;
using UnityEngine.Networking;

namespace MultiFPS
{
    [RequireComponent(typeof(PlayerManager))]
    public class PlayerSetup : NetworkBehaviour
    {
        [SerializeField] Behaviour[] componentsToDisable;

        [SerializeField] private string remotePlayerLayer = "RemotePlayer";

        private Camera sceneCamera;

        void Start()
        {
            if (!isLocalPlayer)
            {
                DisableComponents();                              // if this is not the localPlayer disable some components 
                AssignRemoteLayer();                              // if this is not the localPlayer assign him another Layer
            }
            // disable the sceneCamera if there is a localPlayer
            else
            {
                sceneCamera = Camera.main;
                if (sceneCamera != null)
                {
                    sceneCamera.gameObject.SetActive(false);
                }
            }

            GetComponent<PlayerManager>().Setup();
        }

        // when the client is connected do this
        public override void OnStartClient()
        {
            base.OnStartClient();

            // get the _netID and the _player script of our player and give it to RegisterPlayer
            string _netID = GetComponent<NetworkIdentity>().netId.ToString();
            PlayerManager _player = GetComponent<PlayerManager>();

            GameManager.RegisterPlayer(_netID, _player);
        }

        // disable components 
        void DisableComponents()
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }

        // 
        void AssignRemoteLayer()
        {
            gameObject.layer = LayerMask.NameToLayer(remotePlayerLayer);                  // assign the new layer to the gameobject
        }

        // when the Player disconnects activate the sceneCamera and unregister the player
        void OnDisable()
        {
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(true);
            }

            GameManager.UnRegisterPlayer(transform.name);
        }
    }
}
