using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainScene : BaseScene
{
    public UnityEvent OnMainSceneEntered;
    public UnityEvent OnMainSceneExited;
    private void OnEnable()
    {
        OnMainSceneEntered?.Invoke();
    }

    private void OnDisable()
    {
        OnMainSceneExited?.Invoke();
    }
    public void StartButton()
    {
        Debug.Log("Start button clicked");
        GameManager.Scene.LoadScene("Map1");
    }

    protected override IEnumerator LoadingRoutine()
    {
        GameManager.UI.Init();
        GameManager.Pool.Init();
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        progress = 0.6f;
        yield return new WaitForSecondsRealtime(1f);
        progress = 0.8f;
        yield return new WaitForSecondsRealtime(1f);

        yield return new WaitForSecondsRealtime(1f);
        progress = 1.0f;
    }
}
