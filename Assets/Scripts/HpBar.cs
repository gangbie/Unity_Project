using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] Enemy enemy;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        slider.maxValue = enemy.health;
        slider.value = enemy.health;
        enemy.OnChangeHP.AddListener((hp) => { slider.value = hp; });
    }
}
