using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map2Scene : BaseScene
{
    protected override IEnumerator LoadingRoutine()
    {
        GameManager.UI.Init();
        GameManager.Pool.Init();
        GameManager.data.Init();


        progress = 0.0f;
        yield return new WaitForSecondsRealtime(1f);
        progress = 0.2f;
        yield return new WaitForSecondsRealtime(1f);
        progress = 0.4f;
        yield return new WaitForSecondsRealtime(1f);
        progress = 0.6f;
        yield return new WaitForSecondsRealtime(1f);
        progress = 0.8f;
        yield return new WaitForSecondsRealtime(1f);

        yield return new WaitForSecondsRealtime(1f);
        progress = 1.0f;

    }
}