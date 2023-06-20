using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    bool ApplyDamage(DamageMessage damageMessage);
    public void Hit(GameObject sender, RaycastHit hit);
}
