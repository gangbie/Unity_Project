using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PausePopUpUI : PopUpUI
{
    protected override void Awake()
    {
        base.Awake();

        buttons["ContinueButton"].onClick.AddListener(() => { GameManager.UI.ClosePopUpUI(); });
        buttons["SettingButton"].onClick.AddListener(() => { GameManager.UI.ShowPopUpUI<PopUpUI>("UI/SettingPopUpUI"); });
        buttons["ExitButton"].onClick.AddListener(() => { GameManager.UI.ShowPopUpUI<PopUpUI>("UI/ConfigPopUpUI"); });
    }
}
