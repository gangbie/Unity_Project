using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagedEffect : MonoBehaviour
{
    [SerializeField] PlayerHealth player;

    private Image image;

    public float targetAlpha = 0.3f;

    private Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
    }

    private void Start()
    {
        player.OnDamaged.AddListener(() => { DamageEffect(); });
        player.OnHealed.AddListener(() => { HealEffect(); });
    }

    private void DamageEffect()
    {
        StartCoroutine(DamageRoutine());
    }

    private IEnumerator DamageRoutine()
    {
        Color newColor = image.color;
        newColor.a = 0.2f;
        image.color = newColor;
        yield return new WaitForSeconds(0.3f);

        image.color = originalColor;
    }

    private void HealEffect()
    {
        StartCoroutine(HealRoutine());
    }

    private IEnumerator HealRoutine()
    {
        Color newColor = Color.green;
        newColor.a = 0.2f;
        image.color = newColor;
        yield return new WaitForSeconds(0.8f);

        image.color = originalColor;
    }
}
