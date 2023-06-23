using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy : LivingEntity
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
        Idle, Patrol, Trace, Attacking, Die
    }

    StateMachine<State, Enemy> stateMachine;

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

        stateMachine = new StateMachine<State, Enemy>(this);
        stateMachine.AddState(State.Idle,      new IdleState(this, stateMachine));
        stateMachine.AddState(State.Patrol,    new PatrolState(this, stateMachine));
        stateMachine.AddState(State.Trace,     new TraceState(this, stateMachine));
        stateMachine.AddState(State.Attacking, new AttackingState(this, stateMachine));
        stateMachine.AddState(State.Die,       new DieState(this, stateMachine));

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
        targetEntity = null;
        stateMachine.SetUp(State.Idle);
        StartCoroutine(AIRoutine());
    }

    private void Update()
    {
        stateMachine.Update();

        anim.SetFloat("Speed", agent.desiredVelocity.magnitude);
    }

    private void FixedUpdate()
    {
        if (dead) return;

        if (state == State.Attacking)
        {
            // targetEntity 바라보기
            if (targetEntity != null)
            {
                var lookRotation = Quaternion.LookRotation(targetEntity.transform.position - transform.position);
                var targetAngleY = lookRotation.eulerAngles.y;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY,
                ref turnSmoothVelocity, turnSmoothTime);
            }

            // 공격할 타겟 감지 및 공격 처리
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

    private IEnumerator AIRoutine()
    {
        while (!dead)
        {
            var colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, targetMask);
            foreach (var collider in colliders)
            {
                // if (!IsTargetOnSight(collider.transform)) continue;

                var livingEntity = collider.GetComponent<LivingEntity>();

                // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                if (livingEntity != null && !livingEntity.dead)
                {
                    // 추적 대상을 해당 LivingEntity로 설정
                    targetEntity = livingEntity;
                    Debug.Log("AI코루틴 돌던중 타겟 찾음");
                    Debug.Log(hasTarget);
                    // for문 루프 즉시 정지
                    break;
                }
            }
            if (colliders.Length == 0)
            {
                targetEntity = null;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // 데미지를 입었을때 실행할 처리
    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        if (!base.ApplyDamage(damageMessage)) return false;

        if (targetEntity == null)
        {
            targetEntity = damageMessage.damager.GetComponent<LivingEntity>();
        }

        // EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform, EffectManager.EffectType.Flesh);
        // audioPlayer.PlayOneShot(hitClip);

        return true;
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
            stateMachine.ChangeState(State.Attacking);
        }
        else
        {
            stateMachine.ChangeState(State.Patrol);
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

    // 사망 처리
    public override void Die()
    {
        base.Die();
        GetComponent<Collider>().enabled = false;
        agent.enabled = false;
        StartCoroutine(DieRoutine());
    }
    private IEnumerator DieRoutine()
    {
        anim.applyRootMotion = true;
        anim.SetTrigger("Die");
        GameManager.data.UpdateScore(100);

        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

    private abstract class EnemyState : StateBase<State, Enemy>
    {
        protected GameObject gameObject => owner.gameObject;
        protected Transform transform => owner.transform;
        protected NavMeshAgent agent => owner.agent;
        protected Animator anim => owner.anim;

        protected EnemyState(Enemy owner, StateMachine<State, Enemy> stateMachine) : base(owner, stateMachine)
        {
        }
    }

    private class IdleState : EnemyState
    {
        private NavMeshAgent agent;
        private bool hasTarget;
        public IdleState(Enemy owner, StateMachine<State, Enemy> stateMachine) : base(owner, stateMachine)
        {
        }
        public override void Setup()
        {
            agent = owner.agent;
        }
        public override void Enter()
        {
            Debug.Log("IdleState Enter");
            Debug.Log(owner.hasTarget);
            agent.isStopped = true;
            agent.speed = 0;
            idleRoutine = owner.StartCoroutine(IdleRoutine());
        }
        public override void Update()
        {
        }

        public override void Transition()
        {
            if (owner.hasTarget)
            {
                stateMachine.ChangeState(State.Trace);
            }
        }
        public override void Exit()
        {
            owner.StopCoroutine(idleRoutine);
            Debug.Log("IdleState Exit");
        }

        Coroutine idleRoutine;
        private IEnumerator IdleRoutine()
        {
            yield return new WaitForSeconds(3);
            agent.isStopped = false;
            stateMachine.ChangeState(State.Patrol);
        }
    }

    private class PatrolState : EnemyState
    {
        private NavMeshAgent agent;
        private int routineNum;
        public PatrolState(Enemy owner, StateMachine<State, Enemy> stateMachine) : base(owner, stateMachine)
        {
        }
        public override void Setup()
        {
            agent = owner.agent;
        }
        public override void Enter()
        {
            routineNum = 0;
            agent.stoppingDistance = 0;
            Debug.Log("PatrolState Enter");
            Debug.Log(owner.hasTarget);
            agent.speed = owner.patrolSpeed;
            agent.SetDestination(transform.position);
            patrolRoutine = owner.StartCoroutine(PatrolRoutine());
        }
        public override void Update()
        {
            
        }

        public override void Transition()
        {
            if (owner.hasTarget)
            {
                stateMachine.ChangeState(State.Trace);
            }
            else if (routineNum == 5)
            {
                stateMachine.ChangeState(State.Idle);
            }
        }
        public override void Exit()
        {
            owner.StopCoroutine(patrolRoutine);
            Debug.Log("PatrolState Exit");
        }

        Coroutine patrolRoutine;
        private IEnumerator PatrolRoutine()
        {
            while (!owner.hasTarget)
            {
                if (agent.remainingDistance <= 1f)
                {
                    routineNum++;
                    Debug.Log(routineNum);
                    var patrolPosition = Utility.GetRandomPointOnNavMesh(transform.position, 7f, NavMesh.AllAreas);
                    agent.SetDestination(patrolPosition);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private class TraceState : EnemyState
    {
        private NavMeshAgent agent;
        private float attackDistance;
        public TraceState(Enemy owner, StateMachine<State, Enemy> stateMachine) : base(owner, stateMachine)
        {
        }
        public override void Setup()
        {
            agent = owner.agent;
            attackDistance = owner.attackDistance;
        }
        public override void Enter()
        {
            Debug.Log("TraceState Enter");
            Debug.Log(owner.hasTarget);
            agent.speed = owner.runSpeed;
            traceRoutine = owner.StartCoroutine(TraceRoutine());
        }
        public override void Update()
        {
            
        }

        public override void Transition()
        {
            if (!owner.hasTarget)
            {
                stateMachine.ChangeState(State.Patrol);
            }
            else if (Vector3.Distance(owner.targetEntity.transform.position, owner.transform.position) <= owner.attackDistance)
            {
                stateMachine.ChangeState(State.Attacking);
            }
        }
        public override void Exit()
        {
            Debug.Log("TraceState Exit");
            owner.StopCoroutine(traceRoutine);
        }

        Coroutine traceRoutine;
        private IEnumerator TraceRoutine()
        {
            while (owner.hasTarget)
            {
                // Debug.Log("Trace 코루틴 돌아감");
                agent.SetDestination(owner.targetEntity.transform.position);
                agent.stoppingDistance = attackDistance;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private class AttackingState : EnemyState
    {
        private NavMeshAgent agent;
        private Animator anim;
        private float attackDistance;
        public AttackingState(Enemy owner, StateMachine<State, Enemy> stateMachine) : base(owner, stateMachine)
        {
        }
        public override void Setup()
        {
            agent = owner.agent;
            anim = owner.anim;
            attackDistance = owner.attackDistance;
        }
        public override void Enter()
        {
            Debug.Log("AttakingState Enter");
            Debug.Log(owner.hasTarget);
            agent.isStopped = true;
            anim.SetTrigger("Attack");
        }
        public override void Update()
        {
        }

        public override void Transition()
        {
            if (Vector3.Distance(owner.targetEntity.transform.position, transform.position) > attackDistance + 1f)
            {
                agent.isStopped = false;
                stateMachine.ChangeState(State.Trace);
            }
        }
        public override void Exit()
        {
            Debug.Log("AttackingState Exit");
        }
    }

    // 체력 0 되면 어차피 Die()의 base.Die() 실행되어 죽기 때문에 DieState는 필요 없을듯?
    private class DieState : EnemyState
    {
        // private NavMeshAgent agent;
        public DieState(Enemy owner, StateMachine<State, Enemy> stateMachine) : base(owner, stateMachine)
        {
        }
        public override void Setup()
        {
        }
        public override void Enter()
        {
        }
        public override void Update()
        {
        }

        public override void Transition()
        {
        }
        public override void Exit()
        {
        }
    }
}