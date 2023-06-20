using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
}
