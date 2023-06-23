using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        GameManager.data.Life = 3;
    }
    protected override void OnEnable()
    {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();
        UpdateUI();
    }

    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);
        // 체력 갱신
        UpdateUI();
    }

    private void UpdateUI()
    {
        GameManager.UI.UpdateHealthText(dead ? 0f : health);
    }

    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        // EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        // playerAudioPlayer.PlayOneShot(hitClip);

        // LivingEntity의 OnDamage() 실행(데미지 적용)
        // 갱신된 체력을 체력 슬라이더에 반영
        UpdateUI();
        return true;
    }

    public override void Die()
    {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();

        // 체력 슬라이더 비활성화
        UpdateUI();
        // 사망음 재생
        // playerAudioPlayer.PlayOneShot(deathClip);
        // 애니메이터의 Die 트리거를 발동시켜 사망 애니메이션 재생
        anim.SetTrigger("Die");
        GameManager.data.UpdateLife();
        if (GameManager.data.Life < 0)
        {
            GameManager.data.EndGame();
        }
        else
        {
            Rebirth();
        }
    }

    public void Rebirth()
    {

    }
}
