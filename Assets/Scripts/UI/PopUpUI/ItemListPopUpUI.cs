using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemListPopUpUI : PopUpUI
{
    private GameObject player;
    private WeaponHolder weaponHolder;
    private PlayerShooter shooter;

    private TMP_Text Gun1Text;
    private TMP_Text Gun2Text;


    protected override void Awake()
    {
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player");
        weaponHolder = player.GetComponentInChildren<WeaponHolder>();
        shooter = player.GetComponent<PlayerShooter>();

        buttons["Gun1Button"].onClick.AddListener(() => { shooter.SwapGun(weaponHolder.items[0]); });
        buttons["Gun2Button"].onClick.AddListener(() => { shooter.SwapGun(weaponHolder.items[1]); });
        buttons["Button1"].onClick.AddListener(() => { GameManager.UI.ClosePopUpUI(); });
        buttons["Button2"].onClick.AddListener(() => { GameManager.UI.ClosePopUpUI(); });
        buttons["Button3"].onClick.AddListener(() => { GameManager.UI.ClosePopUpUI(); });
    }

    private void OnEnable()
    {
        texts["Gun1Text"].text = weaponHolder.items[0].name.ToString();
        if (weaponHolder.items.Count < 2)
        {
            texts["Gun2Text"].text = "empty";
        }
        else
        {
            texts["Gun2Text"].text = weaponHolder.items[1].name.ToString();
        }
    }
}
