using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TPSCameraController : MonoBehaviour
{
    [SerializeField] Transform cameraRoot;
    [SerializeField] public Transform aimTarget;
    [SerializeField] float lookDistance;
    
    public int mouseSensitivity;
    private Vector2 lookDelta;
    private float xRotation;
    private float yRotation;

    private void Awake()
    {
        mouseSensitivity = 10;
    }
    private void OnEnable()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        Rotate();
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Rotate()
    {
        Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;
        aimTarget.position = lookPoint;
        lookPoint.y = transform.position.y;
        transform.LookAt(lookPoint);
    }

    private void Look()
    {
        yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime;
        xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }

    private void OnMouseSensitivityUp(InputValue value)
    {
        UpdateMouseSensitivityUp();
    }

    private void OnMouseSensitivityDown(InputValue value)
    {
        UpdateMouseSensitivityDown();
    }

    public void UpdateMouseSensitivityUp()
    {
        mouseSensitivity += 1;
    }

    public void UpdateMouseSensitivityDown()
    {
        if (mouseSensitivity <= 1)
            return;
        mouseSensitivity -= 1;
    }
}
