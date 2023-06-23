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
        // LivingEntity�� OnEnable() ���� (���� �ʱ�ȭ)
        base.OnEnable();
        UpdateUI();
    }

    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity�� RestoreHealth() ���� (ü�� ����)
        base.RestoreHealth(newHealth);
        // ü�� ����
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

        // LivingEntity�� OnDamage() ����(������ ����)
        // ���ŵ� ü���� ü�� �����̴��� �ݿ�
        UpdateUI();
        return true;
    }

    public override void Die()
    {
        // LivingEntity�� Die() ����(��� ����)
        base.Die();

        // ü�� �����̴� ��Ȱ��ȭ
        UpdateUI();
        // ����� ���
        // playerAudioPlayer.PlayOneShot(deathClip);
        // �ִϸ������� Die Ʈ���Ÿ� �ߵ����� ��� �ִϸ��̼� ���
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
