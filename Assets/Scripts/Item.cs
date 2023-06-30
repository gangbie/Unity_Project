using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IGettable
{
    public Collider col;

    private string name;
    private float price;

    private List<Item> items;

    private void Awake()
    {
        col = GetComponent<Collider>();
        items = new List<Item>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Get();
        Destroy(this.gameObject);
    }

    public void Get()
    {
        items.Add(this);
    }

    
}
