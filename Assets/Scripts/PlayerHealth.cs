using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

public class PlayerHealth : LivingEntity
{
    [SerializeField] public GameSceneFlow gameSceneFlow;
    [SerializeField] public MapTwoSceneFlow mapTwoSceneFlow;
    [SerializeField] Rig rig;
    private Animator anim;
    private PlayerMover mover;
    public UnityEvent OnDamaged;
    public UnityEvent OnHealed;
    public bool deadCheckForShooter;
    private void Awake()
    {
        deadCheckForShooter = false; // 살아있음

        anim = GetComponent<Animator>();
        mover = GetComponent<PlayerMover>();
        // shooter = GetComponent<PlayerShooter>();
        // gun = this.gameObject.GetComponentInChildren<Gun>();
    }

    protected override void OnEnable()
    {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();
        GameManager.data.UpdateHp(health);
        GameManager.data.UpdateLife(GameManager.data.Life);
    }

    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);
        OnHealed.Invoke();
    }

    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        // GameManager.UI.ShowPopUpUI<PopUpUI>("UI/PlayerDamagedEffect");

        GameManager.data.UpdateHp(health);
        OnDamaged.Invoke();


        Vector3 hitBackDir = (this.transform.position - damageMessage.damager.transform.position).normalized;
        hitBackDir = Vector3.Lerp(hitBackDir, this.transform.position, Time.deltaTime);
        mover.MoveBack(hitBackDir);
        return true;
    }

    public override void Die()
    {
        deadCheckForShooter = true;
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();
        mover.enabled = false;
        // shooter.enabled = false;
        // gun.enabled = false;
        GameManager.data.UpdateScore(-30);
        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        // anim.applyRootMotion = true;
        anim.SetTrigger("Die");
        anim.SetLayerWeight(1, 0);
        rig.weight = 0;

        yield return new WaitForSeconds(4);
        gameObject.SetActive(false);
        if (GameManager.Instance.curStageNum == 1)
        {
            gameSceneFlow.PlayerDead();
        }
        else
        {
            mapTwoSceneFlow.PlayerDead();
        }
        // GameManager.data.UpdateLife(GameManager.data.Life - 1);
        // if (GameManager.data.Life < 0)
        // {
        //     GameManager.data.EndGame();
        // }
        // else
        // {
        //     yield return new WaitForSeconds(4);
        //     //Destroy(gameObject);
        //     gameObject.SetActive(false);
        //     Rebirth();
        // }

    }

    // public void Rebirth()
    // {
    //     // GameObject player = Instantiate(playerPrefab, playerSpawnPosition.position, playerSpawnPosition.rotation);
    //     // virtualCamera.Follow = cameraRoot.transform;
    // 
    //     this.transform.position = playerSpawnPosition.position;
    //     this.transform.rotation = playerSpawnPosition.rotation;
    //     this.health = 100;
    //     gameObject.SetActive(true);
    // }
}
