using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro.EditorUtilities;
using UnityEditor.EditorTools;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static PoolManager poolManager;
    private static ResourceManager resource;
    public static GameManager Instance { get { return instance; } }
    public static PoolManager Pool { get { return poolManager; } }
    public static ResourceManager Resource { get { return resource; } }

    private int score;
    public bool isGameover { get; private set; }

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

    public void AddScore(int newScore)
    {
        if (!isGameover)
        {
            score += newScore;
            // UIManager.Instance.UpdateScoreText(score);
        }
    }

    public void EndGame()
    {
        isGameover = true;
        // UIManager.Instance.SetActiveGameoverUI(true);
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitManagers()
    {
        GameObject poolObj = new GameObject();
        poolObj.name = "PoolManager";
        poolObj.transform.parent = transform;
        poolManager = poolObj.AddComponent<PoolManager>();

        GameObject resourceObj = new GameObject();
        resourceObj.name = "ResourceManager";
        resourceObj.transform.parent = transform;
        resource = resourceObj.AddComponent<ResourceManager>();
    }
}
