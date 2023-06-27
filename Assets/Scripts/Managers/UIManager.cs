using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private EventSystem eventSystem;

    private Canvas popUpCanvas;
    private Stack<PopUpUI> popUpStack;

    private Canvas windowCanvas;

    // private GameObject gameoverUI;
    // private GameObject crosshair;
    

    private void Awake()
    {
        eventSystem = GameManager.Resource.Instantiate<EventSystem>("UI/EventSystem");
        eventSystem.transform.parent = transform;

        popUpCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        popUpCanvas.gameObject.name = "PopUpCanvas";
        popUpCanvas.sortingOrder = 100;
        popUpStack = new Stack<PopUpUI>();

        windowCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        windowCanvas.gameObject.name = "WindowCanvas";
        windowCanvas.sortingOrder = 10;

    }

    public T ShowPopUpUI<T>(T popUpui) where T : PopUpUI
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        if (popUpStack.Count > 0)
        {
            PopUpUI prevUI = popUpStack.Peek();
            prevUI.gameObject.SetActive(false);
        }

        T ui = GameManager.Pool.GetUI(popUpui);
        ui.transform.SetParent(popUpCanvas.transform, false);

        popUpStack.Push(ui);

        Time.timeScale = 0;

        return ui;
    }

    public T ShowPopUpUI<T>(string path) where T : PopUpUI
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        T ui = GameManager.Resource.Load<T>(path);
        return ShowPopUpUI(ui);
    }

    public void ClosePopUpUI()
    {
        PopUpUI ui = popUpStack.Pop();
        GameManager.Pool.Release(ui.gameObject);

        if (popUpStack.Count > 0)
        {
            PopUpUI curUI = popUpStack.Peek();
            curUI.gameObject.SetActive(true);
        }
        if (popUpStack.Count == 0)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

    public void ShowWindowUI(WindowUI windowUI)
    {
        WindowUI ui = GameManager.Pool.GetUI(windowUI);
        ui.transform.SetParent(windowCanvas.transform, false);
    }

    public void ShowWindowUI(string path)
    {
        WindowUI ui = GameManager.Resource.Load<WindowUI>(path);
        ShowWindowUI(ui);
    }

    public void SelectWindowUI(WindowUI windowUI)
    {
        windowUI.transform.SetAsLastSibling();
    }

    public void CloseWindowUI(WindowUI windowUI)
    {
        GameManager.Pool.Release(windowUI.gameObject);
    }


    // public void UpdateScoreText(int newScore)  // 'scoreText' 점수 UI 갱신
    // {
    //     ScoreCount.text = "" + newScore;
    // }
    // 
    // public void UpdateLifeText(int count)  // 'lifeText' 남은 생명 수 UI 갱신
    // {
    //     LifeCount.text = "" + count;
    // }
    // 
    // public void UpdateHealthText(float health) // 'healthText' 남은 HP UI 갱신
    // {
    //     RemainHP.text = Mathf.Floor(health).ToString(); // 체력의 소숫점을 내림한 후 문자열로 바꿈
    // }
    // 
    // public void SetActiveGameoverUI(bool active) // 게임 오버시 'GameOver' UI 활성화
    // {
    //     gameoverUI.SetActive(active);
    // }
    // 
    // public void SetActiveCrossHairUI(bool active) // 크로스 헤어 UI 활성화
    // {
    //     crosshair.SetActive(active);
    // }

    // public void GameRestart()  // 게임 Over 상태에서 Restart 버튼을 눌렀을 때 실행시킬 함수. 게임 재 시작
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // 현재 씬의 이름을 넘겨 다시 로드
    // }


}
