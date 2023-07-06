using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] public Gun gun;

    public List<Gun> items = new List<Gun>();

    public void Fire()
    {
        gun.Fire();
    }

    public void Swap(Gun changeGun)
    {
        Gun prevGun = gun;
        gun = changeGun;
        gun.transform.position = prevGun.transform.position;
        gun.transform.rotation = prevGun.transform.rotation;
        prevGun.gameObject.SetActive(false);
        changeGun.gameObject.SetActive(true);
    }


}
