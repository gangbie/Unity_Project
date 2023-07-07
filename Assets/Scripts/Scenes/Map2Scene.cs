using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Map2Scene : BaseScene
{
    public GameObject playerPrefab;
    public PlayerHealth playerHealth;
    public PlayerShooter playerShooter;
    public WeaponHolder weaponHolder;
    public TPSCameraController cameraController;
    public CinemachineVirtualCamera virtualCamera;
    public MapTwoSceneFlow mapTwoSceneFlow;
    public Transform cameraRoot;
    public Rig aimRig;
    public Transform playerPosition;
    [SerializeField] FamasGun famasGunPrefab;

    private void Awake()
    {
        GameManager.Instance.curStageNum = 2;

        GameObject player = Instantiate(playerPrefab, playerPosition.position, playerPosition.rotation);
        
        cameraRoot = player.transform.Find("CameraRoot");
        virtualCamera.Follow = cameraRoot;

        playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.mapTwoSceneFlow = mapTwoSceneFlow;

        playerShooter = player.GetComponent<PlayerShooter>();
        weaponHolder = player.GetComponentInChildren<WeaponHolder>();
        aimRig = player.GetComponentInChildren<Rig>();
        mapTwoSceneFlow.player = playerHealth;
        mapTwoSceneFlow.mover = player.GetComponent<PlayerMover>();
        mapTwoSceneFlow.rig = aimRig;

        FamasGun famasGun = Instantiate(famasGunPrefab);
        famasGun.name = "famas gun";
        playerShooter.Get(famasGun);
        famasGun.gameObject.SetActive(false);
        famasGun.GetComponent<BoxCollider>().enabled = false;

        if (GameManager.data.curGunNum == 1)
        {
            playerShooter.SwapGun(famasGun);
        }
    }
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