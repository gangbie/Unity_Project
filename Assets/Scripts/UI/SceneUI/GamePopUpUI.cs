using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GamePopUpUI : PopUpUI
{
    public UnityEvent OnPopUpOpened;
    protected override void Awake()
    {
        base.Awake();
    }

    public void OpenPausePopUpUI()
    {
        GameManager.UI.ShowPopUpUI<PopUpUI>("UI/PausePopUpUI");
        OnPopUpOpened?.Invoke();
    }
    private void OnPause(InputValue value)
    {
        if (GameManager.UI.popUpStack.Count == 0)
            OpenPausePopUpUI();
    }

    public void OpenItemPopUpUI()
    {
        GameManager.UI.ShowPopUpUI<PopUpUI>("UI/ItemListPopUpUI");
        OnPopUpOpened?.Invoke();
    }

    private void OnItem(InputValue value)
    {
        if (GameManager.UI.popUpStack.Count == 0)
            OpenItemPopUpUI();
    }

}
