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

            // if the fire rate is less or equal to 0 shoot by clicking
            if(currentWeapon.fireRate <= 0f)
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
                    InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
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

            if (!isLocalPlayer) { return; }

            CmdOnShoot();

            RaycastHit _hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
            {
                // If the Raycast hit something that has the tag "Player", jump into the Command CmdPlayerIsShot
                if (_hit.collider.tag == PLAYER_TAG)
                {
                    CmdPlayerIsShot(_hit.collider.name, currentWeapon.damage);                   // send over an id of the hit object and some damage
                }
            }
        }

        // Is called on the server whan a player shoots
        [Command]
        void CmdOnShoot()
        {
            RpcDoShootEffect();
        }

        // Is called on all clients when we need to do a shoot effect
        [ClientRpc]
        void RpcDoShootEffect()
        {
            weaponManager.GetCurrentGraphics().muzzleFlash.Play();
        }


        // Called on the server
        [Command]
        void CmdPlayerIsShot(string _playerID, int _damage)
        {
            Debug.Log(_playerID + " has been shot.");

            PlayerManager _player = GameManager.GetPlayer(_playerID);               // finds the playercomponent with the given id
            _player.RpcTakeDamage(_damage);                                         // deals damage inside a method of that player
        }
    }
}
