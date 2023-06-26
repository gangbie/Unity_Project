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

    public bool isGameover { get; private set; } // ���� ���� ����

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

    // ���� ���� ó��
    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        isGameover = true;
        // ���� ���� UI�� Ȱ��ȭ
        // GameManager.UI.SetActiveGameoverUI(true);
    }

    
}
