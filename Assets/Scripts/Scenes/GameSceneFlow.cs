using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneFlow : MonoBehaviour
{
    // public GameObject player;
    [SerializeField] PlayerHealth player;
    [SerializeField] Transform playerSpawnPosition;

    public bool isPlayerDead { get; private set; } // dead ����
    // public bool isGameover = GameManager.data.isGameover;

    private void Awake()
    {
        // player = GameObject.FindWithTag("Player");
    }

    public void Rebirth()
    {
        player.transform.position = playerSpawnPosition.position;
        player.transform.rotation = playerSpawnPosition.rotation;
        player.RestoreHealth(100);
        player.gameObject.SetActive(true);
        isPlayerDead = false;
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
    //     // ���� ���� ���¸� ������ ����
    //     GameManager.data.isGameover = true;
    //     // ���� ���� UI�� Ȱ��ȭ
    //     GameManager.UI.ShowPopUpUI<PopUpUI>("UI/GameOverUI");
    // }
}
