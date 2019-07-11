using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace MultiFPS
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private Behaviour[] disableOnDeath;
        [SerializeField] private bool[] wasEnabled;

        [SyncVar]
        private bool _isDead = false;
        public bool isDead
        {
            get { return _isDead; }
            protected set { _isDead = value; }
        }

        [SyncVar] private int currentHealth;                     // unity recognizes changes and syncs them with all other clients

        // Setup for the player to take damage or die
        public void Setup()
        {
            wasEnabled = new bool[disableOnDeath.Length];                   // gives the wasEnabled Array the lenght of the disableOnDeath Array
            for (int i = 0; i < wasEnabled.Length; i++)                     // writes into the array if the Behaviour was enabled or not as bool
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }

            SetDefaults();
        }

        //// Method to test if player is killed
        //void Update()
        //{
        //    if (!isLocalPlayer) { return; }

        //    if (Input.GetKeyDown(KeyCode.K))
        //    {
        //        RpcTakeDamage(9999);
        //    }
        //}

        // Restores the default values for a Player
        public void SetDefaults()
        {
            isDead = false;

            currentHealth = maxHealth;

            // sets the disableOnDeath components to their default value, that was stored in wasEnabled
            for (int i = 0; i < disableOnDeath.Length; i++)         
            {
                disableOnDeath[i].enabled = wasEnabled[i];
            }

            Collider _col = GetComponent<Collider>();
            if(_col != null)
            {
                _col.enabled = true;
            }
        }

        // player applies the damage to his health on the server
        [ClientRpc]
        public void RpcTakeDamage(int _amount)
        {
            if (isDead) { return; }

            currentHealth -= _amount;

            Debug.Log(transform.name + " now has " + currentHealth + "health");

            if(currentHealth <= 0)                                                  // if the current health is less or eqpual to 0, call death
            {
                Die();
            }
        }

        // disables the wanted Behaviours and collider
        void Die()
        {
            isDead = true;

            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                disableOnDeath[i].enabled = false;
            }

            Collider _col = GetComponent<Collider>();
            if (_col != null)
            {
                _col.enabled = true;
            }

            Debug.Log(transform.name + " is dead!");

            StartCoroutine(Respawn());                                 // start coroutine for respawning
        }

        // Coroutine for Respawning the Player at a StartPosition
        IEnumerator Respawn()
        {
            yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

            SetDefaults();
            Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
            transform.position = _spawnPoint.position;
            transform.rotation = _spawnPoint.rotation;

            Debug.Log(transform.name + " respawned");
        }
    }
}


