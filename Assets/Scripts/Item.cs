using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour //, IGettable
{
    private GameObject player;
    private PlayerShooter shooter;

    private string name;
    private float price;

    // public List<Item> items = new List<Item>();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shooter = player.GetComponent<PlayerShooter>();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        // this.shooter.Get(this);
        // Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

    

    
}
