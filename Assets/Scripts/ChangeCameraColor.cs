using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraColor : MonoBehaviour
{
    // [SerializeField] PlayerHealth playerHealth;

    public Color targetColor = Color.red;
    public float transitionDuration = 1f;

    private Camera mainCamera;
    private Color originalColor;
    private float t = 0f;
    private bool isColorChanging = false;

    private void Start()
    {
        mainCamera = Camera.main;
        originalColor = mainCamera.backgroundColor;
    }

    private void Update()
    {
        // if (playerHealth.ApplyDamage == true)
        // {
        //     StartColorChange();
        // }

        if (isColorChanging)
        {
            t += Time.deltaTime / transitionDuration;
            mainCamera.backgroundColor = Color.Lerp(originalColor, targetColor, t);

            if (t >= 1f)
            {
                isColorChanging = false;
                t = 0f;
            }
        }
    }

    public void StartColorChange()
    {
        isColorChanging = true;
        originalColor = mainCamera.backgroundColor;
        t = 0f;
    }
}
