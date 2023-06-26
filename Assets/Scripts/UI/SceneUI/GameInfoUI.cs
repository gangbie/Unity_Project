using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameInfoUI : SceneUI
{
    [SerializeField] private TMP_Text RemainHP;
    [SerializeField] private TMP_Text LifeCount;
    [SerializeField] private TMP_Text ScoreCount;
    [SerializeField] private TMP_Text RemainBullet;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        GameManager.data.OnChangeHp += () => { texts["RemainHP"].text = GameManager.data.Hp.ToString(); };
        GameManager.data.OnChangeScore += () => { ScoreCount.text = GameManager.data.Score.ToString(); };
        GameManager.data.OnChangeBullet += () => { RemainBullet.text = GameManager.data.Bullet.ToString(); };
        GameManager.data.OnChangeLife += () => { LifeCount.text = GameManager.data.Life.ToString(); };

        texts["RemainHP"].text = GameManager.data.Hp.ToString();
        LifeCount.text = GameManager.data.Life.ToString();
        ScoreCount.text = GameManager.data.Score.ToString();
        RemainBullet.text = GameManager.data.Bullet.ToString();
    }

    private void Update()
    {
        
    }
}
