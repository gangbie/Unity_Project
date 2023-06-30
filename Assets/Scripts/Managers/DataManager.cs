using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    private EnemySpawner enemySpawner;
    private GameSceneFlow gameSceneFlow;
    public PlayerHealth player;
    private Gun gun;

    private float hp;
    private int life;
    private int bullet;
    private int score;
    private int enemy;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        isGameover = false;
        Life = 3;
        UpdateScore(0);
        UpdateHp(100);
        UpdateBullet(30);
    }

    public bool isGameover { get; private set; } // ���� ���� ����

    public float Hp { get {  return hp; } private set { hp = value; OnChangeHp?.Invoke(); } }
    public UnityAction OnChangeHp;
    public int Life { get { return life; } private set { life = value; OnChangeLife?.Invoke(); } }
    public UnityAction OnChangeLife;
    public int Bullet { get { return bullet; } private set { bullet = value; OnChangeBullet?.Invoke(); } }
    public UnityAction OnChangeBullet;
    public int Score { get { return score; } private set { score = value; OnChangeScore?.Invoke(); } }
    public UnityAction OnChangeScore;
    public int Enemy { get { return enemy; } private set { enemy = value; OnChangeEnemy?.Invoke(); } }
    public UnityAction OnChangeEnemy;

    public void UpdateScore(int newScore)
    {
        // ���� ������ �ƴ� ���¿����� ���� ���� ����
        if (!isGameover)
        {
            // ���� �߰�
            Score += newScore;
        }
    }

    public void UpdateHp(float hp)
    {
        if (!isGameover)
            Hp = (int)hp;
    }

    public void UpdateLife(int life)
    {
        if (!isGameover)
            Life = life;
    }

    public void UpdateBullet(int bullet)
    {
        if (!isGameover)
            Bullet = bullet;
    }

    public void UpdateEnemy(int enemy)
    {
        if (!isGameover)
            Enemy = enemy;
    }

    // ���� ���� ó��
    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        isGameover = true;
        // ���� ���� UI�� Ȱ��ȭ
        GameManager.UI.ShowPopUpUI<PopUpUI>("UI/GameOverUI");
    }

    
}
