using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar; // Slider untuk bar nyawa

    public GameObject FloatingTextUniversal; // Prefab untuk teks terapung
    public Transform textSpawnPoint; // Titik spawn untuk teks terapung
    public Transform canvasTransform; // Transform kanvas untuk teks terapung

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = 1; // Tetapkan nilai maksimum slider kepada 1
        Debug.Log("Watak bermula dengan nyawa penuh: " + currentHealth);
        UpdateHealthBar();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
        ShowFloatingText(amount);

        if (currentHealth <= 0)
        {
            Debug.Log("Watak mati!");
            // Boleh sambung ke animasi mati atau respawn
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float ratio = (float)currentHealth / (float)maxHealth;
            healthBar.value = ratio;
            Debug.Log("UpdateHealthBar dipanggil. Ratio: " + (ratio * 100) + "%");
        }
           
    }

    void ShowFloatingText(int amount)
    {
        if (Camera.main == null)
        {
            Debug.LogWarning("Camera utama tidak di jumpai.");
            return;
        }

        if (textSpawnPoint == null)
        {
            Debug.LogWarning("textSpawnPoint belum disambung.");
            return;
        }

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(textSpawnPoint.position);
        GameObject go = Instantiate(FloatingTextUniversal, Vector3.zero, Quaternion.identity, canvasTransform);
        go.GetComponent<RectTransform>().position = screenPosition;

        FloatingTextUniversal ft = go.GetComponent<FloatingTextUniversal>();
        if (ft != null)
            ft.ShowDamage(amount, textSpawnPoint);
        Debug.Log("Damage text muncul di: " + textSpawnPoint.position);
    } 


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn(); // Hidupkan semula untuk uji 
        }
    }

    public void Respawn()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        Debug.Log("Watak respawn dengan nyawa penuh.");
    }
}
