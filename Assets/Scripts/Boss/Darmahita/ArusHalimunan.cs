using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArusHalimunan : MonoBehaviour, IBossAttack
{
   public Transform arahHadapan;
   public int damage = 20;
   public float speed = 8f;
    public float radius = 1.5f;
    public LayerMask playerLayer;

    public void SetArah(Transform arah)
    {
       arahHadapan = arah;
       
    }

    void Start()
    {
       if (arahHadapan == null)
        {
           Vector2 arah = (arahHadapan.position - transform.position).normalized;
           GetComponent<Rigidbody2D>().velocity = arah * speed;
        }

        Invoke("Serang", 0.3f);
    }

    void Serang()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
        Debug.Log("Jumlah Collider dikesan oleh ArusHalimunan: " + hits.Length);

        foreach (Collider2D hit in hits)
        {
            PlayerHealth player = hit.GetComponentInParent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("Damage dihantar kepada: " + hit.name + " sebanyak " + damage);
            }
        }
        Destroy(gameObject, 1f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
