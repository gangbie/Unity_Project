using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Data/Gun")]
public class GunData : ScriptableObject
{
    public GunInfo[] guns;

    [Serializable]
    public class GunInfo
    {
        public Gun gun;

        public string info;
        public float bulletSpeed;
        public float maxDistance;
        public int damage;

        public ParticleSystem hitEffectMetal;
        public ParticleSystem hitEffectHuman;
        public ParticleSystem muzzleEffect;
        public TrailRenderer bulletTrail;
    }
}
