using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IHittable
{
    public float startingHealth = 100f;
    public float health { get; protected set; }
    public bool dead { get; protected set; }

    public event Action OnDeath;

    private const float minTimeBetDamaged = 0.1f;
    private float lastDamagedTime;

    protected bool IsInvulnerable
    {
        get
        {
            if (Time.deltaTime <= lastDamagedTime + minTimeBetDamaged) return false;

            return true;
        }
    }

    private void OnEnable()
    {
        dead = false;
        health = startingHealth;
    }

    public virtual void Die()
    {
        if (OnDeath != null) OnDeath();

        dead = true;
    }

    public void Hit(RaycastHit hit, int damage)
    {
        // if (IsInvulnerable || dead)

        lastDamagedTime = Time.deltaTime;
        health -= damage;

        if (health <= 0) Die();

        // return true;
    }
    
}
