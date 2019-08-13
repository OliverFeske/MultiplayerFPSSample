using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace MultiFPS
{
    public class WeaponManager : NetworkBehaviour
    {
		public bool isReloading = false;

        [SerializeField] private string weaponLayerName = "Weapon";
        [SerializeField] private PlayerWeapon primaryWeapon;
        [SerializeField] private Transform weaponHolder;

        private PlayerWeapon currentWeapon;
        private WeaponGraphics currentGraphics;

        void Start()
        {
            EquipWeapon(primaryWeapon);
        }

        public PlayerWeapon GetCurrentWeapon()
        {
            return currentWeapon;
        }

        public WeaponGraphics GetCurrentGraphics()
        {
            return currentGraphics;
        }

        void EquipWeapon(PlayerWeapon _weapon)
        {
            currentWeapon = _weapon;

            GameObject _weaponIns = (GameObject)Instantiate(_weapon.Graphics, weaponHolder.position, weaponHolder.rotation);
            _weaponIns.transform.SetParent(weaponHolder);

            currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
            if(currentGraphics == null)
            {
                Debug.LogError("No WeaponGraphics component on the weapon object" + _weaponIns.name);
            }

            if (isLocalPlayer)
            {
                Utility.SetLayerRecursively(_weaponIns, LayerMask.NameToLayer(weaponLayerName));
            }
        }

		public void Reload()
		{
			if (isReloading) { return; }
			StartCoroutine(ReloadCoroutine());
		}

		private IEnumerator ReloadCoroutine()
		{
			isReloading = true;

			//CmdOnReload();

			yield return new WaitForSeconds(currentWeapon.ReloadTime);

			currentWeapon.Bullets = currentWeapon.MaxBullets;

			isReloading = false;
		}

		// does not work, animations break it

		//[Command]
		//void CmdOnReload()
		//{
		//	RpcOnReload();
		//}

		//[ClientRpc]
		//void RpcOnReload()
		//{
		//	Animator _anim = currentGraphics.GetComponent<Animator>();
		//	if(_anim != null)
		//	{
		//		_anim.SetTrigger("Reloading");
		//	}
		//}
    }
}
