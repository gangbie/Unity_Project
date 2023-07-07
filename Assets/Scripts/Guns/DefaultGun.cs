using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DefaultGun : Gun
{
    public UnityEvent OnFired;
    protected override void Awake()
    {
        base.Awake();
        data = GameManager.Resource.Load<GunData>("Data/DefaultGun");
        info = data.guns[0].info;
        bulletSpeed = data.guns[0].bulletSpeed;
        maxDistance = data.guns[0].maxDistance;
        damage = data.guns[0].damage;
        hitEffectMetal = data.guns[0].hitEffectMetal;
        hitEffectHuman = data.guns[0].hitEffectHuman;
        muzzleEffect = GetComponentInChildren<ParticleSystem>();
        bulletTrail = data.guns[0].bulletTrail;
        shooter.Get(this);
        this.name = info;
        GameManager.data.UpdateGunInfo(name);
    }
    
    public override void Fire()
    {
        base.Fire();
        OnFired?.Invoke();
    }
}
