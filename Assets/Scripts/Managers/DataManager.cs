using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    public PlayerHealth player;
    private Gun gun;

    private float hp;
    private int life;
    private int bullet;
    private int score;

    private void Awake()
    {
        life = 3;
    }

    public bool isGameover { get; private set; } // 게임 오버 상태

    public float Hp { get {  return hp; } private set { hp = value; OnChangeHp?.Invoke(); } }
    public UnityAction OnChangeHp;
    public int Life { get { return life; } private set { life = value; OnChangeLife?.Invoke(); } }
    public UnityAction OnChangeLife;
    public int Bullet { get { return bullet; } private set { bullet = value; OnChangeBullet?.Invoke(); } }
    public UnityAction OnChangeBullet;
    public int Score { get { return score; } private set { score = value; OnChangeScore?.Invoke(); } }
    public UnityAction OnChangeScore;

    public void UpdateScore(int newScore)
    {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!isGameover)
        {
            // 점수 추가
            Score += newScore;
        }
    }

    public void UpdateHp(float hp)
    {
        if (!isGameover)
            Hp = hp;
    }

    public void UpdateLife()
    {
        if (!isGameover)
            life--;
    }

    public void UpdateBullet(int bullet)
    {
        if (!isGameover)
            Bullet = bullet;
    }

    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
        // GameManager.UI.SetActiveGameoverUI(true);
    }

    
}
