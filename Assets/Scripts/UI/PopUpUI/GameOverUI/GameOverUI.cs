using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : PopUpUI
{
    protected override void Awake()
    {
        base.Awake();

        buttons["OkButton"].onClick.AddListener(() => { GoToMainScene(); });
    }

    public void GoToMainScene()
    {
        GameManager.UI.ClosePopUpUI();
        GameManager.Scene.LoadScene("MainScene");
    }
}
