﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace MultiFPS
{
	[RequireComponent(typeof(PlayerSetup))]
	public class PlayerManager : NetworkBehaviour
	{
		[SerializeField] private GameObject[] disableGameObjectsOnDeath;
		[SerializeField] private GameObject deathEffect;
		[SerializeField] private GameObject spawnEffect;
		[SerializeField] private Behaviour[] disableOnDeath;
		[SerializeField] private bool[] wasEnabled;
		[SerializeField] private int maxHealth = 100;

		[SyncVar] private int currentHealth;                     // unity recognizes changes and syncs them with all other clients

		[SyncVar]
		private bool _isDead = false;
		public bool isDead
		{
			get { return _isDead; }
			protected set { _isDead = value; }
		}

		// Setup Player over the server
		public void PlayerSetup()
		{
			// switch cameras
			GameManager.instance.SetSceneCameraActive(true);
			GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);

			CmdBroadcastNewPlayerSetup();
		}

		// Let Clients on the server know
		[Command]
		void CmdBroadcastNewPlayerSetup()
		{
			RpcSetupPlayerOnAllClients();
		}

		// Gets called from the server
		[ClientRpc]
		void RpcSetupPlayerOnAllClients()
		{
			wasEnabled = new bool[disableOnDeath.Length];                   // gives the wasEnabled Array the lenght of the disableOnDeath Array
			for (int i = 0; i < wasEnabled.Length; i++)                     // writes into the array if the Behaviour was enabled or not as bool
			{
				wasEnabled[i] = disableOnDeath[i].enabled;
			}

			SetDefaults();
		}

		// Method to test if player is killed
		void Update()
		{
			if (!isLocalPlayer) { return; }

			if (Input.GetKeyDown(KeyCode.K))
			{
				RpcTakeDamage(9999);
			}
		}

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

			// Reenable GameObjects
			for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
			{
				disableGameObjectsOnDeath[i].SetActive(true);
			}

			// Enable the collider
			Collider _col = GetComponent<Collider>();
			if (_col != null)
			{
				_col.enabled = true;
			}

			if (isLocalPlayer)
			{
				GameManager.instance.SetSceneCameraActive(false);
				GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
			}

			// create spawn effect
			GameObject _gfxIns = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
			Destroy(_gfxIns, 3f);
		}

		// player applies the damage to his health on the server
		[ClientRpc]
		public void RpcTakeDamage(int _amount)
		{
			if (isDead) { return; }

			currentHealth -= _amount;

			Debug.Log(transform.name + " now has " + currentHealth + "health");

			if (currentHealth <= 0)                                                  // if the current health is less or eqpual to 0, call death
			{
				Die();
			}
		}

		// disables the wanted Behaviours and collider
		void Die()
		{
			isDead = true;

			// Disable components
			for (int i = 0; i < disableOnDeath.Length; i++)
			{
				disableOnDeath[i].enabled = false;
			}

			// Disable GameObjects
			for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
			{
				disableGameObjectsOnDeath[i].SetActive(false);
			}

			// Disable the Collider
			Collider _col = GetComponent<Collider>();
			if (_col != null)
			{
				_col.enabled = true;
			}

			// switch cameras
			if (isLocalPlayer)
			{
				GameManager.instance.SetSceneCameraActive(true);
				GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
			}

			// Spawn a death effect
			GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
			Destroy(_gfxIns, 3f);

			Debug.Log(transform.name + " is dead!");

			StartCoroutine(Respawn());                                 // start coroutine for respawning
		}

		// Coroutine for Respawning the Player at a StartPosition
		IEnumerator Respawn()
		{
			yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

			Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
			transform.position = _spawnPoint.position;
			transform.rotation = _spawnPoint.rotation;

			// switch cameras
			GameManager.instance.SetSceneCameraActive(false);
			GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);

			SetDefaults();

			Debug.Log(transform.name + " respawned");
		}
	}
}


