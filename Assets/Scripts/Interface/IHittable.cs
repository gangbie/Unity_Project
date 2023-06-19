using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    public void Hit(RaycastHit hit, int damage);
}
