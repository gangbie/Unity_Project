using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;

public class ZombieMover : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] GameObject player;
    [SerializeField] GameObject environmentChecker;

    private float moveTime;
    private Rigidbody rb;
    private Animator anim;
    private Vector3 pos;
    
    private Collider checkCollider;

    private bool foundPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        //checkCollider = environmentChecker.AddComponent<Collider>();
    }

    private void Start()
    {
        Move();
    }

    void Update()
    {
        
    }

    private void Move()
    {
        StartCoroutine(ZombieRoutine());
    }

    IEnumerator ZombieRoutine()
    {
        pos = rb.transform.position;

        anim.SetBool("Rest", false);
        Debug.Log("Rest, false");
        
        while (!foundPlayer)
        {
            Debug.Log("while enter");
            moveTime += Time.deltaTime;

            float[] moveRange = RandomDir();
            
            var dir = (pos - this.transform.position).normalized;
            this.transform.LookAt(pos);
            // this.transform.position += dir * walkSpeed * Time.deltaTime;
            rb.velocity = dir * walkSpeed;
            
            float distance = Vector3.Distance(transform.position, pos);
            float distanceWithPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceWithPlayer <= 15f)
            {
                foundPlayer = true;
                StartCoroutine(FollowRoutine());
                yield break;
            }

            if (distance <= 0.1f)
            {
                Debug.Log("distance <= 0.1");
                rb.velocity = Vector3.zero;
                anim.SetBool("Rest", true);
                yield return new WaitForSeconds(2);
                anim.SetBool("Rest", false);
                pos.x = rb.transform.position.x + moveRange[0];
                pos.z = rb.transform.position.z + moveRange[1];
                moveTime = 0;
            }
            else if(moveTime > 5)
            {
                Debug.Log("moveTime > 5");
                rb.velocity = Vector3.zero;
                anim.SetBool("Rest", true);
                yield return new WaitForSeconds(Random.Range(1f, 3f));
                anim.SetBool("Rest", false);
                pos.x = rb.transform.position.x + moveRange[0];
                pos.z = rb.transform.position.z + moveRange[1];
                moveTime = 0;
            }

            yield return null;
        }
    }

    IEnumerator FollowRoutine()
    {
        Debug.Log("Follow Routine!!");
        anim.SetBool("FoundPlayer", true);


        while (true)
        {
            Debug.Log("플레이어에게 다가오는중");
            pos = player.transform.position;
            var dir = (pos - this.transform.position).normalized;
            this.transform.LookAt(pos);
            rb.velocity = dir * runSpeed;

            float distance = Vector3.Distance(transform.position, pos);
            if (distance <= 0.5f)
            {
                Debug.Log("Attack!!");
                rb.velocity = Vector3.zero;
                yield break;
            }
            
            yield return null;
        }
    }

    private float[] RandomDir()
    {
        int randomNum = Random.Range(0, 4);
        float[] moveRange = new float[2];
        float moveRangeX;
        float moveRangeY;
        if (randomNum == 0)
        {
            moveRangeX = 3f;
            moveRangeY = 3f;
        }
        else if (randomNum == 1)
        {
            moveRangeX = -3f;
            moveRangeY = 3f;
        }
        else if (randomNum == 2)
        {
            moveRangeX = 3f;
            moveRangeY = -3f;
        }
        else
        {
            moveRangeX = -3f;
            moveRangeY = -3f;
        }
        moveRange[0] = moveRangeX;
        moveRange[1] = moveRangeY;

        return moveRange;
    }
    
}
