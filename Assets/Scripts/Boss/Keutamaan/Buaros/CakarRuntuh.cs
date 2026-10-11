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
           arahHadapan = transform;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);

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
                }
            }

            Rigidbody2D rb = hit.GetComponentInParent<Rigidbody2D>();
            if (rb != null && arahHadapan != null)
            {
                Vector2 arahTolakan = (transform.position - arahHadapan.position).normalized;
                Vector2 force = arahTolakan * forceAmount;
                rb.AddForce(force, ForceMode2D.Impulse);
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