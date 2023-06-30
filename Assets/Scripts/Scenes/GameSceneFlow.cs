using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GameSceneFlow : MonoBehaviour
{
    [SerializeField] PlayerHealth player;
    [SerializeField] Transform playerSpawnPosition;
    [SerializeField] Rig rig;

    private PlayerMover mover;
    public bool isPlayerDead { get; private set; } // dead 상태

    private void Awake()
    {
        mover = player.GetComponentInChildren<PlayerMover>();
    }

    public void Rebirth()
    {
        rig.weight = 1;
        mover.enabled = true;
        player.transform.position = playerSpawnPosition.position;
        player.transform.rotation = playerSpawnPosition.rotation;
        player.RestoreHealth(100);
        GameManager.data.UpdateBullet(30);
        player.gameObject.SetActive(true);
        isPlayerDead = false;
        player.deadCheckForShooter = false;
    }

    public void PlayerDead()
    {
        isPlayerDead = true;

        GameManager.data.UpdateLife(GameManager.data.Life - 1);
        if (GameManager.data.Life < 0)
        {
            GameManager.data.UpdateLife(0);
            GameManager.data.EndGame();
        }
        else
        {
            Rebirth();
        }
    }

    // public void EndGame()
    // {
    //     // 게임 오버 상태를 참으로 변경
    //     GameManager.data.isGameover = true;
    //     // 게임 오버 UI를 활성화
    //     GameManager.UI.ShowPopUpUI<PopUpUI>("UI/GameOverUI");
    // }
}
