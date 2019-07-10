using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    public PlayerWeapon weapon;

    [SerializeField] private Camera cam;

    private void Start()
    {
        if(cam == null)
        {
            Debug.LogError("PlayerShoot: No Camera referred!");
            this.enabled = false;
        }
    }
}
