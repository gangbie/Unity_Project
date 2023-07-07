using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float jumpTime;

    private CharacterController controller;
    private Animator anim;
    private Vector3 moveDir;
    private float moveSpeed;
    private float ySpeed = 0;
    public bool isWalking;
    private bool isJumping;

    Coroutine soundRoutine;
    bool isSoundPlay;

    public UnityEvent OnWalked;
    public UnityEvent OnJumped;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Jump();
    }
    private void Move()
    {
        if (moveDir.magnitude <= 0)     // 안 움직임
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.5f);
        }
        else if (isWalking)             // 움직이는데 걸음
        {
            moveSpeed = walkSpeed;      // Mathf.Lerp(moveSpeed, walkSpeed, 0.5f);
        }
        else                            // 움직이는데 뜀
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.5f);
        }
        if (!isSoundPlay)
        {
            StartCoroutine(MoveRoutine());
            isSoundPlay = true;
        }
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);

        anim.SetFloat("XSpeed", moveDir.x, 0.1f, Time.deltaTime);
        anim.SetFloat("YSpeed", moveDir.y, 0.1f, Time.deltaTime);
        anim.SetFloat("Speed", moveSpeed);

        
    }

    private IEnumerator MoveRoutine()
    {
        if (moveSpeed > walkSpeed)
        {
            if (!isJumping)
                OnWalked?.Invoke();
        }
        yield return new WaitForSeconds(0.3f);
        isSoundPlay = false;
        yield break;
    }

    public void MoveBack(Vector3 dir)
    {
        Debug.Log("hit back");
        StartCoroutine(MoveBackRoutine(dir));
    }

    private IEnumerator MoveBackRoutine(Vector3 dir)
    {
        controller.Move(dir * 50f * Time.deltaTime);
        yield return null;
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y);
    }

    private void Jump()
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (GroundCheck() && ySpeed < 0)
            ySpeed = 0;

        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    IEnumerator JumpRoutine()
    {
        anim.SetTrigger("Jump");
        isJumping = true;
        yield return new WaitForSeconds(jumpTime);
        isJumping = false;
    }

    private void OnJump(InputValue value)
    {
        if (GroundCheck())
            ySpeed = jumpSpeed;
        if (isJumping)
            return;

        OnJumped?.Invoke();

        StartCoroutine(JumpRoutine());
    }

    private bool GroundCheck()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f);
    }

    private void OnWalk(InputValue value)
    {
        isWalking = value.isPressed;
    }
}
