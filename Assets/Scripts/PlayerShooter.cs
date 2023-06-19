using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] Gun gun;
    [SerializeField] float reloadTime;
    [SerializeField] Rig aimRig;
    [SerializeField] WeaponHolder weaponHolder;

    private Animator anim;
    private bool isReloading;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        gun = GetComponentInChildren<Gun>();
    }

    private void OnReload(InputValue value)
    {
        if (isReloading) return;

        Reload();
    }

    IEnumerator ReloadingRoutine()
    {
        anim.SetTrigger("Reload");
        isReloading = true;
        aimRig.weight = 0f;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        aimRig.weight = 1f;
    }

    public void Reload()
    {
        StartCoroutine(ReloadingRoutine());
        gun.bulletUsed = 0;
        gun.bulletRemain = gun.bulletCapacity;
    }

    public void Fire()
    {
        weaponHolder.Fire();
        anim.SetTrigger("Fire");

    }

    private void OnFire(InputValue value)
    {
        if (isReloading)
            return;

        Fire();
    }
}
