using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPopUpUI : PopUpUI
{
    protected override void Awake()
    {
        base.Awake();

        buttons["SaveButton"].onClick.AddListener(() => { GoToMainScene(); });
        buttons["CancelButton"].onClick.AddListener(() => { GameManager.UI.ClosePopUpUI(); });
    }

    public void GoToMainScene()
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.UI.ClosePopUpUI();
        GameManager.Scene.LoadScene("MainScene");
    }
}
