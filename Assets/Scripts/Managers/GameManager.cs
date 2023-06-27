using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro.EditorUtilities;
using UnityEditor.EditorTools;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // public GameObject player;
    // public Transform playerSpawnPosition;
    // public CinemachineVirtualCamera virtualCamera;

    // public bool isPlayerDead { get; private set; } // dead 상태
    // public bool isGameover { get; private set; } // 게임 오버 상태

    private static GameManager instance;
    private static PoolManager poolManager;
    private static ResourceManager resource;
    private static UIManager ui;
    private static SceneManager sceneManager;
    private static DataManager dataManager;

    public static GameManager Instance { get { return instance; } }
    public static PoolManager Pool { get { return poolManager; } }
    public static ResourceManager Resource { get { return resource; } }
    public static UIManager UI { get { return ui; } }
    public static SceneManager Scene { get { return sceneManager; } }
    public static DataManager data { get { return dataManager; } }

    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        InitManagers();

    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitManagers()
    {
        GameObject resourceObj = new GameObject();
        resourceObj.name = "ResourceManager";
        resourceObj.transform.parent = transform;
        resource = resourceObj.AddComponent<ResourceManager>();

        GameObject poolObj = new GameObject();
        poolObj.name = "PoolManager";
        poolObj.transform.parent = transform;
        poolManager = poolObj.AddComponent<PoolManager>();

        GameObject UIObj = new GameObject();
        UIObj.name = "UIManager";
        UIObj.transform.parent = transform;
        ui = UIObj.AddComponent<UIManager>();

        GameObject sceneObj = new GameObject();
        sceneObj.name = "SceneManager";
        sceneObj.transform.parent = transform;
        sceneManager = sceneObj.AddComponent<SceneManager>();

        GameObject dataObj = new GameObject();
        dataObj.name = "DataManager";
        dataObj.transform.parent = transform;
        dataManager = dataObj.AddComponent<DataManager>();
    }

    // public void Rebirth()
    // {
    //     // GameObject player = Instantiate(playerPrefab, playerSpawnPosition.position, playerSpawnPosition.rotation);
    //     // Transform cameraRoot = player.transform;
    // 
    //     // player.transform.position = playerSpawnPosition.position;
    //     // player.transform.rotation = playerSpawnPosition.rotation;
    //     player.RestoreHealth(100);
    //     gameObject.SetActive(true);
    //     isPlayerDead = false;
    // }
    // 
    // public void PlayerDead()
    // {
    //     isPlayerDead = true;
    // 
    //     GameManager.data.UpdateLife(GameManager.data.Life - 1);
    //     if (GameManager.data.Life < 0)
    //     {
    //         EndGame();
    //     }
    //     else
    //     {
    //         Rebirth();
    //     }
    // }
    // 
    // public void EndGame()
    // {
    //     // 게임 오버 상태를 참으로 변경
    //     isGameover = true;
    //     // 게임 오버 UI를 활성화
    //     GameManager.UI.ShowPopUpUI<PopUpUI>("UI/GameOverUI");
    // }
}
