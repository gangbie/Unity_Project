using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerHealth : LivingEntity
{
    [SerializeField] GameSceneFlow gameSceneFlow;
    [SerializeField] Rig rig;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
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
    }

    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        // EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        // playerAudioPlayer.PlayOneShot(hitClip);

        // LivingEntity의 OnDamage() 실행(데미지 적용)
        GameManager.data.UpdateHp(health);
        return true;
    }

    public override void Die()
    {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();

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
        gameSceneFlow.PlayerDead();

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
