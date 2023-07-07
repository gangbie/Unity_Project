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
        deadCheckForShooter = false; // �������

        anim = GetComponent<Animator>();
        mover = GetComponent<PlayerMover>();
    }

    protected override void OnEnable()
    {
        // LivingEntity�� OnEnable() ���� (���� �ʱ�ȭ)
        base.OnEnable();
        GameManager.data.UpdateHp(health);
        GameManager.data.UpdateLife(GameManager.data.Life);
    }

    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity�� RestoreHealth() ���� (ü�� ����)
        base.RestoreHealth(newHealth);
        OnHealed.Invoke();
    }

    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

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
        // LivingEntity�� Die() ����(��� ����)
        base.Die();
        mover.enabled = false;
        GameManager.data.UpdateScore(-30);
        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
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
    }
}
