using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;

    [SyncVar] private int currentHealth;                     // unity recognizes changes and syncs them with all other clients

    void Awake()
    {
        SetDefaults();
    }

    // player applies the damage to his health locally
    public void TakeDamage(int _amount)
    {
        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + "health");
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }
}
