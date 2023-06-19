using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Target : LivingEntity
{
    [SerializeField] float runSpeed;
    [SerializeField, Range(0.01f, 2f)] float turnSmoothTime;
    [SerializeField] float turnSmoothVelocity;

    [SerializeField] float damage;
    [SerializeField] float attackRange;
    [SerializeField] float attackDistance;

    [SerializeField] float fieldOfView;
    [SerializeField] float viewDistance;
    [SerializeField] float patrolSpeed;

    [SerializeField] LivingEntity targetEntity;
    public LayerMask targetMask;

    private RaycastHit[] hits = new RaycastHit[10];
    private List<LivingEntity> lastAttackedTargets = new List<LivingEntity>();

    private bool hasTarget => targetEntity != null && !targetEntity.dead;

    private enum State
    {
        Patrol, Tracking, AttackBegin, Attaking
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
            Gizmos.DrawSphere(attackRoot.position, attackRange);
        }

        var leftRayRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
        var leftRayDirection = leftRayRotation * transform.forward;
        Handles.color = Color.yellow;
        Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, fieldOfView, viewDistance);
    }

    private void Awake()
    {
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
        // StartCoroutine(UpdatePath());
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


    }
    // public void Hit(RaycastHit hit, int damage)
    // {
    //     if (rb != null)
    //     {
    //         rb.AddForceAtPosition(-10 * hit.normal, hit.point, ForceMode.Impulse);
    //     }
    // }

    // IEnumerator UpdatePath()
    // {
    // 
    // }

    public void BeginAttack()
    {

    }
}
