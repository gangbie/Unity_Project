using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class LivingEntity : MonoBehaviour, IHittable
{
    private NavMeshAgent agent;
    private Rigidbody rb;

    public float startingHealth = 100f;
    public float health { get; protected set; }
    public bool dead { get; protected set; }

    public event Action OnDeath;

    private const float minTimeBetDamaged = 0.1f;
    private float lastDamagedTime;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    protected bool IsInvulnerable
    {
        get
        {
            if (Time.deltaTime <= lastDamagedTime + minTimeBetDamaged) return false;

            return true;
        }
    }

    protected virtual void OnEnable()
    {
        dead = false;
        health = startingHealth;
    }

    public virtual bool ApplyDamage(DamageMessage damageMessage)
    {
        if (IsInvulnerable || damageMessage.damager == gameObject || dead) return false;

        lastDamagedTime = Time.time;
        health -= damageMessage.amount;
        if (health <= 0) { Die(); }

        return true;
    }

    public virtual void RestoreHealth(float newHealth)
    {
        if (dead) return;

        health += newHealth;
    }

    public virtual void Die()
    {
        if (OnDeath != null) OnDeath();

        dead = true;
    }

    public void Hit(GameObject sender, RaycastHit hit)
    {
        Vector3 dir = (transform.position - sender.transform.position).normalized;
        agent.Move(2f * dir);
        /*if (rb != null)
        {
            rb.AddForceAtPosition(-10 * hit.normal, hit.point, ForceMode.Impulse);
        }*/
    }
}