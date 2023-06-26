using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseScene
{
    public void StartButton()
    {
        GameManager.Scene.LoadScene("Map1");
    }

    protected override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
