using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MapTwoSceneFlow : MonoBehaviour
{
    // public GameObject player;

    [SerializeField] private Map2Scene map2Scene;
    [SerializeField] Transform playerSpawnPosition;
    
    public PlayerHealth player;
    public PlayerMover mover;
    public Rig rig;

    public bool isPlayerDead { get; private set; } // dead 상태

    private void Awake()
    {
        // player = map2Scene.playerPrefab;
        // player = map2Scene.playerHealth;
        // mover = player.gameObject.GetComponent<PlayerMover>();
        // rig = player.gameObject.GetComponent<Rig>();
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
