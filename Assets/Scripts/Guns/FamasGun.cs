using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FamasGun : Gun
{
    private GameObject player;
    
    public LayerMask playerMask;

    public UnityEvent OnFired;
    protected override void Awake()
    {
        // base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
        shooter = player.GetComponent<PlayerShooter>();
        data = GameManager.Resource.Load<GunData>("Data/FamasGun");
        info = data.guns[0].info;
        bulletSpeed = data.guns[0].bulletSpeed;
        maxDistance = data.guns[0].maxDistance;
        damage = data.guns[0].damage;
        hitEffectMetal = data.guns[0].hitEffectMetal;
        hitEffectHuman = data.guns[0].hitEffectHuman;
        muzzleEffect = GetComponentInChildren<ParticleSystem>();
        bulletTrail = data.guns[0].bulletTrail;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerMask) != 0)
        {
            shooter.Get(this);
            this.name = info;
            this.gameObject.SetActive(false);
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public override void Fire()
    {
        base.Fire();
        OnFired?.Invoke();
    }
}
