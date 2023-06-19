using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IHittable
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Hit(RaycastHit hit, int damage)
    {
        if (rb != null)
        {
            rb.AddForceAtPosition(-10 * hit.normal, hit.point, ForceMode.Impulse);
        }
    }
}
