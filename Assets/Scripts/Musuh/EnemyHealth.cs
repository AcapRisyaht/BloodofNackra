using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color flashColor = Color.red;
    public int maxHealth = 100;
    public GameObject FloatingTextUniversal; // Prefab untuk teks terapung
    public Transform canvasTransform; // Transform kanvas untuk teks terapung
    public Transform textSpawnPoint; // Titik spawn untuk teks terapung
    public Canvas canvas;
    public GameObject BarNyawaPrefab; // Prefab bar nyawa
    public bool damageTextShown = false;
    private Color originalColor;
    private bool isDead = false;
    private Slider nyawaSlider;
    int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        GameObject barClone = Instantiate(BarNyawaPrefab, transform);
        barClone.transform.localPosition = new Vector3(0, 2f, 0.1f); // Letak bar di atas musuh

        nyawaSlider = barClone.GetComponentInChildren<Slider>();
        nyawaSlider.maxValue = maxHealth;
        nyawaSlider.value = currentHealth;
        SetHealth(1f); // Bar nyawa penuh
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        StartCoroutine(FlashColor());

        if (!damageTextShown)
        {
            ShowFloatingText(damage , textSpawnPoint);
            damageTextShown = true;
            StartCoroutine(ResetDamageTextFlag());
        }

        float healthPercent = (float)currentHealth / (float)maxHealth;
        SetHealth(healthPercent);

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    void ShowFloatingText(int amount, Transform spawnPoint)
    {
        if (Camera.main == null || canvasTransform == null || textSpawnPoint == null)
            return;

        Vector3 offset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.1f, 0.1f), 0);
        Vector3 spawnPosition = textSpawnPoint.position + offset;

        spawnPosition.z = 0; // depan kamera 2D

        GameObject go = Instantiate(FloatingTextUniversal, Vector3.zero, Quaternion.identity, canvasTransform);
        go.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(spawnPosition);

        FloatingTextUniversal ft = go.GetComponent<FloatingTextUniversal>();
        if (ft != null)
            ft.ShowDamage(amount, textSpawnPoint);
    }

    public void SetHealth(float HealthPercent)
    {

        if (nyawaSlider != null)
        {
            nyawaSlider.maxValue = maxHealth;
            nyawaSlider.value = currentHealth;
        }
    }

    IEnumerator FlashColor()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }
    void Die()
    {
        if (isDead) return;
        isDead = true;

        // Tunda pemusnahan supaya damage text sempat muncul
        StartCoroutine(DelayedDeath());
    }

    IEnumerator DelayedDeath()
    {
        // Wait for a short duration to allow floating text to display
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    
    IEnumerator ResetDamageTextFlag()
    {
        yield return new WaitForSeconds(0.1f); // Tunggu 1 saat
        damageTextShown = false; // Reset flag
    }
   
}