using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameObject player;
    public LayerMask targetMask;

    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        GoToNextMap();
        Destroy(gameObject);
    }

    private void GoToNextMap()
    {
        GameManager.Scene.LoadScene("Map2");
    }
}
