using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NgaumanBumi : MonoBehaviour, IBossAttack
{
   public Transform arahHadapan;
   public void SetArah(Transform arah)
    {
       arahHadapan = arah;
       
    }

    public int damage = 10;
    public float radius = 3f;
    public float forceAmount = 1f;
    public LayerMask playerLayer;

    void Start()
    {
        Invoke("Serang", 0.1f);
    }

    void Serang()
    {
        if (arahHadapan == null)
        {
            Debug.LogWarning("arahHadapan tidak di jumpai");
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
        Debug.Log("Jumlah Collider dikesan oleh NgaumanBumi: " + hits.Length);

        HashSet<PlayerHealth> pemainSudahKena = new HashSet<PlayerHealth>();

        foreach (Collider2D hit in hits)
        {
            PlayerHealth player = hit.GetComponentInParent<PlayerHealth>();
            if (player != null)
            {
                if (!pemainSudahKena.Contains(player))
                {
                    player.TakeDamage(damage);
                    pemainSudahKena.Add(player);
                    Debug.Log("Damage dihantar kepada: " + hit.name + " sebanyak " + damage);
                }
                else
                {
                    Debug.Log("Player sudah kena damage, abaikan collider: " + hit.name);
                }
            }

            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 arahTolakan = (arahHadapan.position - transform.position).normalized;
                Vector2 force = -arahTolakan * forceAmount;
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
