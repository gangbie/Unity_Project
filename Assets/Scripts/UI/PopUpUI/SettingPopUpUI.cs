using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopUpUI : PopUpUI
{
    private Slider slider;
    protected override void Awake()
    {
        base.Awake();

        slider = GetComponent<Slider>();

        buttons["SaveButton"].onClick.AddListener(() => { GameManager.UI.ClosePopUpUI(); });
        buttons["CancelButton"].onClick.AddListener(() => { GameManager.UI.ClosePopUpUI(); });
    }
}
