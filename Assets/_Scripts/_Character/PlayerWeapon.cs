using UnityEngine;

namespace MultiFPS
{
    [System.Serializable]
    public class PlayerWeapon
    {
        public GameObject Graphics;

        public string Name = "Deagle";

        public int Damage = 10;
		public int MaxBullets = 20;
		[HideInInspector] public int Bullets;
        public float Range = 100f;
        public float FireRate = 0f;
		public float ReloadTime = 1f;

		public PlayerWeapon()
		{
			Bullets = MaxBullets;
		}
    }
}
