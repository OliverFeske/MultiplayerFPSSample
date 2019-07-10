using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    public PlayerWeapon weapon;

    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask;

    private const string PLAYER_TAG = "Player";

    void Start()
    {
        if(cam == null)
        {
            Debug.LogError("PlayerShoot: No Camera referred!");
            this.enabled = false;
        }
    }

    void Update()
    {
        // Shoot on input
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    // Players check locally if they hit something
    [Client]
    void Shoot()
    {
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            // If the Raycast hit something that has the tag "Player", jump into the Command CmdPlayerIsShot
            if(_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerIsShot(_hit.collider.name, weapon.damage);                   // send over an id of the hit object and some damage
            }
        }
    }

    // Called on the server
    [Command]
    void CmdPlayerIsShot(string _playerID, int _damage)
    {
        Debug.Log(_playerID + " has been shot.");

        PlayerManager _player = GameManager.GetPlayer(_playerID);               // finds the playercomponent with the given id
        _player.TakeDamage(_damage);                                            // deasl damage inside a method of that player
    }
}
