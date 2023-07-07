using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private GameObject player;
    private PlayerShooter shooter;

    private string name;
    private float price;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shooter = player.GetComponent<PlayerShooter>();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        this.gameObject.SetActive(false);
    }
}
