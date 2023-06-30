using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamagedUI : MonoBehaviour
{
    public Image flashImage;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private Color originalColor;

    private void Awake()
    {
        originalColor = flashImage.color;
    }

    public void Flash()
    {
        flashImage.color = flashColor;
        Invoke("ResetFlash", flashDuration);
    }

    private void ResetFlash()
    {
        flashImage.color = originalColor;
    }
}

