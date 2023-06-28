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

        // slider.onValueChanged.AddListener(OnSliderValueChanged);
        buttons["SaveButton"].onClick.AddListener(() => { GameManager.UI.ClosePopUpUI(); });
        buttons["CancelButton"].onClick.AddListener(() => { GameManager.UI.ClosePopUpUI(); });
    }

    private void OnSliderValueChanged(float value)
    {
        Debug.Log("슬라이더 값 : " + value);
    }
}
