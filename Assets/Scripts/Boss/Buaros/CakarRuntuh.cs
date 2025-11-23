using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakarRuntuh : MonoBehaviour, IBossAttack
{
    public Transform arahHadapan;
    public void SetArah(Transform arah)
    {
        arahHadapan = arah;
    }
   
    public int damage = 20;
    public float radius = 2f;
    public float forceAmount = 0.5f;
    public LayerMask playerLayer;

    void Start()
    {
        Invoke("Serang", 0.5f);
    }

    void Serang()
    {
        if (arahHadapan == null)
        {
            Debug.LogWarning("arahHadapan tidak disambungkan ke CakarRuntuh!");
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
        Debug.Log("Jumlah Collider dikesan oleh CakarRuntuh: " + hits.Length);

        HashSet<PlayerHealth> pemainSudahKena = new HashSet<PlayerHealth>();

        foreach (Collider2D hit in hits)
        {
            PlayerHealth player = hit.GetComponentInParent<PlayerHealth>();
            if (player != null)
            {
                if (!pemainSudahKena.Contains(player))
                {
                    float jarak = Vector2.Distance(transform.position, hit.transform.position);
                    int finalDamage = jarak < 1f ? damage + 10 : damage;

                    player.TakeDamage(finalDamage);
                    pemainSudahKena.Add(player);
                    Debug.Log("Damage dihantar kepada: " + hit.name + " sebanyak " + damage);
                }
                else
                {
                    Debug.Log("Player sudah kena damage, abaikan collider: " + hit.name);
                }
            }

            Rigidbody2D rb = hit.GetComponentInParent<Rigidbody2D>();
            if (rb != null && arahHadapan != null)
            {
                Vector2 arahTolakan = (transform.position - arahHadapan.position).normalized;
                Vector2 force = arahTolakan * forceAmount;
                rb.AddForce(force, ForceMode2D.Impulse);
                Debug.Log("Tolakan aktif kepada: " + hit.name);
            }
        }
        Destroy(gameObject, 1f);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}