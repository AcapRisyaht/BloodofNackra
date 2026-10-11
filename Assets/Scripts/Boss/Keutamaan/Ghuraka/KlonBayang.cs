using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlonBayang : MonoBehaviour, IBossAttack
{
    public Transform ArahHadapan;
    public GameObject player;
    public int damage = 15;
    public float speed = 6f;
    public float radius = 1.2f;
    public float duration = 2f;
    public LayerMask playerLayer;

    public void SetArah(Transform arah)
    {
        ArahHadapan = arah;
    }

    void Start ()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (player != null)
        {
            // Salin rupa pemain
            SpriteRenderer srPlayer = player.GetComponentInChildren<SpriteRenderer>();
            SpriteRenderer srKlon = GetComponentInChildren<SpriteRenderer>();
            if (srPlayer != null && srKlon != null)
            {
                srKlon.sprite = srPlayer.sprite;
                srKlon.flipX = srPlayer.flipX;
            }

            // Muncul di posisi cermin dari arahHadapan
            Vector2 arah = (player.transform.position - transform.position).normalized;
            GetComponent<Rigidbody2D>().velocity = arah * speed;
        }

        Invoke("Serang", 0.2f);
        Destroy(gameObject, duration);
    }

    void Serang()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);

        foreach (Collider2D hit in hits)
        {
            PlayerHealth playerHealth = hit.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
