using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AidKit : MonoBehaviour
{
    private GameObject player;
    private PlayerHealth playerHealth;
    public LayerMask playerMask;

    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.transform.gameObject.layer) & playerMask) != 0)
        {
            Heal();
            Destroy(gameObject);
        }
        
    }

    private void Heal()
    {
        playerHealth.RestoreHealth(10f);
    }
}
