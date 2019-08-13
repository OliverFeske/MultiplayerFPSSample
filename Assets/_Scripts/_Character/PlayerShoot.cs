using UnityEngine;
using UnityEngine.Networking;

namespace MultiFPS
{
	[RequireComponent(typeof(WeaponManager))]
	public class PlayerShoot : NetworkBehaviour
	{
		[SerializeField] private Camera cam;
		[SerializeField] private LayerMask mask;

		private PlayerWeapon currentWeapon;
		private WeaponManager weaponManager;

		private const string PLAYER_TAG = "Player";

		void Start()
		{
			// if there is no camera attached disable this
			if (cam == null)
			{
				Debug.LogError("PlayerShoot: No Camera referred!");
				this.enabled = false;
			}

			weaponManager = GetComponent<WeaponManager>();
		}

		void Update()
		{
			currentWeapon = weaponManager.GetCurrentWeapon();

			if (PauseMenu.IsOn) { return; }

			if (currentWeapon.Bullets < currentWeapon.MaxBullets)
			{
				if (Input.GetButtonDown("Reload"))
				{
					weaponManager.Reload();
					return;
				}
			}

			// if the fire rate is less or equal to 0 shoot by clicking
			if (currentWeapon.FireRate <= 0f)
			{
				if (Input.GetButtonDown("Fire1"))
				{
					Shoot();
				}
			}
			// if the fire rate is higher than 0 shoot at a rate
			else
			{
				if (Input.GetButtonDown("Fire1"))
				{
					InvokeRepeating("Shoot", 0f, 1f / currentWeapon.FireRate);
				}
				else if (Input.GetButtonUp("Fire1"))
				{
					CancelInvoke("Shoot");
				}
			}

		}
		// Players check locally if they hit something
		[Client]
		void Shoot()
		{
			Debug.Log("Shoot!");

			if (!isLocalPlayer && !weaponManager.isReloading) { return; }

			if (currentWeapon.Bullets <= 0)
			{
				weaponManager.Reload();
				return;
			}

			currentWeapon.Bullets--;

			CmdOnShoot();

			RaycastHit _hit;
			if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.Range, mask))
			{
				// If the Raycast hit something that has the tag "Player", jump into the Command CmdPlayerIsShot
				if (_hit.collider.tag == PLAYER_TAG)
				{
					CmdPlayerIsShot(_hit.collider.name, currentWeapon.Damage, transform.name);                   // send over an id of the hit object and some damage
				}

				// hit something and call CmdOnHit on server
				CmdOnHit(_hit.point, _hit.normal);
			}
		}

		// Is called on the server whan a player shoots
		[Command]
		void CmdOnShoot()
		{
			RpcDoShootEffect();
		}

		// Is called on the server when something is hit
		// takes in the hit point and the normal of the surface
		[Command]
		void CmdOnHit(Vector3 _pos, Vector3 _normal)
		{
			RpcDoHitEffect(_pos, _normal);
		}

		// Is called on all clients when we need to do a shoot effect
		[ClientRpc]
		void RpcDoShootEffect()
		{
			weaponManager.GetCurrentGraphics().muzzleFlash.Play();
		}

		// Is called on all clients
		// spawn in effects
		[ClientRpc]
		void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
		{
			GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
			Destroy(_hitEffect, 2f);
		}

		// Called on the server
		[Command]
		void CmdPlayerIsShot(string _playerID, int _damage, string _sourceID)
		{
			Debug.Log(_playerID + " has been shot.");

			PlayerManager _player = GameManager.GetPlayer(_playerID);               // finds the playercomponent with the given id
			_player.RpcTakeDamage(_damage, _sourceID);                                         // deals damage inside a method of that player
		}
	}
}
