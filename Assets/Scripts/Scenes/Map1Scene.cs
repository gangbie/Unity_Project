using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1Scene : BaseScene
{
    public GameObject playerPrefab;
    public Transform playerPosition;
    public GameObject zombiePrefab;
    public Transform zombiePosition;

    protected override IEnumerator LoadingRoutine()
    {
        progress = 0.0f;
        yield return new WaitForSecondsRealtime(1f);
        progress = 0.2f;
        yield return new WaitForSecondsRealtime(1f);
        progress = 0.4f;
        // GameObject player = Instantiate(playerPrefab, playerPosition.position, playerPosition.rotation);
        // GameObject zombie = Instantiate(zombiePrefab, zombiePosition.position, zombiePosition.rotation);
        yield return new WaitForSecondsRealtime(1f);
        progress = 0.6f;
        yield return new WaitForSecondsRealtime(1f);
        progress = 0.8f;
        // freeLookCamera.Follow = player.transform;
        // freeLookCamera.LookAt = player.transform;
        yield return new WaitForSecondsRealtime(1f);

        yield return new WaitForSecondsRealtime(1f);
        progress = 1.0f;

    }
}
