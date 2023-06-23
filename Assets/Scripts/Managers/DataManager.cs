using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    private int life; // ���� Life 
    private int score; // ���� ���� ����
    private float hp;
    private int bullet;

    private void Awake()
    {

    }

    public bool isGameover { get; private set; } // ���� ���� ����

    public int Life { get { return life; } set { life = value; } }
    public int Bullet { get { return bullet; } set { bullet = value; } }
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            OnScoreChanged?.Invoke(score);
        }
    }
    public event UnityAction<float> OnScoreChanged;

    public void UpdateScore(int newScore)
    {
        // ���� ������ �ƴ� ���¿����� ���� ���� ����
        if (!isGameover)
        {
            // ���� �߰�
            score += newScore;
            // ���� UI �ؽ�Ʈ ����
            GameManager.UI.UpdateScoreText(score);
        }
    }

    public void UpdateLife()
    {
        if (!isGameover)
        {
            life--;
            GameManager.UI.UpdateLifeText(life);
        }
    }

    // ���� ���� ó��
    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        isGameover = true;
        // ���� ���� UI�� Ȱ��ȭ
        GameManager.UI.SetActiveGameoverUI(true);
    }

    
}
