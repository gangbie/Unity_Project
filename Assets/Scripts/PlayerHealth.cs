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
        // LivingEntity�� OnEnable() ���� (���� �ʱ�ȭ)
        base.OnEnable();
        GameManager.data.UpdateHp(health);
        // UpdateUI();
    }

    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity�� RestoreHealth() ���� (ü�� ����)
        base.RestoreHealth(newHealth);
        // ü�� ����
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

        // LivingEntity�� OnDamage() ����(������ ����)
        // ���ŵ� ü���� ü�� �����̴��� �ݿ�
        // UpdateUI();
        GameManager.data.UpdateHp(health);
        return true;
    }

    public override void Die()
    {
        // LivingEntity�� Die() ����(��� ����)
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
