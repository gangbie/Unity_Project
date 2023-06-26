using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerHealth : LivingEntity
{
    [SerializeField] Rig rig;
    private Animator anim;
    public int life;
    public int score;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    protected override void OnEnable()
    {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();
        GameManager.data.UpdateHp(health);
        // UpdateUI();
    }

    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);
        // 체력 갱신
        // UpdateUI();
    }

    // private void UpdateUI()
    // {
    //     GameManager.UI.UpdateHealthText(dead ? 0f : health);
    // }

    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        // EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        // playerAudioPlayer.PlayOneShot(hitClip);

        // LivingEntity의 OnDamage() 실행(데미지 적용)
        // 갱신된 체력을 체력 슬라이더에 반영
        // UpdateUI();
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
        anim.applyRootMotion = true;
        anim.SetTrigger("Die");
        anim.SetLayerWeight(1, 0);
        rig.weight = 0;


        GameManager.data.UpdateLife();
        if (GameManager.data.Life < 0)
        {
            GameManager.data.EndGame();
        }
        else
        {
            Rebirth();
        }
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

    public void Rebirth()
    {
        // Instantiate(playerPrefab, playerSpawnPosition.position, playerSpawnPosition.rotation);
    }
}
