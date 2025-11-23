using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilahAngin : MonoBehaviour, IBossAttack
{
    public Transform arahHadapan;
    public void SetArah(Transform arah)
    {
        arahHadapan = arah;
    }

    public int damage = 20;
    public float radius = 2.5f;
    public float knockbackForce = 0.7f;
    public LayerMask playerLayer;
    

    void Start()
    {
        Invoke("Serang", 0.4f);
    }

    void Serang()
    {
        if (arahHadapan == null)
        {
            Debug.LogWarning("arahHadapan tidak disambungkan ke BilahAngin!");
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
        Debug.Log("Jumlah Collider dikesan oleh BilahAngin: " + hits.Length);

        foreach (Collider2D hit in hits)
        {
            PlayerHealth player = hit.GetComponentInParent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("Damage dihantar kepada: " + hit.name + " sebanyak " + damage);
            }

            Rigidbody2D rb = hit.GetComponentInParent<Rigidbody2D>();
            if (rb != null && arahHadapan != null)
            {
                Vector2 arahTolakan = (transform.position - arahHadapan.position).normalized;
                Vector2 force = arahTolakan * knockbackForce;
                rb.AddForce(force, ForceMode2D.Impulse);
                Debug.Log("Tolakan aktif kepada: " + hit.name);
            }
        }
        Destroy(gameObject, 1f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
