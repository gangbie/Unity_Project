using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.AI;

public class Target : LivingEntity
{
    [SerializeField] float runSpeed;
    [SerializeField, Range(0.01f, 2f)] float turnSmoothTime;
    private float turnSmoothVelocity;

    [SerializeField] float damage;
    [SerializeField] float attackRange;
    private float attackDistance;

    [SerializeField] float fieldOfView;
    [SerializeField] float viewDistance;
    [SerializeField] float patrolSpeed;

    [SerializeField] float walkRadius;

    [SerializeField] LivingEntity targetEntity;
    public LayerMask targetMask;

    private RaycastHit[] hits = new RaycastHit[10];
    private List<LivingEntity> lastAttackedTargets = new List<LivingEntity>();

    private bool hasTarget => targetEntity != null && !targetEntity.dead;

    private enum State
    {
        Patrol, Tracking, AttackBegin, Attacking
    }

    private State state;

    private NavMeshAgent agent;
    private Animator anim;

    public Transform attackRoot;
    public Transform eyeTransform;

    private void OnDrawGizmosSelected()
    {
        if (attackRoot != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackRoot.position, attackRange);
        }

        var leftRayRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
        var leftRayDirection = leftRayRotation * transform.forward;
        Handles.color = Color.yellow;
        Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, fieldOfView, viewDistance);
    }

    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        attackDistance = Vector3.Distance(transform.position,
            new Vector3(attackRoot.position.x, transform.position.y, attackRoot.position.z)) + attackRange;

        attackDistance += agent.radius;

        agent.stoppingDistance = attackDistance;
        agent.speed = patrolSpeed;
    }

    public void Setup(float health, float damage,
        float runSpeed, float patrolSpeed, Color skinColor)
    {
        // 체력 설정
        this.startingHealth = health;
        this.health = health;

        // 내비메쉬 에이전트의 이동 속도 설정
        this.runSpeed = runSpeed;
        this.patrolSpeed = patrolSpeed;

        this.damage = damage;

        agent.speed = patrolSpeed;
    }

    private void Start()
    {
        state = State.Patrol;
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        if (dead) return;

        if (state == State.Tracking && Vector3.Distance(targetEntity.transform.position, transform.position) <= attackDistance)
        {
            BeginAttack();
        }

        anim.SetFloat("Speed", agent.desiredVelocity.magnitude);
    }

    private void FixedUpdate()
    {
        if (dead) return;

        if (state == State.AttackBegin || state == State.Attacking)
        {
            var lookRotation = Quaternion.LookRotation(targetEntity.transform.position - transform.position);
            var targetAngleY = lookRotation.eulerAngles.y;

            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY,
                ref turnSmoothVelocity, turnSmoothTime);
        }

        if (state == State.Attacking)
        {
            var direction = transform.forward;
            var deltaDistance = agent.velocity.magnitude * Time.deltaTime;

            var size = Physics.SphereCastNonAlloc(attackRoot.position, attackRange, direction, hits, deltaDistance,
                targetMask);

            for (var i = 0; i < size; i++)
            {
                var attackTargetEntity = hits[i].collider.GetComponent<LivingEntity>();

                if (attackTargetEntity != null && !lastAttackedTargets.Contains(attackTargetEntity))
                {

                    var message = new DamageMessage();
                    message.amount = damage;
                    message.damager = gameObject;

                    if (hits[i].distance <= 0.5f)
                    {
                        message.hitPoint = attackRoot.position;
                    }
                    else
                    {
                        message.hitPoint = hits[i].point;
                    }

                    message.hitNormal = hits[i].normal;

                    attackTargetEntity.ApplyDamage(message);

                    lastAttackedTargets.Add(attackTargetEntity);
                    break;
                }
            }
        }
    }

    private IEnumerator UpdatePath()
    {
        while (!dead)
        {
            if (hasTarget)
            {
                if (state == State.Patrol)
                {
                    state = State.Tracking;
                    agent.speed = runSpeed;
                }
                agent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                if (targetEntity != null) targetEntity = null;

                if (state != State.Patrol)
                {
                    state = State.Patrol;
                    agent.speed = patrolSpeed;
                }
                if (agent.remainingDistance <= 1f)
                {
                    var patrolPosition = Utility.GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
                    // UnityEngine.Random.insideUnitSphere * walkRadius;
                    // randomDirection += transform.position;
                    // NavMeshHit hit;
                    // NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
                    // Vector3 finalPosition = hit.position;

                    agent.SetDestination(patrolPosition);
                }
                var colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, targetMask);
                
                foreach (var collider in colliders)
                {
                    if (!IsTargetOnSight(collider.transform)) continue;

                    var livingEntity = collider.GetComponent<LivingEntity>();

                    // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        // 추적 대상을 해당 LivingEntity로 설정
                        targetEntity = livingEntity;

                        // for문 루프 즉시 정지
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }


    // public void Hit(RaycastHit hit, int damage)
    // {
    //     if (rb != null)
    //     {
    //         rb.AddForceAtPosition(-10 * hit.normal, hit.point, ForceMode.Impulse);
    //     }
    // }

    public void BeginAttack()
    {
        state = State.AttackBegin;

        agent.isStopped = true;
        anim.SetTrigger("Attack");
    }

    public void EnableAttack()
    {
        state = State.Attacking;

        lastAttackedTargets.Clear();
    }

    public void DisableAttack()
    {
        if (hasTarget)
        {
            state = State.Tracking;
        }
        else
        {
            state = State.Patrol;
        }

        agent.isStopped = false;
    }

    private bool IsTargetOnSight(Transform target)
    {
        RaycastHit hit;

        var direction = target.position - eyeTransform.position;

        direction.y = eyeTransform.forward.y;

        if (Vector3.Angle(direction, eyeTransform.forward) > fieldOfView * 0.5f)
        {
            return false;
        }

        direction = target.position - eyeTransform.position;

        if (Physics.Raycast(eyeTransform.position, direction, out hit, viewDistance, targetMask))
        {
            if (hit.transform == target) return true;
        }

        return false;
    }
    public override void Die()
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        // 다른 AI들을 방해하지 않도록 자신의 모든 콜라이더들을 비활성화
        GetComponent<Collider>().enabled = false;

        // AI 추적을 중지하고 내비메쉬 컴포넌트를 비활성화
        agent.enabled = false;

        StartCoroutine(DieRoutine());
        // 사망 애니메이션 재생
        // anim.applyRootMotion = true;
        // anim.SetTrigger("Die");

        // 사망 효과음 재생
        // if (deathClip != null) audioPlayer.PlayOneShot(deathClip);
    }

    private IEnumerator DieRoutine()
    {
        anim.applyRootMotion = true;
        anim.SetTrigger("Die");

        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
