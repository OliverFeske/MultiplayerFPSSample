using UnityEngine;

namespace MultiFPS
{
    [System.Serializable]
    public class PlayerWeapon
    {
        public string name = "Deagle";

        public int damage = 10;
        public float range = 100f;
        public float fireRate = 0f;


        public GameObject graphics;
    }
}
