using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    private int life; // 현재 Life 
    private int score; // 현재 게임 점수
    private float hp;
    private int bullet;

    private void Awake()
    {

    }

    public bool isGameover { get; private set; } // 게임 오버 상태

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
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!isGameover)
        {
            // 점수 추가
            score += newScore;
            // 점수 UI 텍스트 갱신
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

    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
        GameManager.UI.SetActiveGameoverUI(true);
    }

    
}
